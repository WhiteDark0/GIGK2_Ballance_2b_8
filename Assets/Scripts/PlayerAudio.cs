using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip FallDownClip;
    [SerializeField] private AudioClip ExtraPointsClip;
    [SerializeField] private AudioClip ExtraLifeClip;
    [SerializeField] private AudioClip TransformClip;
    [SerializeField] private AudioClip BreakClip;
    [SerializeField] private AudioClip CheckpointClip;
    [SerializeField] private AudioClip EndClip;


    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    public void PlayFallDownSound() => audioSource.PlayOneShot(FallDownClip);
    public void PlayExtraPointsSound() => audioSource.PlayOneShot(ExtraPointsClip);
    public void PlayExtraLifeSound() => audioSource.PlayOneShot(ExtraLifeClip);
    public void PlayTransformSound() => audioSource.PlayOneShot(TransformClip);
    public void PlayBreakSound() => audioSource.PlayOneShot(BreakClip);
    public void PlayCheckpointSound() => audioSource.PlayOneShot(CheckpointClip);
    public void PlayEndSound() => audioSource.PlayOneShot(EndClip);

}