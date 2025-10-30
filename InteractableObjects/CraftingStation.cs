using System.Collections.Generic;
using UnityEngine;



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
    void FindUI()
    {
        if (craftingUI == null)
        {
            craftingUI = Resources.Load<GameObject>("Prefabs/UI/CraftingUI");
        }
    }
    public void OpenCraftingUI()
    {
        FindUI();
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
