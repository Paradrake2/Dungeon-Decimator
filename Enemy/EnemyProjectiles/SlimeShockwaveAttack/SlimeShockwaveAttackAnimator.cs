using System.Collections;
using UnityEngine;

public class SlimeShockwaveAttackAnimator : MonoBehaviour
{
    [Header("Shockwave Settings")]
    public float maxRadius = 3f;
    public float expansionTime = 0.8f; // Total time to reach max radius
    public float holdTime = 0.2f; // Time to hold at max radius
    public float shrinkTime = 0.3f; // Time to shrink back down
    
    [Header("Visual Settings")]
    public AnimationCurve expansionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    
    public CircleCollider2D circleCollider2D;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        Debug.Log("Shockwave: Animation started");
        
        // Get components
        if (circleCollider2D == null)
            circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Initialize
        circleCollider2D.radius = 0f;
        if (spriteRenderer != null)
            transform.localScale = Vector3.zero;
            
        StartCoroutine(ShockwaveSequence());
    }

    IEnumerator ShockwaveSequence()
    {
        // Phase 1: Expansion
        yield return StartCoroutine(ExpandShockwave());
        
        // Phase 2: Hold at maximum
        yield return new WaitForSeconds(holdTime);
        
        // Phase 3: Shrink and fade
        yield return StartCoroutine(ShrinkShockwave());
        
        Debug.Log("Shockwave: Animation complete, destroying");
        Destroy(gameObject);
    }

    IEnumerator ExpandShockwave()
    {
        Debug.Log("Shockwave: Expanding");
        float timer = 0f;
        
        while (timer < expansionTime)
        {
            timer += Time.deltaTime;
            float progress = timer / expansionTime;
            float curveValue = expansionCurve.Evaluate(progress);
            float currentRadius = curveValue * maxRadius;
            
            // Update collider
            circleCollider2D.radius = currentRadius;
            
            // Update visual scale (if sprite exists)
            if (spriteRenderer != null)
            {
                float scale = (currentRadius / maxRadius) * 2f; // *2 because scale affects diameter
                transform.localScale = Vector3.one * scale;
            }
            
            yield return null;
        }
        
        // Ensure we reach maximum
        circleCollider2D.radius = maxRadius;
        if (spriteRenderer != null)
            transform.localScale = Vector3.one * 2f;
            
        Debug.Log($"Shockwave: Reached max radius {maxRadius}");
    }
    
    IEnumerator ShrinkShockwave()
    {
        Debug.Log("Shockwave: Shrinking");
        float timer = 0f;
        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        
        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;
            float progress = timer / shrinkTime;
            
            // Shrink radius
            float currentRadius = Mathf.Lerp(maxRadius, 0f, progress);
            circleCollider2D.radius = currentRadius;
            
            // Shrink visual and fade
            if (spriteRenderer != null)
            {
                float scale = Mathf.Lerp(2f, 0f, progress);
                transform.localScale = Vector3.one * scale;
                
                // Fade out
                float alpha = Mathf.Lerp(originalColor.a, 0f, progress);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            
            yield return null;
        }
    }
}
