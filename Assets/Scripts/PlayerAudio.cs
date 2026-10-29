using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioClip FallDownClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        FallDownClip = Resources.Load<AudioClip>("Sounds/FallDown");
        audioSource.spatialBlend = 1.0f;
    }

    public void PlayFallDownSound() => audioSource.PlayOneShot(FallDownClip);
}