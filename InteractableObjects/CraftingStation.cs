using System.Collections.Generic;
using UnityEngine;



public abstract class CraftingStation : MonoBehaviour
{
    public CraftingStationType stationType;
    public GameObject craftingUI;
    [SerializeField] private GameObject currentUI;
    public MaterialInventory inventory;
    public bool isTagBased = false;
    public PlayerStats stats;
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
        stats.canAttack = false;
        currentUI = ui;
    }
    public void CloseCraftingUI()
    {
        stats.canAttack = true;
        CraftingManager.Instance.ClearTempMaterials();
        if (currentUI != null)
        {
            Destroy(currentUI);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetPlayerStats();
            Interact();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloseCraftingUI();
        }
    }
    void GetPlayerStats()
    {
        if (stats == null)
        {
            stats = FindFirstObjectByType<PlayerStats>();
        }
    }
    public abstract List<BaseRecipe> GetAvailableRecipes();
    public abstract List<CraftingMaterial> GetAvailableMaterials();
    public void CraftedItemsCheck(List<CraftingMaterial> mat, bool? condition)
    {
        if (MaterialInventory.Instance != null)
        {
            if ((bool)condition)
            {
                foreach (var material in MaterialInventory.Instance.materials)
                {
                    if (material.amount > 0 && !mat.Contains(material.material))
                    {
                        mat.Add(material.material);
                    }
                }
            }
        }
    }
}
