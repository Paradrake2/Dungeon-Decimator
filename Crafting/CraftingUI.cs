using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public GameObject UIParentPrefab;
    public GameObject materialRecipeButtonPrefab;
    public GameObject materialButtonPrefab;
    public Transform materialInventoryHolder;
    public MaterialInventory inventory;
    public void Initialize(CraftingStation station)
    {
        if (inventory == null)
        {
            inventory = MaterialInventory.Instance;
        }
        PopulateMaterialInventory(station);
        
    }

    public void PopulateMaterialInventory(CraftingStation station)
    {
        if (station.stationType != CraftingStationType.EquipmentCrafter)
        {
            // Populate material inventory UI
            foreach (var material in inventory.materials)
            {
                var button = Instantiate(materialButtonPrefab, materialInventoryHolder);
                button.GetComponent<MaterialButton>().Initialize(material.material, material.amount, button.transform.parent);
            }
        }
        else // is equipment crafter, filter by equipmentMaterial
        {
            foreach (var material in inventory.materials)
            {
                if (material.material.equipmentMaterial)
                {
                    var button = Instantiate(materialButtonPrefab, materialInventoryHolder);
                    button.GetComponent<MaterialButton>().Initialize(material.material, material.amount, button.transform.parent);
                }
            }
        }
    }
    
    public void SetupCraftingArea()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
