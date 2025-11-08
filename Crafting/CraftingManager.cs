using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;
    public CraftingUI craftingUI;
    public BaseRecipe selectedRecipe;
    public RecipeProgress currentProgress;
    [SerializeField] private List<CraftingMaterial> tempMaterials;
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

    public void SetRecipe(BaseRecipe recipe, CraftingUI ui)
    {
        selectedRecipe = recipe;
        craftingUI = ui;
        currentProgress = new RecipeProgress(recipe);
    }
    public void ClearRecipe()
    {
        selectedRecipe = null;
        craftingUI = null;
        currentProgress = null;
    }

    public void CraftSelectedRecipe()
    {
        if (selectedRecipe == null || currentProgress == null) return;
        if (!currentProgress.IsComplete())
        {
            // put warning that recipe is incomplete
            Debug.Log("Recipe is not complete.");
            return;
        }
        selectedRecipe.Craft();
        /**
        switch (selectedRecipe.recipeType)
        {
            case RecipeType.CraftingMaterial:
                CraftingFactory.CraftMaterial((CraftingMaterialRecipe)selectedRecipe);
                break;
            case RecipeType.Equipment:
                CraftingFactory.CraftEquipment((EquipmentRecipe)selectedRecipe);
                break;
            case RecipeType.Alloying:
                CraftingFactory.AlloyMaterial((CraftingMaterialRecipe)selectedRecipe);
                break;
            default:
                break;
        }
        **/
        craftingUI.PopulateMaterialInventory(craftingUI.craftingStation);
        RefillRecipeAutomatically();
        //ClearRecipe();
    }
    void RefillRecipeAutomatically()
    {
        if (selectedRecipe == null) return;

        currentProgress = new RecipeProgress(selectedRecipe);
        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();
        foreach (var slot in slots)
        {
            slot.RemoveMaterial();
        }

        if (CheckIfEnoughMaterials(tempMaterials)) AutoFillRecipe();
    }

    void AutoFillRecipe()
    {
        var materialsToProcess = new List<CraftingMaterial>(tempMaterials);

        tempMaterials.Clear();

        foreach (CraftingMaterial material in materialsToProcess)
        {
            TryAddMaterialToRecipe(material);
        }
    }
    bool CheckIfEnoughMaterials(List<CraftingMaterial> tempMaterials)
    {
        Dictionary<CraftingMaterial, int> requiredMaterials = new Dictionary<CraftingMaterial, int>();

        foreach (var material in tempMaterials)
        {
            if (requiredMaterials.ContainsKey(material)) requiredMaterials[material]++;
            else
            {
                requiredMaterials[material] = 1;
            }
        }

        foreach (var kvp in requiredMaterials)
        {
            CraftingMaterial material = kvp.Key;
            int required = kvp.Value;
            int available = MaterialInventory.Instance.GetMaterialAmount(material);

            if (available < required) return false;
        }
        return true;
    }
    public void TryAddMaterialToRecipe(CraftingMaterial material)
    {
        if (currentProgress == null) return;
        if (MaterialInventory.Instance.GetMaterialAmount(material) <= 0)
        {
            return;
        }

        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (!slot.isOccupied && slot.CanAcceptMaterial(material))
            {
                if (currentProgress.TryAddMaterial(material, out int slotIndex))
                {
                    slots[i].PlaceMaterialInSlot(material);
                    slots[i].CheckIfOccupied();
                    tempMaterials.Add(material);
                    MaterialInventory.Instance.RemoveMaterial(material, 1);
                    UpdateMaterialButton(material, -1);
                }
                break;
            }
        }
    }
    public void RemoveMaterialFromRecipe(int slotIndex)
    {
        if (currentProgress == null) return;

        var slots = craftingUI.recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            var slot = slots[slotIndex];
            var material = slot.GetPlacedMaterial();
            if (material != null)
            {
                currentProgress.RemoveMaterial(slotIndex);
                slots[slotIndex].isOccupied = false;
                slot.RemoveMaterial();
                slots[slotIndex].CheckIfOccupied();
                tempMaterials.Remove(material);
                MaterialInventory.Instance.AddMaterial(material, 1);
                UpdateMaterialButton(material, 1);
            }
        }
    }
    public void SetUI(CraftingUI ui)
    {
        craftingUI = ui;
    }
    public void ClearUI()
    {
        craftingUI = null;
    }

    void UpdateMaterialButton(CraftingMaterial material, int delta)
    {
        var materialButtons = craftingUI.materialInventoryHolder.GetComponentsInChildren<MaterialButton>();

        foreach (var button in materialButtons)
        {
            if (button.material == material)
            {
                button.UpdateAmount(delta);
                return;
            }
        }

        if (delta > 0)
        {
            CreateMaterialButton(material);
        }
    }

    void CreateMaterialButton(CraftingMaterial material)
    {
        int amount = MaterialInventory.Instance.GetMaterialAmount(material);
        if (amount > 0)
        {
            var button = Instantiate(craftingUI.materialButtonPrefab, craftingUI.materialInventoryHolder);
            button.GetComponent<MaterialButton>().Initialize(material, amount, craftingUI.materialInventoryHolder);
        }
    }

}
