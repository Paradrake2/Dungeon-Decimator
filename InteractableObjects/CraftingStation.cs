using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CraftingStationType
{
    EquipmentCrafter,
    MaterialCrafter,
    Both
}


public class CraftingStation : MonoBehaviour
{
    public CraftingStationType stationType;
    public GameObject craftingUI;
    [SerializeField] private GameObject currentUI;
    public MaterialInventory inventory;
    public virtual void Interact()
    {
        OpenCraftingUI();
    }
    public void OpenCraftingUI()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        GameObject ui = Instantiate(craftingUI, canvas.transform);
        ui.GetComponent<CraftingUI>().Initialize(this);
        currentUI = ui;
    }
    public void CloseCraftingUI()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }
}
