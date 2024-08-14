using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLines : MonoBehaviour
{
    SimpleNeuron neuron;

    public Material baseLineMaterial;
    private List<LineRenderer> connectionLines = new List<LineRenderer>();    
    [SerializeField] NeuronSettings settings;

    public bool showInput;
    public bool howOutput;
    
    private void Start()
    {        
        neuron = GetComponent<SimpleNeuron>();
        neuron.OnFired += Fired;
    }

    private void Fired(SimpleNeuron neuron)
    {
        print("fired");
        UpdateConnectionColorsOut();
    }

    void Update()
    {
        UpdateConnectionColorsOut();
    }

    void UpdateConnectionColorsOut()
    {
        for (int i = 0; i < neuron.connections_OUT.Count; i++)
        {            
            SimpleNeuron connection = neuron.connections_OUT[i];

            LineRenderer lineRenderer = GetLineRenderer(i);
            if (lineRenderer != null)
            {
                float strength = neuron.connectionStrengths.ContainsKey(connection) ? neuron.connectionStrengths[connection] : 0;

                float t = neuron.activityLevel;

                Color connectionColor;
                
                if (t <= 0)
                {                   
                    t *= -1;//flip for neg values
                    connectionColor = Color.Lerp(settings.inactiveColor, settings.negativeColor, t);
                }
                else
                {                    
                    connectionColor = Color.Lerp(settings.inactiveColor, settings.positiveColor, t);
                }

                lineRenderer.startColor = connectionColor;
                lineRenderer.endColor = connectionColor;
                float width = Mathf.Clamp(strength, settings.lineWidthMin, settings.lineWidthMax);
                lineRenderer.startWidth = width * settings.lineWidthScale;
                lineRenderer.endWidth = width * settings.lineWidthScale / 3;
                // Update the LineRenderer positions
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, connection.transform.position);
            }
        }
    }
    
    LineRenderer GetLineRenderer(int index)
    {
        if (index >= connectionLines.Count)
        {
            GameObject lineObject = new GameObject($"Connection_{index}");
            lineObject.transform.parent = transform;
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = baseLineMaterial;

            connectionLines.Add(lineRenderer);
        }

        return connectionLines[index];
    }
}
