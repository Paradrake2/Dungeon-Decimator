using System.Collections.Generic;
using UnityEngine;


public enum CraftingMaterialRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
public enum MaterialAttributes
{
    None,
    Normal,
    Darkness,
    Light,
    Fire,
    Water,
    Earth,
    Air
}
[System.Serializable]
public class MaterialAttributeStats
{
    public MaterialAttributes[] attribute;
    public float valueMult;
}
[System.Serializable]
public class MaterialStats
{
    public float tier;
    public float goldCost;
    public StatCollection stats = new StatCollection();
}
[CreateAssetMenu(fileName = "CraftingMaterial", menuName = "Scriptable Objects/CraftingMaterial")]
public class CraftingMaterial : ScriptableObject
{
    public Sprite icon;
    public Color itemColor = Color.white;
    public string materialName;
    public string description;
    public string combinationName;
    public CraftingMaterialRarity rarity;
    public MaterialAttributeStats materialStats;
    public MaterialStats stats;
    public CraftingMaterialTag[] tags;
    public CraftingMaterialTag[] secondaryTags;
    public CraftingMaterialTag materialTag;
    public bool isStackable = true;
    public bool equipmentMaterial = true;
    public bool isAlloyable = false;
    public string ID;

    public float GetStatValue(string name)
    {
        return stats.stats.GetStat(name);
    }
    public List<StatValue> GetAllStats()
    {
        return new List<StatValue>(stats.stats.Stats);
    }
    public void SetStatValue(string name, float value)
    {
        var statType = StatDatabase.Instance.GetStat(name);
        if (statType != null)
        {
            stats.stats.SetStat(statType, value);
        }
    }
    public string GetID()
    {
        return ID;
    }
}
