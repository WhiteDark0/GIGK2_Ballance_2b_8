using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource musicSource;

    public AudioClip DefaultClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        musicSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip, float volume = 0.1f)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return; // Avoid restarting the same music
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.spatialBlend = 0.0f; // Ensure the music is 2D
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();
    public void SetVolume(float volume) => musicSource.volume = volume;
}
