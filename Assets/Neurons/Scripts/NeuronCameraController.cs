using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronCameraController : MonoBehaviour
{
    // keyboard
    public Transform cameraTransform;
    public float moveSpeed = 1;   
   
    // scroll wheel zoom
    public float scrollSpeed = 10f; // Speed of zooming
    public float minY = 20; // Minimum y position (zoomed out)
    public float maxY = 100f; // Maximum y position (zoomed in)
    public float smoothTime = 0.3f; // Time to smooth the camera zoom movement                                   
    private float _currentY;
    private float _targetY;
    // pan drag
    public float panSpeed = 5;
    bool isDragging;
    Vector3 startDragPosition;
  
   
    void Start()
    {
        cameraTransform = Camera.main.transform;
        _currentY = cameraTransform.position.y;
        _targetY = _currentY; // Initialize targetY to current position
    }    
    void Update()
    {
        UpdateKBMovement();
        MouseDrag();        
        UpdateMouseZoom();
    }

    void UpdateMouseZoom()
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
    void MouseDrag()
    {

        if (Input.GetMouseButtonDown(2))
        {
            isDragging = true;
            GlobalNeuronEvents.SetOnCameraDrag();
            startDragPosition = Input.mousePosition;           
        }

        if (Input.GetMouseButtonUp(2))
        {
            GlobalNeuronEvents.SetOnCameraDragEnd();
            isDragging = false;
        }

        if (isDragging)
        {           
            var delta = startDragPosition - Input.mousePosition;
            
            // Get the distance from the camera to the ground 
            float distanceToGround = cameraTransform.position.y; 

            // Adjust delta based on distance
            delta *= panSpeed * distanceToGround * Time.deltaTime;

            var currentPos = cameraTransform.position;
            currentPos.x += delta.x;
            currentPos.z += delta.y;
            
            cameraTransform.position = currentPos;
            startDragPosition = Input.mousePosition; 
        }
       


    }
    void UpdateKBMovement()
    {
        //keyboard movement
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
   
    
}
