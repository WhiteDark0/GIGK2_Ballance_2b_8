using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 defaultOffset = new Vector3(0f, 10f, -10f);
    private Vector3 offset;

    InputAction moveLeftAction;
    InputAction moveRightAction;
    InputAction moveUpAction;

    public float rotationSpeed = 5f;

    private Vector3 targetOffset;
    private Quaternion targetRotation;

    // Parameters for when lifting the view point
    public float extraHeight = 5f;
    public float extraPitch = 15f;
    private bool isCameraUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = defaultOffset;
        moveLeftAction = InputSystem.actions.FindAction("LookLeft");
        moveRightAction = InputSystem.actions.FindAction("LookRight");
        moveUpAction = InputSystem.actions.FindAction("ToggleLookUp");

        targetOffset = offset;
        targetRotation = transform.rotation;

        if(player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool moveLeftValue = moveLeftAction.WasPressedThisFrame();
        bool moveRightValue = moveRightAction.WasPressedThisFrame();
        bool moveUpValue = moveUpAction.WasPressedThisFrame();

        if (moveLeftValue) RotateCamera(90f);
        else if (moveRightValue) RotateCamera(-90f);

        if (moveUpValue) ToggleCameraUp();
 
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

    private void ToggleCameraUp()
    {
        isCameraUp = !isCameraUp;

        float heightDelta = isCameraUp ? extraHeight : -extraHeight;
        float pitchDelta = isCameraUp ? extraPitch : -extraPitch;

        targetOffset.y += heightDelta;
        targetRotation = targetRotation * Quaternion.Euler(pitchDelta, 0f, 0f);
    }

}
