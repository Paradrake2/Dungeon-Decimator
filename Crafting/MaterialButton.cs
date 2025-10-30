using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MaterialButton : MonoBehaviour
{
    public CraftingMaterial material;
    public int amount;
    public Image icon;
    public TextMeshProUGUI amountText;
    public void Initialize(CraftingMaterial material, int amount, Transform parent)
    {
        this.material = material;
        icon.sprite = material.icon;
        amountText.text = amount.ToString();
        this.amount = amount;
        transform.SetParent(parent);
        // Initialize the button with the material details
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
