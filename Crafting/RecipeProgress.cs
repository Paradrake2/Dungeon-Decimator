using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RecipeSlotData
{
    public CraftingMaterial placedMaterial;
    public CraftingMaterial requiredMaterial;
    public CraftingMaterialTag[] requiredTag;
    public CraftingMaterialTag requiredMaterialTag;
    public bool isTagBased;
    public bool isFilled;
    public bool isDynamicSlot;
    public bool isMultiTag;
}



public class RecipeProgress
{
    public BaseRecipe recipe;
    public List<RecipeSlotData> slots = new List<RecipeSlotData>();
    public Dictionary<CraftingMaterial, int> placedMaterials = new Dictionary<CraftingMaterial, int>();

    public RecipeProgress(BaseRecipe recipe) {
        this.recipe = recipe;
        InitializeSlots();
    }
    public void InitializeSlots()
    {
        slots.Clear();

        switch (recipe.recipeType)
        {
            case RecipeType.CraftingMaterial:
                SetupCraftingMaterialRecipe();
                break;
            case RecipeType.Equipment:
                SetupEquipmentRecipe();
                break;
            case RecipeType.CraftingMaterialTB:
                SetupCraftingMaterialTBRecipe();
                break;
            case RecipeType.MonoItem:
                SetupMonoItemRecipe();
                break;
            case RecipeType.TagBasedSpecificMaterialRecipe:
                SetupTagBasedSpecificMaterialRecipe();
                break;
        }

    }
    public void SetupCraftingMaterialRecipe()
    {
        var matRecipe = (CraftingMaterialRecipe)recipe;
        foreach (var ingredient in matRecipe.ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                slots.Add(new RecipeSlotData
                {
                    requiredMaterial = ingredient.material,
                    isTagBased = false,
                    isFilled = false
                });
            }
        }
    }
    public void SetupEquipmentRecipe()
    {
        var equipRecipe = (EquipmentRecipe)recipe;
        foreach (var ingredient in equipRecipe.ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                if (recipe.isTagBased)
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredTag = new CraftingMaterialTag[] { ingredient.materialTag },
                        isTagBased = true,
                        isFilled = false
                    });
                }
                else // for equipment crafted with specific items
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredMaterial = ingredient.material,
                        isTagBased = false,
                        isFilled = false
                    });
                }
            }
        }
    }
    public void SetupCraftingMaterialTBRecipe()
    {
        var matRecipe = (CraftingMaterialRecipeTB)recipe;
        foreach (var ingredient in matRecipe.ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                if (ingredient.useTag)
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredTag = new CraftingMaterialTag[] { ingredient.materialTag },
                        isTagBased = true,
                        isFilled = false
                    });

                }
                else
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredMaterial = ingredient.specificMaterial,
                        isTagBased = false,
                        isFilled = false
                    });
                }
                Debug.Log(slots[slots.Count - 1].isTagBased);
            }
        }
    }

    public void SetupMonoItemRecipe()
    {
        var matRecipe = (ModularMonoItemRecipe)recipe;
        foreach (var ingredient in matRecipe.ingredient)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                slots.Add(new RecipeSlotData
                {
                    requiredTag = new CraftingMaterialTag[] { ingredient.materialTag },
                    isTagBased = false,
                    isFilled = false,
                    isDynamicSlot = true
                });
            }
        }
    }
    public void SetupTagBasedSpecificMaterialRecipe()
    {
        var tagBasedRecipe = (TagBasedSpecificMaterialRecipe)recipe;
        foreach (var ingredient in tagBasedRecipe.ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                if (ingredient.specificMaterial != null)
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredMaterial = ingredient.specificMaterial,
                        isTagBased = false,
                        isFilled = false
                    });
                }
                else
                {
                    slots.Add(new RecipeSlotData
                    {
                        requiredTag = ingredient.materialTags,
                        requiredMaterialTag = ingredient.materialTypeTag,
                        isTagBased = true,
                        isFilled = false,
                        isMultiTag = true
                    });
                }
            }
        }
    }
    public bool TryAddMaterial(CraftingMaterial material, out int slotIndex)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (!slot.isFilled && CanMaterialFitSlot(material, slot))
            {
                Debug.LogWarning("Can add material");
                slot.placedMaterial = material;
                slot.isFilled = true;
                if (placedMaterials.ContainsKey(material))
                {
                    placedMaterials[material]++;
                }
                else
                {
                    placedMaterials[material] = 1;
                    Debug.Log(placedMaterials[material]);
                }
                slotIndex = i;
                return true;
            }
        }
        slotIndex = -1;
        return false;
    }

    public void RemoveMaterial(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count) {
            var slot = slots[slotIndex];
            if (slot.isFilled)
            {
                var material = slot.placedMaterial;
                slot.placedMaterial = null;
                slot.isFilled = false;

                if (placedMaterials.ContainsKey(material))
                {
                    placedMaterials[material]--;
                    if (placedMaterials[material] <= 0) 
                    {
                        placedMaterials.Remove(material);
                    }
                }
            }
        }
    }

    bool CanMaterialFitSlot(CraftingMaterial material, RecipeSlotData slot)
    {
        if (slot.isDynamicSlot)
        {
            Debug.Log("Dynamic slot check");
            if (placedMaterials.Count == 0)
            {
                return true;
            } else
            {
                return placedMaterials.ContainsKey(material);
            }
        }
        if (slot.isTagBased)
        {
            // Check if material has any of the required tags
            bool hasTag = false;

            if (material.tags != null && material.tags.Count() > 0)
            {
                foreach (var materialTag in material.tags)
                {
                    if (slot.requiredTag.Contains(materialTag))
                    {
                        hasTag = true;
                        break;
                    }
                }
            }

            Debug.Log($"Material: {material.materialName}, Required Tag: {slot.requiredTag}, Has Tag: {hasTag}");
            Debug.Log($"Material Tags: {string.Join(", ", material.tags.ToList() ?? new List<CraftingMaterialTag>())}");

            return hasTag;
        }
        if (slot.isMultiTag)
        {
            foreach (var tag in slot.requiredTag)
            {
                if (!material.tags.Contains(tag))
                {
                    Debug.LogWarning(material.ToString() + " AAAAAAAAAAAA");
                    return false;
                }
            }
            if (!material.materialTag.Equals(slot.requiredMaterialTag))
            {
                Debug.LogWarning(material.ToString() + " BBBBBBBBBB");
                return false;
            }
            return true;
        }
        else
        {
            Debug.LogWarning(material.ToString() == slot.requiredMaterial + " AAAAAAAAAAAA");
            return material == slot.requiredMaterial;
        }
    }

    public bool IsComplete()
    {
        return slots.TrueForAll(slot => slot.isFilled);
    }

    public Dictionary<CraftingMaterial, int> GetRequiredMaterials()
    {
        return new Dictionary<CraftingMaterial, int>(placedMaterials);
    }

}
