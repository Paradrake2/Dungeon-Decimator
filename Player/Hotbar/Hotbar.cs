using UnityEngine;



public class Hotbar : MonoBehaviour
{
    public static Hotbar Instance;
    public int selectedIndex = 0;
    public int slotNum = 5;
    public GameObject[] slots = new GameObject[5];
    public GameObject slotPrefab;
    /**
    public void AddEquipmentToSlot(Equipment equipment, int index)
    {
        if (index < 0 || index >= slotNum)
        {
            Debug.LogWarning("Invalid hotbar slot index");
            return;
        }
        slots[index].equippedItem = equipment;
    }
    **/
    public void AddEquipmentToSlot(Equipment equipment)
    {
        for (int i = 0; i < slotNum; i++)
        {
            if (slots[i].GetComponent<HotbarSlot>().equippedItem == null)
            {
                slots[i].GetComponent<HotbarSlot>().equippedItem = equipment;
                return;
            }
        }
        Debug.LogWarning("No empty hotbar slots available");
        // HandleNoSlots();
    }
    public void RemoveEquipmentFromSlot(int index)
    {
        if (index < 0 || index >= slotNum)
        {
            Debug.LogWarning("Invalid hotbar slot index");
            return;
        }
        slots[index].GetComponent<HotbarSlot>().equippedItem = null;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitializeSlots();
    }
    public Equipment GetHeldItem()
    {
        if (slots[selectedIndex] != null)
        {
            return slots[selectedIndex].GetComponent<HotbarSlot>().GetEquipment();
        }
        return null;
    }
    public void InitializeSlots()
    {
        for (int i = 0; i < slotNum; i++)
        {
            slots[i] = Instantiate(slotPrefab, transform);
            SetSlot(slots[i], i);
            Debug.LogWarning("Making slot");
            slots[i].gameObject.GetComponent<HotbarSlot>().SetIndex(i);
            slots[i].gameObject.GetComponent<HotbarSlotUI>().InitializeSlot(i);
        }
        Combat combat = FindFirstObjectByType<Combat>();
        slots[0].gameObject.GetComponent<HotbarSlot>().equippedItem = combat.basicEquipment;
        slots[0].gameObject.GetComponent<HotbarSlotUI>().UpdateSlotImage(combat.basicEquipment.icon);
    }
    public void SetSlot(GameObject slot, int index)
    {
        if (index < 0 || index >= slotNum)
        {
            Debug.LogWarning("Invalid hotbar slot index");
            return;
        }
        slots[index] = slot;
    }
}
