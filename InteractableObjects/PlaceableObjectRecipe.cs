using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectRecipe", menuName = "Scriptable Objects/PlaceableObjectRecipe")]
public class PlaceableObjectRecipe : BaseRecipe
{
    public CraftingMaterial[] requiredMaterials;
    public GameObject placeableObjectPrefab;
    public int resultQuantity = 1;

    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab)
    {
        throw new System.NotImplementedException();
    }
    public override void Craft()
    {
        foreach (var material in requiredMaterials)
        {
            if (MaterialInventory.Instance.GetMaterialAmount(material) < 1)
            {
                return;
            }
        }
        foreach (var material in requiredMaterials)
        {
            MaterialInventory.Instance.RemoveMaterial(material, 1);
        }
        // Logic to add the placeable object to the player's inventory would go here.
    }
}
