using UnityEngine;

[System.Serializable]
public class EquipmentIngredient
{
    public CraftingMaterialTag materialTag;
    public int quantity;
}


[CreateAssetMenu(fileName = "EquipmentRecipe", menuName = "Scriptable Objects/EquipmentRecipe")]
public class EquipmentRecipe : ScriptableObject
{
    public EquipmentType equipmentType;
    public EquipmentIngredient[] ingredients;
    public float tier;
    public int levelRequirement = 1;

}
