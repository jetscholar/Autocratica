using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton Pattern
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake() 
    {
        instance = this;
        source = GetComponent<AudioSource>();

        // Keep this object even when we go to a new screen
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy any duplicates
        else if (instance != null && instance != this)
            Destroy(gameObject);
        
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}
