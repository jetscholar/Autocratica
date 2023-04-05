using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // A rigid body variable
    private Rigidbody2D body;
    private Animator anim;

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
        if (Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }

        // Set animator properties
        anim.SetBool("run", horizontalInput != 0);        
    }
}