using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource backgroundAudioSource;

    public AudioClip backgroundMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            backgroundAudioSource = gameObject.AddComponent<AudioSource>();
            backgroundAudioSource.clip = backgroundMusic;
            backgroundAudioSource.loop = true;
            backgroundAudioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SetVolume(float volume)
    {
        if (instance != null)
        {
            if (instance.backgroundAudioSource != null)
            {
                instance.backgroundAudioSource.volume = volume;
            }
            foreach (var audioSource in FindObjectsOfType<AudioSource>())
            {
                audioSource.volume = volume;
            }
        }
    }
}

