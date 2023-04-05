using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // A rigid body variable
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    [SerializeField] private float speed = 5;

    private void Awake()
    {
        // Get references to rigidbody and animator from game object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);        
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}