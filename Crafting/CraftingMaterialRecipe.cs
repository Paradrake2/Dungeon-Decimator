using UnityEngine;

[System.Serializable]
public class CraftingMaterialIngredient
{
    public CraftingMaterial material;
    public int quantity;
}


[CreateAssetMenu(fileName = "CraftingMaterialRecipe", menuName = "Scriptable Objects/CraftingMaterialRecipe")]
public class CraftingMaterialRecipe : BaseRecipe
{
    public CraftingMaterialIngredient[] ingredients;
    public CraftingMaterial resultMaterial;
    public int resultQuantity = 1;
}
