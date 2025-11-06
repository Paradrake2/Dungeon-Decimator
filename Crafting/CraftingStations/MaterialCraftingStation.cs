using System.Collections.Generic;
using UnityEngine;

public class MaterialCraftingStation : CraftingStation
{
    public override List<CraftingMaterial> GetAvailableMaterials()
    {
        List<CraftingMaterial> availableMaterials = new List<CraftingMaterial>();
        foreach (var material in Resources.LoadAll<CraftingMaterial>("Items"))
        {
            availableMaterials.Add(material);
        }
        return availableMaterials;
    }

    public override List<BaseRecipe> GetAvailableRecipes()
    {
        List<BaseRecipe> availableRecipes = new List<BaseRecipe>();
        foreach (var recipe in Resources.LoadAll<CraftingMaterialRecipe>("Recipes/CraftingMaterials"))
        {
            if (recipe.levelRequirement <= stats.GetStatValue("Level"))
            {
                availableRecipes.Add(recipe);
            }
        }
        return availableRecipes;
    }
}
