using UnityEngine;


[CreateAssetMenu(fileName = "New Stat Type", menuName = "Stats/Stat Type")]
public class StatType : ScriptableObject
{
    [Header("Basic Info")]
    public string displayName;
    [TextArea(2, 4)]
    public string description;
    public Sprite icon;
    
    [Header("Stat Properties")]
    public StatCategory category;
    public StatValueType valueType;
    public bool isPercentage;
    public float minValue = 0f;
    public float maxValue = 1000f;
    public float defaultValue = 0f;
    
    [Header("Display")]
    public string suffix = "";
    public int decimalPlaces = 0;
    public Color displayColor = Color.white;
    
    /// <summary>
    /// Unique identifier for this stat type
    /// </summary>
    public string StatID => name;
    
    /// <summary>
    /// Format the value for display
    /// </summary>
    public string FormatValue(float value)
    {
        string formatted = value.ToString($"F{decimalPlaces}");
        if (isPercentage)
            formatted += "%";
        else if (!string.IsNullOrEmpty(suffix))
            formatted += suffix;
        return formatted;
    }
}

public enum StatCategory
{
    Combat,
    Utility
}

public enum StatValueType
{
    Flat,      // Raw number (e.g., +10 damage)
    Multiplier, // Percentage multiplier (e.g., +20% damage)
    Additive   // Additive percentage (e.g., +20% crit chance)
}