using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject UIParentPrefab;
    public GameObject materialButtonPrefab;
    public GameObject recipeMaterialButtonPrefab;
    public Transform materialInventoryHolder;
    public Transform recipeListHolder;
    public MaterialInventory inventory;
    public CraftingStationType craftingStationType;
    public PlayerStats stats;
    public CraftingUI Instance;
    [SerializeField] private BaseRecipe selectedRecipe;
    [SerializeField] private CraftingStation craftingStation;
    public Dictionary<CraftingMaterial, int> tempMaterialInventory = new Dictionary<CraftingMaterial, int>();

    public void Initialize(CraftingStation station)
    {
        NullChecks();
        craftingStation = station;
        craftingStationType = station.stationType;
        SetupCraftingArea(station);
        PopulateMaterialInventory(station);
    }
    void NullChecks()
    {
        if (inventory == null)
        {
            inventory = MaterialInventory.Instance;
        }
        if (stats == null) {
            stats = FindFirstObjectByType<PlayerStats>();
        }
    }
    public void PopulateMaterialInventory(CraftingStation station)
    {
        foreach (Transform child in materialInventoryHolder)
        {
            Destroy(child.gameObject);
        }
        switch (station.stationType.ToString())
        {
            case "MaterialCrafter":
                SetupMaterialInventory();
                break;
            case "EquipmentCrafter":
                SetupEquipmentInventory();
                break;
            default:
                SetupMaterialInventory();
                break;
        }
    }

    public void SetupCraftingArea(CraftingStation station)
    {
        foreach (Transform child in recipeListHolder)
        {
            Destroy(child.gameObject);
        }
        // Populate recipe list UI
        switch (station.stationType.ToString())
        {
            case "MaterialCrafter":
                SetupMaterialCrafter();
                break;
            case "EquipmentCrafter":
                SetupEquipmentCrafter();
                break;
            default:
                SetupMaterialCrafter();
                break;
        }
    }

    void SetupMaterialInventory()
    {
        foreach (var material in inventory.materials)
        {
            var button = Instantiate(materialButtonPrefab, materialInventoryHolder);
            button.GetComponent<MaterialButton>().Initialize(material.material, material.amount, button.transform.parent);
        }
    }
    void SetupEquipmentInventory()
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
    void SetupMaterialCrafter()
    {
        foreach (var recipe in Resources.LoadAll<CraftingMaterialRecipe>("Recipes/CraftingMaterials"))
        {
            if (!IsUnlocked(recipe)) continue;
            var button = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
            button.GetComponent<Button>().onClick.AddListener(() => OnRecipeButtonClick(recipe));
            button.GetComponent<Image>().sprite = recipe.icon;
        }
    }
    void SetupEquipmentCrafter()
    {
        foreach (var recipe in Resources.LoadAll<EquipmentRecipe>("Recipes/EquipmentRecipes"))
        {
            if (!IsUnlocked(recipe)) continue;
            var button = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
            button.GetComponent<Button>().onClick.AddListener(() => OnRecipeButtonClick(recipe));
            button.GetComponent<Image>().sprite = recipe.icon;
        }
    }
    public void OnRecipeButtonClick(BaseRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateCraftingPreview(recipe);
    }

    void UpdateCraftingPreview(BaseRecipe recipe)
    {
        foreach (Transform child in recipeListHolder)
        {
            Destroy(child.gameObject);
        }
        if (recipe.recipeUIElementPrefab != null)
        {
            var _recipeUI = Instantiate(recipe.recipeUIElementPrefab, recipeListHolder);
        }
        else
        {
            GenerateDynamicRecipeUI(recipe);
        }
    }

    void GenerateDynamicRecipeUI(BaseRecipe recipe)
    {
        switch (recipe.recipeType)
        {
            case RecipeType.CraftingMaterial:
                GenerateMaterialRecipeUI((CraftingMaterialRecipe)recipe);
                break;
            case RecipeType.Equipment:
                GenerateEquipmentRecipeUI((EquipmentRecipe)recipe);
                break;
            default:
                break;
        }
    }

    void GenerateEquipmentRecipeUI(EquipmentRecipe recipe)
    {
        GameObject container = new GameObject("RecipeHolder");
        container.transform.SetParent(recipeListHolder, false);

        var gridLayout = container.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(100, 100);
        gridLayout.spacing = new Vector2(5, 5);

        foreach (var ingredient in recipe.ingredients)
        {
            CreateTagBasedSlots(container.transform, ingredient);
        }
    }
    void CreateTagBasedSlots(Transform parent, EquipmentIngredient requiredMaterial)
    {
        for (int i = 0; i < requiredMaterial.quantity; i++)
        {
            GameObject slot = Instantiate(recipeMaterialButtonPrefab, parent);
            var tagSlot = slot.GetComponent<RecipeMaterialSlot>();
            tagSlot.SetupTagBasedSlot(requiredMaterial.materialTag);
        }
    }

    void GenerateMaterialRecipeUI(CraftingMaterialRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                materialSlot.SetupSpecificMaterialSlot(ingredient.material);
                
            }
            tempMaterialInventory.AddRange(recipe.ingredients.ToDictionary(ing => ing.material, ing => ing.quantity));
        }
    }
    bool IsUnlocked(BaseRecipe recipe)
    {
        return stats.GetStatValue("Level") >= recipe.levelRequirement;
    }
    public void ResetStation() // Called when closing the crafting UI
    {
        craftingStationType = null;
        selectedRecipe = null;
        craftingStation = null;
    }
    public void CraftSelectedRecipe()
    {
        if (selectedRecipe == null) return;
        if (HasRequiredMaterials(selectedRecipe) == false) return;
        switch (selectedRecipe.recipeType)
        {
            case RecipeType.CraftingMaterial:
                CraftingFactory.CraftMaterial((CraftingMaterialRecipe)selectedRecipe);
                break;
            case RecipeType.Equipment:
                CraftingFactory.CraftEquipment((EquipmentRecipe)selectedRecipe);
                break;
            default:
                break;
        }
        PopulateMaterialInventory(craftingStation);
    }
    bool HasRequiredMaterials(BaseRecipe recipe)
    {
        var slots = recipeListHolder.GetComponentsInChildren<RecipeMaterialSlot>();
        foreach (var slot in slots)
        {
            if (!slot.isOccupied) return false;
        }
        return true;
    }
}
