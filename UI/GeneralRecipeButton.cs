using UnityEngine;
using UnityEngine.UI;

public class GeneralRecipeButton : MonoBehaviour
{
    public BaseRecipe recipe;
    public Image icon;
    public CraftingUI cUI;
    public void Initialize(BaseRecipe newRecipe)
    {
        recipe = newRecipe;
        icon.sprite = recipe.icon;
        cUI = FindFirstObjectByType<CraftingUI>();
    }

    public void OnButtonClick()
    {
        cUI.OnRecipeButtonClick(recipe);
    }

}
