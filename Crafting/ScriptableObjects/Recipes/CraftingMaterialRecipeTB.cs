using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CraftingMaterialTBIngredient
{
    public CraftingMaterialTag materialTag;
    public CraftingMaterial specificMaterial;
    public int quantity;
    public bool useTag = false;
}




[CreateAssetMenu(fileName = "CraftingMaterialRecipeTB", menuName = "Scriptable Objects/CraftingMaterialRecipeTB")]
public class CraftingMaterialRecipeTB : BaseRecipe
{

    public CraftingMaterialTBIngredient[] ingredients;

    public CraftingMaterial baseMaterial;
    public int resultQuantity = 1;
    public float statMultiplier = 0.25f;
    public bool overwriteColor = false;
    public StatType[] blacklistedStats;
    public override void Craft()
    {
        CraftingMaterial resultMaterial = Instantiate(baseMaterial);
        var usedMaterials = CraftingManager.Instance.currentProgress.GetRequiredMaterials();
        List<StatValue> baseStats = baseMaterial.GetAllStats();
        foreach (var material in usedMaterials)
        {
            List<StatValue> stats = material.Key.GetAllStats();
            foreach (var stat in stats)
            {
                if (blacklistedStats != null && blacklistedStats.Contains(stat.StatType))
                {
                    continue;
                }
                var baseStat = baseStats.Find(s => s.StatType == stat.StatType);
                if (baseStat != null)
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    float newValue = baseStat.Value + addedValue;
                    resultMaterial.stats.stats.SetStat(stat.StatType, newValue);
                }
                else
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    resultMaterial.stats.stats.SetStat(stat.StatType, addedValue);
                }
            }
        }
        resultMaterial.name = usedMaterials.First().Key.combinationName + baseMaterial.combinationName;
        if (overwriteColor)
        {
            resultMaterial.itemColor = usedMaterials.First().Key.itemColor;
        }
        MaterialInventory.Instance.AddMaterial(resultMaterial, resultQuantity);
        CraftingManager.Instance.UpdateMaterialButton(resultMaterial);
    }

    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab, CraftingUI cUI)
    {
        foreach (var ingredient in ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(slotPrefab, parent);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                if (ingredient.specificMaterial != null && (ingredient.materialTag == null || ingredient.useTag == false ))
                {
                    materialSlot.SetupSpecificMaterialSlot(ingredient.specificMaterial, cUI);
                    continue;
                } else if (ingredient.materialTag != null && ingredient.specificMaterial == null)
                {
                    materialSlot.SetupTagBasedSlot(ingredient.materialTag, cUI);
                    continue;
                } else if (ingredient.materialTag != null && ingredient.specificMaterial != null)
                {
                    materialSlot.SetupSpecificTagMaterialSlot(ingredient.specificMaterial, ingredient.materialTag, cUI);
                    continue;
                }

            }
        }
    }
}
