using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuronUILine : MonoBehaviour
{
    public Image dot;
    public Material baseLineMaterial;
    LineRenderer lineRenderer;
    public float lineDistance = 1;
    public float lineWidthStart = .1f;
    public float lineWidthEnd = .1f;
    public Color colorStart = Color.white;
    public Color colorEnd = Color.white;

    private void Start()
    {
        SetLineRenderer();
    }

    private void Update()
    {     
        var neuron = NeuronUI.instance.selectedNueron;
       
        if (neuron)
        {
            UpdateUiLine(neuron);
            ColorUpdate2(neuron);
        }
        
    }
    private void ColorUpdate2(Neuron neuron)
    {
        //Get current neuron's color
        var neuronColor = neuron.GetComponent<NeuronColor>();
        if (neuronColor)
        {
            dot.color = neuronColor.currentColor;
        }
    }    

    void UpdateUiLine(Neuron neuron)
    {
        lineRenderer.startColor = colorStart;
        lineRenderer.endColor = colorEnd;        
        lineRenderer.startWidth = lineWidthStart;
        lineRenderer.endWidth = lineWidthEnd;
       
        Vector3 worldPosition = neuron.transform.position;
        // Convert world position to screen position
        RectTransform rect = dot.GetComponent<RectTransform>();
        Vector3 offsetPosition = rect.position;
        offsetPosition.z = lineDistance;
        
        // Update the positions of the LineRenderer
        lineRenderer.SetPosition(0, worldPosition);
        //Vector3 newPos = Camera.main.ViewportToScreenPoint(rect.position);
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(offsetPosition));
    }


    void SetLineRenderer()
    {
        if (lineRenderer == null)
        {
            GameObject lineObject = new GameObject($"UI Current Data Line");
            lineObject.transform.parent = transform;
            lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = baseLineMaterial;
        }        
    } 
}

