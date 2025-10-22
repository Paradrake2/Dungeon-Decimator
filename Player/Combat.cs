using UnityEngine;

public class Combat : MonoBehaviour
{
    Vector3 mousePos;
    public Transform player;
    void Start()
    {
        player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Set the z position to 0
        Vector3 direction = (mousePos - player.position).normalized;
        player.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
