using System.Collections.Generic;
using UnityEngine;

public static class RecipeHolder
{
    public static EquipmentRecipe[] EquipmentRecipes;
    public static CraftingMaterialRecipe[] CraftingMaterialRecipes;

    public static void LoadRecipes()
    {
        // Load equipment recipes
        EquipmentRecipes = Resources.LoadAll<EquipmentRecipe>("Recipes/Equipment");

        // Load crafting material recipes
        CraftingMaterialRecipes = Resources.LoadAll<CraftingMaterialRecipe>("Recipes/CraftingMaterials");
    }

    public static EquipmentRecipe GetEquipmentRecipeByName(string recipeName)
    {
        foreach (var recipe in EquipmentRecipes)
        {
            if (recipe.name == recipeName)
            {
                return recipe;
            }
        }
        return null;
    }
    public static CraftingMaterialRecipe GetCraftingMaterialRecipeByName(string recipeName)
    {
        foreach (var recipe in CraftingMaterialRecipes)
        {
            if (recipe.name == recipeName)
            {
                return recipe;
            }
        }
        return null;
    }
}
