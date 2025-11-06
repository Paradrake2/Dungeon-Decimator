using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;
    public CraftingUI craftingUI;
    public BaseRecipe selectedRecipe;
    public RecipeProgress currentProgress;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRecipe(BaseRecipe recipe, CraftingUI ui)
    {
        selectedRecipe = recipe;
        craftingUI = ui;
        currentProgress = new RecipeProgress(recipe);
    }
    public void ClearRecipe()
    {
        selectedRecipe = null;
        craftingUI = null;
        currentProgress = null;
    }

    public void CraftSelectedRecipe()
    {
        if (selectedRecipe == null || currentProgress == null) return;
        if (!currentProgress.IsComplete())
        {
            // put warning that recipe is incomplete
            Debug.Log("Recipe is not complete.");
            return;
        }
        var requiredMaterials = currentProgress.GetRequiredMaterials();
        foreach (var kvp in requiredMaterials)
        {
            MaterialInventory.Instance.RemoveMaterial(kvp.Key, kvp.Value);
        }


        switch (selectedRecipe.recipeType)
        {
            case RecipeType.CraftingMaterial:
                CraftingFactory.CraftMaterial((CraftingMaterialRecipe)selectedRecipe);
                break;
            case RecipeType.Equipment:
                CraftingFactory.CraftEquipment((EquipmentRecipe)selectedRecipe);
                break;
            default:
                break;
        }
        ClearRecipe();
        craftingUI.PopulateMaterialInventory(craftingUI.craftingStation);
    }
    bool HasRequiredMaterials(BaseRecipe recipe)
    {
        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();
        foreach (var slot in slots)
        {
            if (!slot.isOccupied) return false;
        }
        return true;
    }
    public void TryAddMaterialToRecipe(CraftingMaterial material)
    {
        if (currentProgress == null) return;
        if (MaterialInventory.Instance.GetMaterialAmount(material) <= 0)
        {
            return;
        }

        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (!slot.isOccupied && slot.CanAcceptMaterial(material))
            {
                if (currentProgress.TryAddMaterial(material, out int slotIndex))
                {
                    slots[slotIndex].PlaceMaterialInSlot(material);
                    slots[slotIndex].CheckIfOccupied();
                }
                break;
            }
        }
    }
    public void RemoveMaterialFromRecipe(int slotIndex)
    {
        if (currentProgress == null) return;

        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            var slot = slots[slotIndex];
            var material = slot.GetPlacedMaterial();
            if (material != null)
            {
                currentProgress.RemoveMaterial(slotIndex);
                slots[slotIndex].isOccupied = false;
                slot.RemoveMaterial();
                slots[slotIndex].CheckIfOccupied();
            }
        }
    }

}
