using Assets.Scripts;
using System.Xml;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    [SerializeField]
    private string _uniqueID;
    [SerializeField]
    private bool _saveTransform = false; // Set to true for obstacles and other moveable objects
    private bool _initialActiveState;

    public string UniqueID => _uniqueID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(_uniqueID))
        {
            // Generate a unique ID based on the object's name and position if none given
            _uniqueID = gameObject.name + "_" + transform.position.GetHashCode();
        }
    }

    private void Start()
    {
        if (CheckpointController.Instance == null) return;

        // Register the object with the CheckpointController
        CheckpointController.Instance.RegisterSaveable(this);

        _initialActiveState = gameObject.activeSelf;

        // Load the saved state if it exists
        if (CheckpointController.Instance.TryGetSavedState(_uniqueID, out ObjectState savedState))
        {
            gameObject.SetActive(savedState.IsActive);

            if (savedState.IsActive && _saveTransform)
            {
                transform.position = savedState.Position;
                transform.rotation = savedState.Rotation;
            }
        }
    }

    public void RestoreState(ObjectState state)
    {
        gameObject.SetActive(state.IsActive);
    }

    public void ResetToDefault()
    {
        gameObject.SetActive(_initialActiveState);
    }

    public ObjectState CaptureState()
    {
        return new ObjectState
        {
            IsActive = gameObject.activeSelf,
            Position = transform.position,
            Rotation = transform.rotation
        };
    }
}
