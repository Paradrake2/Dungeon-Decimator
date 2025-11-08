using UnityEngine;

[System.Serializable]
public class EquipmentIngredient
{
    public CraftingMaterialTag materialTag;
    public CraftingMaterial material;
    public int quantity;
}


[CreateAssetMenu(fileName = "EquipmentRecipe", menuName = "Scriptable Objects/EquipmentRecipe")]
public class EquipmentRecipe : BaseRecipe
{
    public EquipmentType equipmentType;
    public EquipmentIngredient[] ingredients;
    public float tier;

    public override void GenerateDynamicUI(Transform parent, GameObject slotPrefab)
    {
        
    }
    public override void Craft()
    {
        
    }
}
