using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//place this script in the scene, not on a neuron object
public class DraggableUILine : MonoBehaviour
{ 
    public Vector3 lineOffset;
    public LineRenderer linePrefab;//premake to specs in editor and assign
    public RectTransform newLineConnectionIcon;
    internal Neuron selectedNueron;//the nueron we last clicked
    internal Neuron currentMouseOverNeuron;// current neuron the mouse is over
    internal Neuron highlightedNeuron;//the last neuron that the mouse was over
    
    Camera mainCamera;     
   
    void Start()
    {
        mainCamera = Camera.main;
        linePrefab.gameObject.SetActive(false);

        GlobalNeuronEvents.OnMouseOverNeuron += SetOnMouseOverNeuron;
        GlobalNeuronEvents.OnMouseUpNeuron += SetOnMouseUpNeuron;
        GlobalNeuronEvents.OnMouseDownNeuron += SetOnMouseDown;
        GlobalNeuronEvents.OnDragNeuron += SetOnDragNeuron;        
        GlobalNeuronEvents.OnMouseExitNeuron += SetMouseExitNeuron;
       
    }
    private void SetOnMouseOverNeuron(Neuron neuron)
    {       
        currentMouseOverNeuron = neuron;
    }
    private void SetOnMouseUpNeuron(Neuron neuron)
    {        
        //add a new connection if not our self
        if (!GameObject.Equals(currentMouseOverNeuron, selectedNueron))
        {
            //print("Mouse Up, Can connect since - NOT the same object");
            if (currentMouseOverNeuron != null)
            {
                AddConnection(selectedNueron, currentMouseOverNeuron);
            }
        }
        //turn off new line connection       
        HideLine(linePrefab);
    }
    private void SetOnMouseDown(Neuron neuron)
    {
        selectedNueron = neuron;
    }
    private void SetOnDragNeuron(Neuron neuron)
    {        
        selectedNueron = neuron;
        OnDragNewConnection();
    } 
    private void SetMouseExitNeuron(Neuron neuron)
    {
        currentMouseOverNeuron = null;
    }
    private void HideLine(LineRenderer line)
    {        
        linePrefab.gameObject.SetActive(false);
        newLineConnectionIcon.gameObject.SetActive(false);
    }
    private void ShowLine(LineRenderer line)
    {       
        linePrefab.gameObject.SetActive(true);
        newLineConnectionIcon.gameObject.SetActive(true);
    }
    private void AddConnection(Neuron from, Neuron to)
    {
        print("Adding new Connection! ");
        from.connections.Add(to);
    }
    private void OnDragNewConnection()
    {
        ShowLine(linePrefab);
        SetLinePositions();
    }

    private void SetLinePositions()
    {
        // Set start point        
        linePrefab.SetPosition(0, selectedNueron.transform.position);

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits a collider in the scene
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 target = hit.point + lineOffset;//raise up off ground

            //if we are not the same object we can snap line to target
            if (GameObject.Equals(selectedNueron, currentMouseOverNeuron))
            {
                //we are ourself
                HideLine(linePrefab);
            }
            else
            {
                if (currentMouseOverNeuron != null)
                {
                    target = currentMouseOverNeuron.transform.position;
                }
            }

            // Set end point 
            linePrefab.SetPosition(1, target);
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target);
            // Set icon
            newLineConnectionIcon.position = screenPos;
        }
        else
        {            
            // If no hit, you can set the line to a default dist or disable it
            linePrefab.SetPosition(1, ray.GetPoint(10)); // Extend the line to some arbitrary distance
        }
    }


}
