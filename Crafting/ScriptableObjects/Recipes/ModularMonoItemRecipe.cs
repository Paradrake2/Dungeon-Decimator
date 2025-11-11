using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ModularMonoItemRecipe", menuName = "Recipes/ModularMonoItemRecipe")]
public class ModularMonoItemRecipe : BaseRecipe
{
    public CraftingMaterial resultMaterial;
    public CraftingMaterialTBIngredient[] ingredient;
    public CraftingMaterial baseMaterial;
    public int resultQuantity = 1;
    public float statMultiplier = 0.25f;
    public bool overwriteColor = false;
    public StatType[] blacklistedStats;

    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab, CraftingUI cUI)
    {
        foreach (var ingredient in ingredient)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(slotPrefab, parent);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                materialSlot.SetupMonoItemSlot(cUI, isTagBased, ingredient.specificMaterial, ingredient.materialTag);
            }
        }
    }
    public override void Craft()
    {
        CraftingMaterial resultMaterial = Instantiate(baseMaterial);
        var usedMaterials = CraftingManager.Instance.currentProgress.GetRequiredMaterials();
        List<StatValue> baseStats = baseMaterial.GetAllStats();
        string ID = baseMaterial.ID;
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
                ID += material.Key.GetID();
            }
        }
        resultMaterial.name = usedMaterials.First().Key.combinationName + baseMaterial.combinationName;
        resultMaterial.ID = ID;
        if (overwriteColor)
        {
            resultMaterial.itemColor = usedMaterials.First().Key.itemColor;
        }
        resultMaterial.materialTag = usedMaterials.First().Key.materialTag;
        MaterialInventory.Instance.AddMaterial(resultMaterial, resultQuantity);
        CraftingManager.Instance.UpdateMaterialButton(resultMaterial);
    }

}
