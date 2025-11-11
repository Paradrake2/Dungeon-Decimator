using System;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Armor,
    Boots,
    Weapon,
    Accessory
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    public EquipmentType equipmentType;
    public Sprite icon;
    public StatCollection stats = new StatCollection();
    public int levelRequirement = 1;
    public string ID;
    public List<StatValue> GetAllStats()
    {
        return new List<StatValue>(stats.Stats);
    }
}
