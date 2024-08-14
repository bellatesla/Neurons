using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronCameraController : MonoBehaviour
{
    public float moveSpeed = 1;
    public Transform cameraTransform;
    public float mouseSensitivity = 1;
    public float xRotation;

    public float scrollSpeed = 10f; // Speed of zooming
    public float minY = 10f; // Minimum y position (zoomed out)
    public float maxY = 100f; // Maximum y position (zoomed in)
    public float smoothTime = 0.3f; // Time to smooth the camera movement

    
    private float _currentY;
    private float _targetY;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;
        cameraTransform = Camera.main.transform;
        xRotation = cameraTransform.localEulerAngles.x;
        _currentY = cameraTransform.position.y;
        _targetY = _currentY; // Initialize targetY to current position
    }    
    void Update()
    {
        UpdateMovement();
        UpdateLookRotation();
        UpdateMouseScroll();

    }

    void UpdateMouseScroll()
    {
        // Get the scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // Calculate the target y position
        _targetY = Mathf.Clamp(_targetY - scrollInput * scrollSpeed, minY, maxY);

        // Smoothly interpolate the current y position to the target y position
        _currentY = Mathf.Lerp(_currentY, _targetY, Time.deltaTime / smoothTime);

        // Update the camera's position
        cameraTransform.position = new Vector3(cameraTransform.position.x, _currentY, cameraTransform.position.z);
    }
    void UpdateMovement()
    {
        Vector3 input = Vector3.zero;
        //arrows to camera world x,z
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        input.x = h;
        input.z = v;
        // Get direction from input relative to camera
        Vector3 movementDirection = cameraTransform.TransformDirection(input);
        movementDirection.y = 0;

        cameraTransform.Translate(moveSpeed * Time.deltaTime * movementDirection, Space.World);
    }
    void UpdateLookRotation()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust the vertical rotation (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation

        // Rotate the camera around the X-axis (up and down) and Y-axis (left and right)
        cameraTransform.localRotation = Quaternion.Euler(xRotation, cameraTransform.localRotation.eulerAngles.y + mouseX, 0f);
    }
    
}
