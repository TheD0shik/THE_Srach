using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("Settings")]
    public float sensitivityX = 20f;
    public float sensitivityY = 20f;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    [Header("References")]
    public Transform playerBody;

    private float rotationX;
    private float rotationY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        // Горизонтальное вращение игрока
        playerBody.Rotate(Vector3.up * mouseX);

        // Вертикальное вращение камеры
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
    }
}