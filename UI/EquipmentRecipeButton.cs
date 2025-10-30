using UnityEngine;

public class EquipmentRecipeButton : MonoBehaviour
{
    public EquipmentRecipe recipe;
    
    public void Initialize(EquipmentRecipe recipe, GameObject parent)
    {
        this.recipe = recipe;
        transform.SetParent(parent.transform);
    }

    public void OnClick()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
