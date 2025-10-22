using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public ProjectileData projectileData;
    private float speed;
    private float damage;
    private float lifetime;
    public float size = 1;
    public int quantity = 1;
    void Start()
    {
        
    }
    public void Initialize(Vector3 direction, int _quantity)
    {
        transform.up = direction;
        speed = projectileData.speed;
        damage = projectileData.damage;
        lifetime = projectileData.lifetime;
        size = projectileData.size;
        quantity = _quantity;
        transform.localScale = Vector3.one * size;
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

}
