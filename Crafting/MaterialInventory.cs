using System.Collections.Generic;
using UnityEngine;

public class MaterialAmount
{
    public CraftingMaterial material;
    public int amount;
}

public class MaterialInventory : MonoBehaviour
{
    public static MaterialInventory Instance;
    public List<MaterialAmount> materials = new List<MaterialAmount>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMaterial(CraftingMaterial material, int amount)
    {
        MaterialAmount matAmount = materials.Find(m => m.material == material);
        if (matAmount != null)
        {
            matAmount.amount += amount;
        }
        else
        {
            materials.Add(new MaterialAmount { material = material, amount = amount });
        }
    }
    public void RemoveMaterial(CraftingMaterial material, int amount)
    {
        MaterialAmount matAmount = materials.Find(m => m.material == material);
        if (matAmount != null)
        {
            matAmount.amount -= amount;
            if (matAmount.amount <= 0)
            {
                materials.Remove(matAmount);
            }
        }
    }
    public int GetMaterialAmount(CraftingMaterial material)
    {
        MaterialAmount matAmount = materials.Find(m => m.material == material);
        return matAmount != null ? matAmount.amount : 0;
    }
}