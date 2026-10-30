using UnityEngine;

public class TransformationManager : MonoBehaviour
{
    public enum PlayerMaterialType
    {
        Paper,
        Planks,
        Stone
    }

    [SerializeField] private Material paperMaterial;
    private float paperMass = 0.5f;

    [SerializeField] private Material planksMaterial;
    private float planksMass = 1.0f;

    [SerializeField] private Material stoneMaterial;
    private float stoneMass = 2.0f;

    [SerializeField] private PlayerMaterialType defaultForm = PlayerMaterialType.Stone;

    private Renderer playerRenderer;
    private Rigidbody rb;
    private PlayerMaterialType currentForm;

    private void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        ApplyTransformation(defaultForm);
    }

    public void ApplyTransformation(PlayerMaterialType targetType)
    {
        Material targetMaterial = null;
        float targetMass = 1.0f;

        switch (targetType)
        {
            case PlayerMaterialType.Paper:
                targetMaterial = paperMaterial;
                targetMass = paperMass;
                break;

            case PlayerMaterialType.Planks:
                targetMaterial = planksMaterial;
                targetMass = planksMass;
                break;

            case PlayerMaterialType.Stone:
                targetMaterial = stoneMaterial;
                targetMass = stoneMass;
                break;
        }

        if (playerRenderer != null && targetMaterial != null)
        {
            // Use sharedMaterial to avoid generating duplicate runtime material instances
            playerRenderer.sharedMaterial = targetMaterial;
        }

        if (rb != null)
        {
            rb.mass = targetMass;
        }

        currentForm = targetType;
    }

    public void ResetToDefault()
    {
        ApplyTransformation(defaultForm);
    }

    public PlayerMaterialType CurrentForm => currentForm;
}