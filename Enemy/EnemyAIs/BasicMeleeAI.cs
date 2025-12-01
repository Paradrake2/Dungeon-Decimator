using System.Collections;
using UnityEngine;

public class BasicMeleeAI : EnemyAI
{
    //public Transform player;
    
    public override IEnumerator Attack()
    {
        // Implement melee attack logic here
        yield return null;
    }

    public override void Movement()
    {
        BaseMovement();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created


}
