using UnityEngine;
using UnityEngine.UI;

public class MaterialStatViewer : MonoBehaviour
{
    public CraftingMaterial material;
    public Image icon;
    public Transform statHolder;
    public GameObject statBoxPrefab;
    public MaterialAttributes[] att;
    StatCollection stats;

    public void Initialize(CraftingMaterial material)
    {
        this.material = material;
        this.icon.sprite = material.icon;
        this.stats = material.stats.stats;
        this.att = material.materialStats.attribute;
        SetupStats();
    }

    void SetupStats()
    {
        foreach (Transform child in statHolder)
        {
            Destroy(child.gameObject);
        }
        foreach (var stat in stats.Stats)
        {
            GameObject statBoxObj = Instantiate(statBoxPrefab, statHolder);
            MaterialStatBox statBox = statBoxObj.GetComponent<MaterialStatBox>();
            statBox.Initialize(stat.GetStatID(), stat.Value, stat.GetIcon());
        }
        // set material attributes
    }

    void Start()
    {
        Initialize(material);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
