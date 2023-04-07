using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private LayerMask wallLayer;
    // A rigid body variable
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;



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
        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Flip mechanic
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);

        
        // The jump mechanic
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());   

        //DEBUG 
        print(onWall());     
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

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