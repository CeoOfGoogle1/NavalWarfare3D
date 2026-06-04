using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraNode;
    public float moveSpeed;
    public float sprintSpeed;
    public float zoomSpeed;
    public float sensetivity;
    float currentMoveSpeed;
    Vector3 movement;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) currentMoveSpeed = sprintSpeed;
        else currentMoveSpeed = moveSpeed;

        Vector3 forward = cameraNode.forward;
        Vector3 right = cameraNode.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement += forward;
        else if (Input.GetKey(KeyCode.S)) movement -= forward;
        if (Input.GetKey(KeyCode.A)) movement -= right;
        else if (Input.GetKey(KeyCode.D)) movement += right;

        transform.position += movement * currentMoveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            float mouseX = Input.GetAxis("Mouse X") * sensetivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensetivity;

            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, -mouseY, Space.Self);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        cameraNode.localPosition += Vector3.forward * scroll;
    }
}
