using UnityEngine;

[CreateAssetMenu(fileName = "RecipeCategory", menuName = "Tags/RecipeCategory")]
public class RecipeCategory : ScriptableObject
{
    public string categoryName;
    public Sprite categoryIcon;
}
