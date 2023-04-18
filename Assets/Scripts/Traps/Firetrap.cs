using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [Header ("Firetrap Parameters")]
    [SerializeField] private float damage;
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header ("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; // when trap gets triggered
    private bool active; // when the trap can hurt the player

    private Health playerHealth;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if (playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            if (!triggered)
            {
                // trigger the firetrap
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);             
        }

        if (!triggered)
        {
            // trigger the firetrap
            StartCoroutine(ActivateFiretrap());
        }
        if (active)
            collision.GetComponent<Health>().TakeDamage(damage);    
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.tag == "Player")
            playerHealth = null;
    }

    private IEnumerator ActivateFiretrap()
    {
        // turn sprite red to warn player and trigger trap
        triggered = true;
        spriteRend.color = Color.red; 

        // wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; // turn sprite back to its initial color
        active = true;
        anim.SetBool("activated", true);

        // wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}
