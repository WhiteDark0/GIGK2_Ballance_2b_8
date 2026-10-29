using UnityEngine;

public class PlayerAudioMaterial : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip concreteRollClip;
    [SerializeField]
    private AudioClip metalRollClip;

    private float minSpeed = 0.2f;
    private float maxSpeed = 12.0f;

    private float minPitch = 0.8f;
    private float maxPitch = 1.2f;

    private Rigidbody rb;
    private bool isTouchingSurface;
    private AudioClip activeClip;
    private float targetVolume;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        audioSource.spatialBlend = 1.0f; // 3D spatial audio
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }
    private void FixedUpdate()
    {
        // Reset flag every physics update, OnCollisionStay will set it to true if the player is touching a surface
        isTouchingSurface = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts.Length == 0 || collision.contacts[0].normal.y < 0.5f) return; // Ignore if not touching a surface

        isTouchingSurface = true;
        DetectSurfaceMaterial(collision);
    }

    private void Update()
    {
        float rollSpeed = rb.angularVelocity.magnitude;

        if (isTouchingSurface && rollSpeed > minSpeed) 
        {
            float speedRatio = Mathf.InverseLerp(minSpeed, maxSpeed, rollSpeed);

            targetVolume = speedRatio; // Volume based on speed
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);

            if(!audioSource.isPlaying && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            targetVolume = 0f; // Fade out when not rolling
        }

        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, Time.deltaTime * 6f); // Smooth volume transition

        if(audioSource.volume <= 0f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void DetectSurfaceMaterial(Collision collision)
    {
        string materialTag = collision.gameObject.tag;
        AudioClip chosenClip = null;

        switch (materialTag)
        {
            case "Material_Concrete":
                chosenClip = concreteRollClip;
                break;
            case "Material_Metal":
                chosenClip = metalRollClip;
                break;
            default:
                chosenClip = null;
                break;
        }

        if (chosenClip != null && audioSource.clip != chosenClip)
        {
            float playbackTime = audioSource.time;
            audioSource.clip = chosenClip;
            audioSource.time = playbackTime % chosenClip.length; // Maintain playback position
            if(targetVolume > 0f && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }


}
