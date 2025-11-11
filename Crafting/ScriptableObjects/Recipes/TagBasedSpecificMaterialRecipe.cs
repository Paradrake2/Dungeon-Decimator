using UnityEngine;

[System.Serializable]
public class MultiTagIngredient
{
    public CraftingMaterialTag[] materialTags;
    public CraftingMaterialTag materialTypeTag = null;
    public CraftingMaterial specificMaterial = null;
    public int quantity;
}




[CreateAssetMenu(fileName = "TagBasedSpecificMaterialRecipe", menuName = "Recipes/TagBasedSpecificMaterialRecipe")]
public class TagBasedSpecificMaterialRecipe : BaseRecipe
{
    public CraftingMaterial resultMaterial;
    public MultiTagIngredient[] ingredients;
    public int resultQuantity = 1;
    public override void Craft()
    {
        MaterialInventory.Instance.AddMaterial(resultMaterial, resultQuantity);
        CraftingManager.Instance.UpdateMaterialButton(resultMaterial);
    }

    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab, CraftingUI cUI)
    {
        foreach (var ingredient in ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(slotPrefab, parent);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                materialSlot.SetupMultiTagBaseSlot(ingredient.materialTags, cUI, ingredient.specificMaterial, ingredient.materialTypeTag);
            }
        }
    }
}
