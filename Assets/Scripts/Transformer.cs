using UnityEngine;

public class Transformer : MonoBehaviour
{
    [SerializeField] private TransformationManager.PlayerMaterialType targetForm;
    public TransformationManager.PlayerMaterialType TargetForm => targetForm;
}
