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
    public Equipment weapon2;
    public Equipment[] accessories = new Equipment[4];

    public PlayerStats playerStats;

    [Header("Cached Equipment Stats")]
    public StatCollection cachedEquipmentStats;

    public System.Action OnEquipmentChanged;
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
    public void UpdateCachedStats()
    {
        cachedEquipmentStats = CalculateEquipmentStats();
    }
    public StatCollection GetEquipmentStats()
    {
        return cachedEquipmentStats;
    }
    public bool EquipItem(Equipment equipment)
    {
        bool equipSucceeded = false;
        switch (equipment.equipmentType)
        {
            case EquipmentType.Helmet:
                helmet = equipment;

                equipSucceeded = true;
                break;
            case EquipmentType.Armor:
                armor = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                equipSucceeded = true;
                break;
            case EquipmentType.Boots:
                boots = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                equipSucceeded = true;
                break;
            case EquipmentType.Weapon:
                weapon = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                equipSucceeded = true;
                break;
            case EquipmentType.Weapon2:
                weapon2 = equipment;
                equippedItems.Add(equipment);
                equipmentInventory.Remove(equipment);
                equipSucceeded = true;
                break;
            case EquipmentType.Accessory:
                equipSucceeded =  EquipAccessory(equipment);
                break;
            default:
                return false;
        }
        if (equipSucceeded)
        {
            UpdateCachedStats();
            OnEquipmentChanged?.Invoke();
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
                break;
            case EquipmentType.Armor:
                removedEquipment = armor;
                armor = null;
                break;
            case EquipmentType.Boots:
                removedEquipment = boots;
                boots = null;
                break;
            case EquipmentType.Weapon:
                removedEquipment = weapon;
                weapon = null;
                break;
            case EquipmentType.Accessory:
                if (accessoryIndex >= 0 && accessoryIndex < accessories.Length)
                {
                    removedEquipment = accessories[accessoryIndex];
                    accessories[accessoryIndex] = null;
                }
                break;
        }
        if (removedEquipment != null)
        {
            equippedItems.Remove(removedEquipment);
            equipmentInventory.Add(removedEquipment);
            UpdateCachedStats();
            OnEquipmentChanged?.Invoke();
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
