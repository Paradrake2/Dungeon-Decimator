using UnityEngine;

public class NumberPopupEnv : MonoBehaviour
{
    public void Setup(string text, Color color, Vector3 position, float duration = 1f)
    {
        var popupText = GetComponent<TMPro.TextMeshPro>();
        popupText.text = text;
        popupText.color = color;
        transform.position = position;
        Destroy(gameObject, duration);
    }
}
