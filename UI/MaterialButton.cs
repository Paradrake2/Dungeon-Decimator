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
        icon.color = material.itemColor;
        amountText.text = amount.ToString();
        this.amount = amount;
        transform.SetParent(parent);

        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        CraftingManager.Instance.TryAddMaterialToRecipe(material);
    }
    public void UpdateAmount(int newAmount)
    {
        amount += newAmount;
        UpdateDisplay();
    }
    public void IncreaseAmount(int delta)
    {
        amount += delta;
        UpdateDisplay();
    }
    public void DecreaseAmount(int delta)
    {
        amount -= delta;
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        amountText.text = amount.ToString();
        gameObject.SetActive(amount > 0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
