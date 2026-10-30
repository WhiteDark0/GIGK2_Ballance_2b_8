using System.Collections;
using UnityEngine;

public class PlankBreaker : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float destroyDelay = 5f;

    private ObjectAudio objectAudio;
    private bool hasTriggered = false;

    private void Start()
    {
        objectAudio = GetComponent<ObjectAudio>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasTriggered && collision.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(BreakSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            StartCoroutine(BreakSequence());
        }
    }

    private IEnumerator BreakSequence()
    {
        hasTriggered = true;

        if (objectAudio != null)
        {
            objectAudio.PlayCreakSound();
        }

        yield return new WaitForSeconds(destroyDelay);

        if (objectAudio != null)
        {
            objectAudio.PlayBreakSound();
        }

        HideAndDisablePlank();

        Destroy(gameObject, 1.5f);
    }

    private void HideAndDisablePlank()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }
}