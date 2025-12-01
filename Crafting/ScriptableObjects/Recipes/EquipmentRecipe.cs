using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentIngredient
{
    public CraftingMaterialTag materialTag;
    public CraftingMaterial material;
    public int quantity;
}


[CreateAssetMenu(fileName = "EquipmentRecipe", menuName = "Recipes/EquipmentRecipe")]
public class EquipmentRecipe : BaseRecipe
{
    public EquipmentIngredient[] ingredients;
    public GameObject projectileGameObject = null;
    public Equipment baseEquipment;
    public float tier;
    public float statMultiplier = 0.5f;
    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab, CraftingUI cUI)
    {
        foreach (var ingredient in ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(slotPrefab, parent);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                if (ingredient.material != null && (ingredient.materialTag == null))
                {
                    materialSlot.SetupSpecificMaterialSlot(ingredient.material, cUI);
                    continue;
                } else if (ingredient.materialTag != null && ingredient.material == null)
                {
                    materialSlot.SetupTagBasedSlot(ingredient.materialTag, cUI);
                    continue;
                } else if (ingredient.materialTag != null && ingredient.material != null)
                {
                    materialSlot.SetupSpecificTagMaterialSlot(ingredient.material, ingredient.materialTag, cUI);
                    continue;
                }
            }
        }
    }
    public override void Craft()
    {
        Equipment resultEquipment = Instantiate(baseEquipment);
        var usedMaterials = CraftingManager.Instance.currentProgress.GetRequiredMaterials();
        List<StatValue> baseStats = baseEquipment.GetAllStats();
        string ID = baseEquipment.ID;
        foreach (var material in usedMaterials)
        {
            List<StatValue> stats = material.Key.GetAllStats();
            foreach (var stat in stats)
            {
                var baseStat = baseStats.Find(s => s.StatType == stat.StatType);
                if (baseStat != null)
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    float newValue = baseStat.Value + addedValue;
                    resultEquipment.stats.SetStat(stat.StatType, newValue);
                }
                else
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    resultEquipment.stats.SetStat(stat.StatType, addedValue);
                }
            }
            ID += material.Key.GetID();
        }
        if (projectileGameObject != null)
        {
            resultEquipment.playerProjectile = projectileGameObject;
        }
        EquipmentManager.Instance.AddToInventory(resultEquipment);
        EquipmentManager.Instance.EquipItem(resultEquipment);
    }
    public override List<StatValue> GetPreviewStats(Dictionary<CraftingMaterial, int> placedMaterials)
    {
        List<StatValue> previewStats = new List<StatValue>();
        List<StatValue> baseStats = baseEquipment.GetAllStats();
        foreach (var material in placedMaterials)
        {
            List<StatValue> stats = material.Key.GetAllStats();
            foreach (var stat in stats)
            {
                var baseStat = baseStats.Find(s => s.StatType == stat.StatType);
                if (baseStat != null)
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    float newValue = baseStat.Value + addedValue;
                    previewStats.Add(new StatValue(stat.StatType, newValue));
                }
                else
                {
                    float addedValue = stat.Value * statMultiplier * material.Value;
                    previewStats.Add(new StatValue(stat.StatType, addedValue));
                }
            }
        }
        return previewStats;
    }
}

