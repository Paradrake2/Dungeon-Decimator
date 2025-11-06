using UnityEngine;

[System.Serializable]
public enum RecipeType
{
    CraftingMaterial,
    Equipment
}


[CreateAssetMenu(fileName = "BaseRecipe", menuName = "Scriptable Objects/BaseRecipe")]
public abstract class BaseRecipe : ScriptableObject
{
    public Sprite icon;
    public int levelRequirement = 1;
    public RecipeType recipeType;
    public GameObject recipeUIElementPrefab = null;
    public bool isTagBased = false;
    public abstract void GenerateDynamicUI(Transform parent, GameObject slotPrefab);
    // public abstract void Craft
}
