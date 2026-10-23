using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    InputAction moveLeftAction;
    InputAction moveRightAction;

    public float rotationSpeed = 5f;

    private Vector3 targetOffset;
    private Quaternion targetRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
        moveLeftAction = InputSystem.actions.FindAction("LookLeft");
        moveRightAction = InputSystem.actions.FindAction("LookRight");

        targetOffset = offset;
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool moveLeftValue = moveLeftAction.WasPressedThisFrame();
        bool moveRightValue = moveRightAction.WasPressedThisFrame();

        if (moveLeftValue) RotateCamera(90f);
        else if (moveRightValue) RotateCamera(-90f);

        // Interpolate towards the target offset and rotation
        offset = Vector3.Lerp(offset, targetOffset, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.position = player.transform.position + offset;
    }

    private void RotateCamera(float angle)
    {
        // Calculate the new offset and rotation based on the angle
        Quaternion turnRotation = Quaternion.Euler(0f, angle, 0f);
        targetOffset = turnRotation * targetOffset;
        targetRotation = turnRotation * targetRotation;
    }

}
