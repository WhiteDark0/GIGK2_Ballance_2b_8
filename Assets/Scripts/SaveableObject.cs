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
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private Rigidbody _rb;
    private Rigidbody2D _rb2D;
    private PlankBreaker _plankBreaker;

    public string UniqueID => _uniqueID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(_uniqueID))
        {
            // Generate a unique ID based on the object's name and position if none given
            _uniqueID = gameObject.name + "_" + transform.position.GetHashCode();
        }

        // Cache physics components if present
        _rb = GetComponent<Rigidbody>();
        _rb2D = GetComponent<Rigidbody2D>();

        // Cache PlankBreaker component if present
        _plankBreaker = GetComponent<PlankBreaker>();

        // Cache initial transforms for default resets
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
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
            RestoreState(savedState);
        }
    }

    public void RestoreState(ObjectState state)
    {
        gameObject.SetActive(state.IsActive);

        if(_plankBreaker != null && state.IsActive)
        {
            _plankBreaker.ResetPlank();
        }

        if(_saveTransform && state.IsActive)
        {
            ApplyTransform(state.Position, state.Rotation);
        }
    }

    public void ResetToDefault()
    {
        gameObject.SetActive(_initialActiveState);

        if(_initialActiveState && _plankBreaker != null)
        {
            _plankBreaker.ResetPlank();
        }

        if (_saveTransform && _initialActiveState)
        {
            ApplyTransform(_initialPosition, _initialRotation);
        }
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

    private void ApplyTransform(Vector3 targetPosition, Quaternion targetRotation)
    {
        transform.position = targetPosition;
        transform.rotation = targetRotation;

        // Reset residual momentum so the object does not keep falling/flying
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        else if (_rb2D != null)
        {
            _rb2D.linearVelocity = Vector2.zero;
            _rb2D.angularVelocity = 0f;
        }
    }
}
