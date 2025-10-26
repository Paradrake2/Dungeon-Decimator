using System.Collections;
using UnityEngine;

public class SlimeShockwaveAttackAnimator : MonoBehaviour
{
    public float maxRadius = 1f;
    public float expansionSpeed = 5f;
    public CircleCollider2D circleCollider2D;
    void Start()
    {
        StartCoroutine(ExpandShockwave());
    }

    IEnumerator ExpandShockwave()
    {
        float currentRadius = 0f;
        while (currentRadius < maxRadius)
        {
            currentRadius += expansionSpeed * Time.deltaTime;
            circleCollider2D.radius = currentRadius;
            yield return null;
        }
        Destroy(gameObject);
    }
}
