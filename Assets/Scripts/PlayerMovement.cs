using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpPower;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private LayerMask wallLayer;
    // A rigid body variable
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;




    private void Awake()
    {
        // Get references to rigidbody and animator from game object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //Flipping the player
        horizontalInput = Input.GetAxis("Horizontal");
      
        //Flip mechanic
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
   
        // The jump mechanic
        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded()); 

        //Wall jump logic
        if(wallJumpCooldown > 0.2f)
        {
            
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); 

            // Sticks player to the wall
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 1;

            if (Input.GetKey(KeyCode.Space))
                Jump();
                
        }
        else
            wallJumpCooldown += Time.deltaTime;

        //DEBUG 
        //print(onWall());     
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*3, 6);
            wallJumpCooldown = 0;
            
        }

    }

    private bool isGrounded() 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall() 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
}