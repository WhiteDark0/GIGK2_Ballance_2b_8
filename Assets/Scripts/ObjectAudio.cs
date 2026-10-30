using UnityEngine;

public class ObjectAudio : MonoBehaviour
{
    [SerializeField] private AudioClip CreakClip;
    [SerializeField] private AudioClip BreakClip;


    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    public void PlayCreakSound() => audioSource.PlayOneShot(CreakClip);
    public void PlayBreakSound() => AudioSource.PlayClipAtPoint(BreakClip, transform.position);

}