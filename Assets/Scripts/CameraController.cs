using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    InputAction moveRightAction;
    InputAction moveLeftAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
        moveRightAction = InputSystem.actions.FindAction("LookRight");
        moveLeftAction = InputSystem.actions.FindAction("LookLeft");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool moveRightValue = moveRightAction.IsPressed();
        bool moveLeftValue = moveLeftAction.IsPressed();


        // TODO: Correct movement
        transform.position = player.transform.position + offset;
        if (moveRightValue)
        {
            transform.RotateAround(player.transform.position, Vector3.up, 20 * Time.deltaTime);
        }
        if (moveLeftValue)
        {
            transform.RotateAround(player.transform.position, Vector3.up, -20 * Time.deltaTime);
        }
    }

}
