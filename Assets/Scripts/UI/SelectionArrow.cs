using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; // when moving arrow
    [SerializeField] private AudioClip interactSound; // when option selected
    private RectTransform rect;
    // Which option is currently selected
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        
    }

    private void Update()
    {
        // Change position of the selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        // Interact with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        //Assign the Y position of the current option to the arrow (move it up or down)
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);

    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        // Access the button component on each option and call it's function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
