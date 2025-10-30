using UnityEngine;

public class MaterialRecipeButton : MonoBehaviour
{
    public CraftingMaterialRecipe recipe;

    public void Initialize(CraftingMaterialRecipe recipe, GameObject parent)
    {
        this.recipe = recipe;
        transform.SetParent(parent.transform);
        // Initialize the button with the recipe details
    }
}

