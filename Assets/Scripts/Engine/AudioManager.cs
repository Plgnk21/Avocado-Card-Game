using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource menuAudioSource;

    AudioClip[] soundsCache;

    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        soundsCache = Resources.LoadAll<AudioClip>("Sounds");
    }

    public void PlaySound(string sound, float volume = 1.0f)
    {
        for (int i = 0; i < soundsCache.Length; i++)
            if ((soundsCache[i]).name.Equals(sound))
                AudioSource.PlayClipAtPoint(soundsCache[i], Camera.main.transform.position, volume);
    }
}