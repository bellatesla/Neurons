using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//place this script in the scene not on a neuron object
public class NeuronUI : MonoBehaviour
{     
    public static NeuronUI instance;//singleton

    public Button hoverFireButtonPositive;
    public Button hoverFireButtonNegative;
    public Button hoverInvertButton;
    public Button hoverRemoveButton;
    public RectTransform buttonsPanel;
    
    public Vector3 lineOffset;
    public LineRenderer newConnectionLine;
    public RectTransform newLineConnectionIcon;
    internal Neuron selectedNueron;//the nueron we last clicked
    internal Neuron currentMouseOverNeuron;// current neuron the mouse is over
    internal Neuron highlightedNeuron;//the last neuron that the mouse was over
    
    Camera mainCamera; 
    public bool isDragging; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    void Start()
    {
        mainCamera = Camera.main;

        hoverFireButtonPositive.onClick.AddListener(OnClickedPositiveFireButton);
        hoverFireButtonNegative.onClick.AddListener(OnClickedNegativeFireButton);
        hoverInvertButton.onClick.AddListener(OnClickedInvertButton);
        hoverRemoveButton.onClick.AddListener(OnClickedRemoveButton);       
    }

    internal void SetOnDragEndNeuron(Neuron neuron)
    {
        isDragging = false;
    }

    void Update()
    { 
        if (highlightedNeuron != null)
        {
            UpdateMouseOverUI();
        }
    }
    public void SetSelectedNeuron(Neuron neuron)
    {
        selectedNueron = neuron;
    }   
    void UpdateMouseOverUI()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(highlightedNeuron.transform.position);
        screenPos.z = 0;
        //screenPos.x += hudOffset.x;
        //screenPos.y += hudOffset.y;
        buttonsPanel.parent.position = screenPos;
    }    
    void HideLine(LineRenderer line)
    {
        buttonsPanel.gameObject.SetActive(true);
        newConnectionLine.gameObject.SetActive(false);
        newLineConnectionIcon.gameObject.SetActive(false);
    }
    void ShowLine(LineRenderer line)
    {
        buttonsPanel.gameObject.SetActive(false);
        newConnectionLine.gameObject.SetActive(true);
        newLineConnectionIcon.gameObject.SetActive(true);
    }
    void RemoveConnection(Neuron from, Neuron to)
    {
        from.connections.Remove(to);
    }
    void AddConnection(Neuron from, Neuron to)
    {
        print("Adding new Connection! ");
        from.connections.Add(to);
    }   
    void FireNeuron(Neuron neuron, float voltage)
    {
        if (neuron == null) return;
        neuron.ForceFire(voltage);
    }
    void OnClickedRemoveButton()
    {
        int count = highlightedNeuron.connections.Count;
        if (count > 0)
        {
            //remove the last one n the list
            RemoveConnection(highlightedNeuron, highlightedNeuron.connections[count - 1]);
        }
    }
    void OnClickedInvertButton()
    {       
        if (highlightedNeuron)
        {
            highlightedNeuron.Invert();
        }
    }
    void OnClickedPositiveFireButton()
    {
        FireNeuron(highlightedNeuron,1);
    }
    void OnClickedNegativeFireButton()
    {
        FireNeuron(highlightedNeuron,-1);
    }
    void OnDragNewConnection()
    {        
        ShowLine(newConnectionLine);

        // Start point        
        newConnectionLine.SetPosition(0, selectedNueron.transform.position);

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
                HideLine(newConnectionLine);
            }
            else
            {
                if (currentMouseOverNeuron != null)
                {
                    target = currentMouseOverNeuron.transform.position;
                }
            }

            // End point 
            newConnectionLine.SetPosition(1, target);
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target);
            // Icon
            newLineConnectionIcon.position = screenPos;
        }
        else
        {
            //when no objects under mouse 
            // If no hit, you can set the line to some default position or disable it
            newConnectionLine.SetPosition(1, ray.GetPoint(10)); // Extend the line to some arbitrary distance
        }
    }
    internal void SetOnMouseUpNeuron(Neuron neuron)
    {        
        //adds a new connection if not our self
        if (!GameObject.Equals(currentMouseOverNeuron, selectedNueron))
        {
            //print("Mouse Up - NOT the same object! ");
            if (currentMouseOverNeuron != null)
            {
                AddConnection(selectedNueron, currentMouseOverNeuron);
            }
        }
        //turn off new line connection       
        HideLine(newConnectionLine);
    }
    internal void SetOnDragNeuron(Neuron neuron)
    {
        isDragging = true;
        selectedNueron = neuron;
        OnDragNewConnection();
        
    }
    internal void SetMouseExitNeuron(Neuron neuron)
    {      
        currentMouseOverNeuron = null;        
    }
    internal void SetOnMouseOver(Neuron neuron)
    {
        //show HUD
        currentMouseOverNeuron = neuron;
        
        // Keeps panel focused if the mouse is over it
        if (buttonsPanel.parent.GetComponent<HoverableUI>().isMouseOver)
        {
            UpdateMouseOverUI();
        }
        else highlightedNeuron = neuron;

        
    }
   
}
