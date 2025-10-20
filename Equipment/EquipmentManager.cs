using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public List<Equipment> equipmentInventory = new List<Equipment>();
    public List<Equipment> equippedItems = new List<Equipment>();
    public Equipment helmet;
    public Equipment armor;
    public Equipment boots;
    public Equipment weapon;
    public Equipment[] accessories = new Equipment[4];

    public PlayerStats playerStats;
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

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }
    }

    public bool EquipItem(Equipment equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentType.Helmet:
                helmet = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                break;
            case EquipmentType.Armor:
                armor = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                break;
            case EquipmentType.Boots:
                boots = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                break;
            case EquipmentType.Weapon:
                weapon = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                break;
            case EquipmentType.Accessory:
                return EquipAccessory(equipment);
            default:
                return false;
        }
        return true;
    }

    private bool EquipAccessory(Equipment accessory)
    {
        for (int i = 0; i < accessories.Length; i++)
        {
            if (accessories[i] == null)
            {
                accessories[i] = accessory;
                equippedItems.Add(accessory);
                equipmentInventory.Remove(accessory);
                return true;
            }
        }
        return false; // No empty slot found
    }

    public Equipment UnequipItem(EquipmentType type, int accessoryIndex = 0)
    {
        Equipment removedEquipment = null;
        switch (type)
        {
            case EquipmentType.Helmet:
                removedEquipment = helmet;
                helmet = null;
                equippedItems.Remove(removedEquipment);
                equipmentInventory.Add(removedEquipment);
                break;
            case EquipmentType.Armor:
                removedEquipment = armor;
                armor = null;
                equippedItems.Remove(removedEquipment);
                equipmentInventory.Add(removedEquipment);
                break;
            case EquipmentType.Boots:
                removedEquipment = boots;
                boots = null;
                equippedItems.Remove(removedEquipment);
                equipmentInventory.Add(removedEquipment);
                break;
            case EquipmentType.Weapon:
                removedEquipment = weapon;
                weapon = null;
                equippedItems.Remove(removedEquipment);
                equipmentInventory.Add(removedEquipment);
                break;
            case EquipmentType.Accessory:
                if (accessoryIndex >= 0 && accessoryIndex < accessories.Length)
                {
                    removedEquipment = accessories[accessoryIndex];
                    accessories[accessoryIndex] = null;
                    equippedItems.Remove(removedEquipment);
                    equipmentInventory.Add(removedEquipment);
                }
                break;
        }
        return removedEquipment;
    }

    public void AddToInventory(Equipment equipment)
    {
        equipmentInventory.Add(equipment);
    }
    public void RemoveFromInventory(Equipment equipment)
    {
        equipmentInventory.Remove(equipment);
    }

    public void ClearInventory()
    {
        equipmentInventory.Clear();
    }

    public StatCollection CalculateEquipmentStats()
    {
        if (playerStats == null) return null;
        StatCollection totalStats = new StatCollection();
        if (helmet != null) totalStats.AddStats(helmet.stats);
        if (armor != null) totalStats.AddStats(armor.stats);
        if (boots != null) totalStats.AddStats(boots.stats);
        if (weapon != null) totalStats.AddStats(weapon.stats);
        foreach (var accessory in accessories)
        {
            if (accessory != null)
            {
                totalStats.AddStats(accessory.stats);
            }
        }

        return totalStats;
    }

}
