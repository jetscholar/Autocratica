using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        // check to see if enough time has passed between shots
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();    

        cooldownTimer += Time.deltaTime;
    }

    private void Attack() 
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
    }
}
