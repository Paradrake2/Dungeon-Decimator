using UnityEngine;

public class HotbarSlot : MonoBehaviour
{
    public Equipment equippedItem;
    public int slotIndex;
    public Equipment GetEquipment()
    {
        if (equippedItem == null)
        {
            return null;
        }
        return equippedItem;
    }
    public int SetIndex(int index)
    {
        slotIndex = index;
        return slotIndex;
    }
}
