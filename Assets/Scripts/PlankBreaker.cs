using System.Collections;
using UnityEngine;

public class PlankBreaker : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float destroyDelay = 5f;

    private ObjectAudio objectAudio;
    private bool hasTriggered = false;
    private Coroutine breakCoroutine;

    private void Start()
    {
        objectAudio = GetComponent<ObjectAudio>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasTriggered && collision.gameObject.CompareTag(playerTag))
        {
            breakCoroutine = StartCoroutine(BreakSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            breakCoroutine = StartCoroutine(BreakSequence());
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

        gameObject.SetActive(false);
    }

    public void ResetPlank()
    {
        if(breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
        }
        hasTriggered = false;

        foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
        {
            r.enabled = true;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>(true))
        {
            c.enabled = true;
        }
    }
}