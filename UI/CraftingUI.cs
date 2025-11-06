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
    //[SerializeField] private BaseRecipe selectedRecipe;
    public CraftingStation craftingStation;
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
        var availableMaterials = station.GetAvailableMaterials();
        foreach (var material in availableMaterials)
        {
            var materialStack = inventory.materials.Find(m => m.material == material);
            if (materialStack != null && materialStack.amount > 0)
            {
                var button = Instantiate(materialButtonPrefab, materialInventoryHolder);
                button.GetComponent<MaterialButton>().Initialize(material, materialStack.amount, button.transform.parent);
            }
        }
    }

    public void SetupCraftingArea(CraftingStation station)
    {
        foreach (Transform child in recipeListHolder)
        {
            Destroy(child.gameObject);
        }

        var availableRecipes = station.GetAvailableRecipes();

        foreach (var recipe in availableRecipes)
        {
            if (!IsUnlocked(recipe)) continue;
            var button = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
            button.GetComponent<Button>().onClick.AddListener(() => OnRecipeButtonClick(recipe));
            button.GetComponent<Image>().sprite = recipe.icon;
        }
    }

    
    public void OnRecipeButtonClick(BaseRecipe recipe)
    {
        CraftingManager.Instance.SetRecipe(recipe, this);
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
            recipeListHolder.GetComponent<GridLayoutGroup>().enabled = false;
            Instantiate(recipe.recipeUIElementPrefab, recipeListHolder);
        }
        else
        {
            recipe.GenerateDynamicUI(recipeListHolder, recipeMaterialButtonPrefab);
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
        CraftingManager.Instance.ClearRecipe();
        craftingStation = null;
    }
    
    

    
    /**
    void SetupTagBasedSlots(CraftingStation station)
    {
        foreach (var recipe in station.GetAvailableRecipes())
        {
            if (!IsUnlocked(recipe)) continue;
            var button = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
            button.GetComponent<Button>().onClick.AddListener(() => OnRecipeButtonClick(recipe));
            button.GetComponent<Image>().sprite = recipe.icon;
        }
    }
    
    void SetupSpecificMaterialSlots(CraftingStation station)
    {
        foreach (var recipe in station.GetAvailableRecipes())
        {
            if (!IsUnlocked(recipe)) continue;
            var button = Instantiate(recipeMaterialButtonPrefab, recipeListHolder);
            button.GetComponent<Button>().onClick.AddListener(() => OnRecipeButtonClick(recipe));
            button.GetComponent<Image>().sprite = recipe.icon;
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
    **/
}
