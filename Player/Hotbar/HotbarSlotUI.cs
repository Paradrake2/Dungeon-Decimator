using System;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    public Image slotImage;
    public Equipment equipmentInSlot;
    public Button button;
    public int slotIndex = 0;

    public void UpdateSlotImage(Sprite icon)
    {
        slotImage.sprite = icon;
    }
    void Start()
    {
        
    }
    public void InitializeSlot(int index)
    {
        slotIndex = index;
        button.onClick.AddListener(OnSlotClicked);
        equipmentInSlot = Hotbar.Instance.slots[index].GetComponent<HotbarSlot>().equippedItem;
        if (equipmentInSlot != null)
        {
            UpdateSlotImage(equipmentInSlot.icon);
        }
    }

    public void OnSlotClicked()
    {
        Combat playerCombat = FindFirstObjectByType<Combat>();
        playerCombat.ChangeSelectedSlotIndex(slotIndex);
    }
}
