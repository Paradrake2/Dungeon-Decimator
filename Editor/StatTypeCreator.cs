#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor utility to quickly create common stat types
/// </summary>
public class StatTypeCreator : EditorWindow
{
    private string statName = "";
    private string displayName = "";
    private string description = "";
    private StatCategory category = StatCategory.Combat;
    private StatValueType valueType = StatValueType.Flat;
    private bool isPercentage = false;
    private float minValue = 0f;
    private float maxValue = 10000f;
    private float defaultValue = 0f;
    private string suffix = "";
    private int decimalPlaces = 0;
    
    [MenuItem("Tools/Stats/Create Stat Type")]
    public static void ShowWindow()
    {
        GetWindow<StatTypeCreator>("Stat Type Creator");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Create New Stat Type", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        statName = EditorGUILayout.TextField("Stat Name (File Name)", statName);
        displayName = EditorGUILayout.TextField("Display Name", displayName);
        description = EditorGUILayout.TextField("Description", description, GUILayout.Height(40));
        
        EditorGUILayout.Space();
        category = (StatCategory)EditorGUILayout.EnumPopup("Category", category);
        valueType = (StatValueType)EditorGUILayout.EnumPopup("Value Type", valueType);
        isPercentage = EditorGUILayout.Toggle("Is Percentage", isPercentage);
        
        EditorGUILayout.Space();
        minValue = EditorGUILayout.FloatField("Min Value", minValue);
        maxValue = EditorGUILayout.FloatField("Max Value", maxValue);
        defaultValue = EditorGUILayout.FloatField("Default Value", defaultValue);
        
        EditorGUILayout.Space();
        suffix = EditorGUILayout.TextField("Suffix", suffix);
        decimalPlaces = EditorGUILayout.IntField("Decimal Places", decimalPlaces);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Create Stat Type"))
        {
            CreateStatType();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Create Common Combat Stats"))
        {
            CreateCommonStats();
        }
    }
    
    private void CreateStatType()
    {
        if (string.IsNullOrEmpty(statName))
        {
            EditorUtility.DisplayDialog("Error", "Please enter a stat name", "OK");
            return;
        }
        
        StatType newStat = CreateInstance<StatType>();
        newStat.displayName = string.IsNullOrEmpty(displayName) ? statName : displayName;
        newStat.description = description;
        newStat.category = category;
        newStat.valueType = valueType;
        newStat.isPercentage = isPercentage;
        newStat.minValue = minValue;
        newStat.maxValue = maxValue;
        newStat.defaultValue = defaultValue;
        newStat.suffix = suffix;
        newStat.decimalPlaces = decimalPlaces;
        
        string path = $"Assets/ScriptableObjects/Stats/{statName}.asset";
        System.IO.Directory.CreateDirectory("Assets/ScriptableObjects/Stats");
        AssetDatabase.CreateAsset(newStat, path);
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newStat;
        
        Debug.Log($"Created stat type: {statName} at {path}");
    }
    
    private void CreateCommonStats()
    {
        string[] combatStats = {
            "Health|Health|Maximum health points|Combat|Flat|false|0|10000|100||0",
            "Damage|Damage|Base damage dealt|Combat|Flat|false|0|1000|10||0",
            "CriticalChance|Critical Chance|Chance to deal critical damage|Combat|Additive|true|0|100|5|%|1",
            "CriticalDamage|Critical Damage|Multiplier for critical hits|Combat|Multiplier|true|100|500|150|%|0",
            "AttackSpeed|Attack Speed|Speed of attacks|Combat|Multiplier|true|50|300|100|%|0",
            "Defense|Defense|Damage reduction|Defense|Flat|false|0|1000|5||0",
            "MovementSpeed|Movement Speed|Character movement speed|Movement|Multiplier|true|50|200|100|%|0",
            "LifeSteal|Life Steal|Health recovered per damage dealt|Utility|Additive|true|0|50|0|%|1"
        };
        
        System.IO.Directory.CreateDirectory("Assets/ScriptableObjects/Stats");
        
        foreach (string statData in combatStats)
        {
            string[] parts = statData.Split('|');
            if (parts.Length == 11)
            {
                StatType newStat = CreateInstance<StatType>();
                newStat.displayName = parts[1];
                newStat.description = parts[2];
                newStat.category = (StatCategory)System.Enum.Parse(typeof(StatCategory), parts[3]);
                newStat.valueType = (StatValueType)System.Enum.Parse(typeof(StatValueType), parts[4]);
                newStat.isPercentage = bool.Parse(parts[5]);
                newStat.minValue = float.Parse(parts[6]);
                newStat.maxValue = float.Parse(parts[7]);
                newStat.defaultValue = float.Parse(parts[8]);
                newStat.suffix = parts[9];
                newStat.decimalPlaces = int.Parse(parts[10]);
                
                string path = $"Assets/ScriptableObjects/Stats/{parts[0]}.asset";
                AssetDatabase.CreateAsset(newStat, path);
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Created common combat stats!");
    }
}
#endif