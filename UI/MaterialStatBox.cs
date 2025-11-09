using UnityEngine;
using UnityEngine.UI;

public class MaterialStatBox : MonoBehaviour
{
    public Image icon;
    public TMPro.TextMeshProUGUI statText;
    public string stat;
    public float value;
    public Color displayColor;

    public void Initialize(string statType, float value, Sprite icon, Color color)
    {
        this.stat = statType;
        this.value = value;
        this.displayColor = color;
        if (icon != null)
        {
            this.icon.sprite = icon;
        }
        
        SetupStatText();
    }

    void SetupStatText()
    {
        statText.color = displayColor;
        statText.text = stat + " Value: " + value;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
