using UnityEngine;

public static class CraftingFactory
{
    public static void CraftMaterial(CraftingMaterialRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!(MaterialInventory.Instance.GetMaterialAmount(ingredient.material) < ingredient.quantity))
            {
                return;
            }
        }
        foreach (var ingredient in recipe.ingredients)
        {
            MaterialInventory.Instance.RemoveMaterial(ingredient.material, ingredient.quantity);
        }
        for (int i = 0; i < recipe.resultQuantity; i++)
        {
            MaterialInventory.Instance.AddMaterial(recipe.resultMaterial, 1);
        }
    }

    public static void CraftEquipment(EquipmentRecipe recipe)
    {
        
    }


}

