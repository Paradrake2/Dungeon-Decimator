using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public GameObject previewWindow;
    public Sprite defaultPreviewSprite;
    public MaterialInventory inventory;
    public CraftingStationType craftingStationType;
    public PlayerStats stats;
    public CraftingUI Instance;
    //[SerializeField] private BaseRecipe selectedRecipe;
    public CraftingStation craftingStation;
    public GameObject materialInspector;
    public Dictionary<CraftingMaterial, int> tempMaterialInventory = new Dictionary<CraftingMaterial, int>();
    public GameObject materialInspectorButton;
    public GameObject craftButton;
    public bool inspectMaterial = false;
    [SerializeField] private List<CraftingMaterial> craftingMaterials;
    public TextMeshProUGUI statText;

    public void Initialize(CraftingStation station)
    {
        NullChecks();
        Setup();
        craftingStation = station;
        craftingStationType = station.stationType;
        SetupCraftingArea(station);
        PopulateMaterialInventory(station);
        CraftingManager.Instance.SetUI(this);
        tempMaterialInventory.Clear();

        Instance = this;
    }
    void Setup()
    {
        craftButton.GetComponent<Button>().onClick.AddListener(() => CraftingManager.Instance.CraftSelectedRecipe());
    }

    public void ResetWindow()
    {
        foreach (Transform child in recipeListHolder)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in materialInventoryHolder)
        {
            Destroy(child.gameObject);
        }
        CraftingManager.Instance.AddExtraTempMaterials();
        CraftingManager.Instance.ClearTempMaterials();
        recipeListHolder.GetComponent<GridLayoutGroup>().enabled = true;

        SetPreviewSprite();
        SetupCraftingArea(craftingStation);
        PopulateMaterialInventory(craftingStation);
        statText.text = "";
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
                button.GetComponent<MaterialButton>().Initialize(material, materialStack.amount, button.transform.parent, this);
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
            if (recipe.useDisplayColor) button.GetComponent<Image>().color = recipe.displayColor;
            button.GetComponent<RecipeMaterialSlot>().cUI = this;
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
            recipe.GenerateDynamicUI(recipeListHolder, recipeMaterialButtonPrefab, this);
        }
        if (recipe.icon != null) previewWindow.GetComponent<Image>().sprite = recipe.icon;
        UpdateMaterialButtonShaders();
    }

    void UpdateMaterialButtonShaders()
    {
        var materialButtons = materialInventoryHolder.GetComponentsInChildren<MaterialButton>();
        foreach (var button in materialButtons)
        {
            for (int i = 0; i < CraftingManager.Instance.currentProgress.slots.Count; i++)
            {
                var slot = CraftingManager.Instance.currentProgress.slots[i];
                bool canUse = CraftingManager.Instance.currentProgress.CanMaterialFitSlot(button.material, slot);
                if (canUse)
                {
                    button.DeactivateShader();
                    break;
                }
                else
                {
                    button.ActivateShader();
                }
            }
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
        CraftingManager.Instance.ClearUI();
    }

    public void SetPreviewSprite()
    {
        previewWindow.GetComponent<Image>().sprite = defaultPreviewSprite;
    }

    public void BringUpMaterialInspector(CraftingMaterial material)
    {
        GameObject inspector = Instantiate(materialInspector, transform);
        inspector.GetComponent<MaterialStatViewer>().Initialize(material);
    }
    public void ToggleInspectMaterial()
    {
        inspectMaterial = !inspectMaterial;
        materialInspectorButton.GetComponent<Image>().color = inspectMaterial ? Color.green : Color.white;
    }
    public void UpdateStats()
    {
        if (!CraftingManager.Instance.currentProgress.IsComplete()) return;
        var stats = CraftingManager.Instance.currentProgress.GetPreviewStats();
        foreach (var stat in stats)
        {
            statText.text += $"{stat.StatType}: {stat.Value}\n";
        }
    }
}
