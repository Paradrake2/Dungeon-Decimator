using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmeltingStation : CraftingStation
{
    public override List<CraftingMaterial> GetAvailableMaterials()
    {
        List<CraftingMaterial> availableMaterials = new List<CraftingMaterial>();
        foreach (var material in Resources.LoadAll<CraftingMaterial>("Items"))
        {
            if (material.tags.Any(tag => tag.name == "ore")) availableMaterials.Add(material);
        }
        return availableMaterials;
    }

    public override List<BaseRecipe> GetAvailableRecipes()
    {
        List<BaseRecipe> availableRecipes = new List<BaseRecipe>();
        foreach (var recipe in Resources.LoadAll<BaseRecipe>("Recipes/CraftingMaterials"))
        {
            if (recipe.levelRequirement <= stats.GetStatValue("Level") && recipe.recipeCategory.Any(category => category.categoryName == "oreSmelting"))
            {
                availableRecipes.Add(recipe);
            }
        }
        return availableRecipes;
    }
}
