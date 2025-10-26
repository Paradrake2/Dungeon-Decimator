using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    Wood,
    Stone,
    Metal,
    Fabric,
    Leather,
    Gemstone,
    MonsterComponent,
    Ore
}
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
    public MaterialAttributes attribute;
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
    public string materialName;
    public CraftingMaterialRarity rarity;
    public MaterialType materialType;
    public MaterialAttributeStats materialStats;
    public MaterialStats stats;
}
