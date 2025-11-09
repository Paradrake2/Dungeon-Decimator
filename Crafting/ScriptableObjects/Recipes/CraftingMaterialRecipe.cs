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
    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab, CraftingUI cUI)
    {
        foreach (var ingredient in ingredients)
        {
            for (int i = 0; i < ingredient.quantity; i++)
            {
                GameObject slot = Instantiate(slotPrefab, parent);
                var materialSlot = slot.GetComponent<RecipeMaterialSlot>();
                materialSlot.SetupSpecificMaterialSlot(ingredient.material, cUI);

            }
        }
    }
    public override void Craft()
    {

        MaterialInventory.Instance.AddMaterial(resultMaterial, resultQuantity);
        CraftingManager.Instance.UpdateMaterialButton(resultMaterial);
    }
}
