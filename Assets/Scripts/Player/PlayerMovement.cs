using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Player Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header ("Coyote Time")]
    [SerializeField] private float coyoteTime; // hang time before jumping
    private float coyoteCounter; // Time since running of edge

    [Header ("Multiple Jumps")]
    [SerializeField] private int extraJumps; // number of jumps
    private float jumpCounter;

    [Header ("Wall Jumping")]
    [SerializeField] private float wallJumpX; // Horizontal wall jump force
    [SerializeField] private float wallJumpY; // Vertical wall jump force

    [Header ("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;
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

        // New Jump Method
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            // small hop
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            // prevent sliding off wall
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 1;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; // reset ability to do a coyote jump
                jumpCounter = extraJumps;
            }
            else
                coyoteCounter -= Time.deltaTime; // while > 0, another jump can be made
        }

}

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                // for a normal jump
                if (jumpCounter > 0) // if extra jumps
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
            }
            // reset coyote counter to avoid double jumps
            coyoteCounter = 0;
        }

    }

    private void WallJump() 
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
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

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
    
}