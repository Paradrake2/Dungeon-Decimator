using UnityEngine;

public class DungeonUIManager : MonoBehaviour
{
    public static DungeonUIManager Instance;
    public GameObject numberPopupPrefab;


    public void CreateNumberPopup(string text, Color color, Vector3 position, float duration = 1f)
    {
        GameObject popup = Instantiate(numberPopupPrefab, position, Quaternion.identity);
        NumberPopupEnv popupScript = popup.GetComponent<NumberPopupEnv>();
        popupScript.Setup(text, color, position, duration);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
