
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // This script's purpose is to damage the player when it touches her
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);
        
    }
}
