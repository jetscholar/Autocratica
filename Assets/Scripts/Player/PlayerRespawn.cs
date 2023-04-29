
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint; // Last checkpoint stored here
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake() 
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        // Check if check point available
        if (currentCheckpoint == null) 
        {
            // Show game over screen
            uiManager.GameOver();
            return; // Do not execute the rest of this function
        }

        // move player to checkpoint position
        transform.position = currentCheckpoint.position;

        // Restore player health and reset animation
        playerHealth.Respawn();

        //Move camera back to checkpoint room
        // for this to work, the checkpoint object has to be placed as a child of the the room object
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; // Store the check point as current
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //Trigger the checkpoint anim
        }
    }
}
