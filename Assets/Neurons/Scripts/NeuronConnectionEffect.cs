using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class NeuronConnectionEffect : MonoBehaviour
{
    Neuron neuron;  

    public Material baseLineMaterial;
    List<LineRenderer> connectionLines = new List<LineRenderer>();
    NeuronSettingsSO settings;    
    float smoothActivity;
    
    private void Awake()
    {
        neuron = GetComponent<Neuron>();
        settings = neuron.settings;        
    }    
    private void OnEnable()
    {
        neuron.OnReceived += OnReceived;
        neuron.OnFired += OnNeuronFired;
    }
    private void OnDisable()
    {
        neuron.OnReceived -= OnReceived;
        neuron.OnFired -= OnNeuronFired;
    }
    private void OnNeuronFired(Neuron obj)
    {
        smoothActivity = neuron.voltage;
        UpdateConnectionColors(smoothActivity);
    }
    private void OnReceived(Neuron obj)
    {
        smoothActivity = neuron.voltage;
        UpdateConnectionColors(smoothActivity);
    }
    private void Update()
    {

        // Interpolate smoothActivity toward zero
        float t = 1 / settings.lineSettings.lineDecayDuration * Time.deltaTime;
        smoothActivity = Mathf.Lerp(smoothActivity, 0, t);


        UpdateConnectionColors(smoothActivity);
    }
    void UpdateConnectionColors(float value)
    {

        for (int i = 0; i < neuron.connections.Count; i++)
        {
            var connection = neuron.connections[i];
            
            if (connection == null) return;

            LineRenderer lineRenderer = GetLineRenderer(i, connection);
            if (lineRenderer != null)
            {
                float strength = neuron.connectionStrengths.ContainsKey(connection) ? neuron.connectionStrengths[connection] : 0;
                
                float t = value;
                
                Color connectionColor;
                
                if (t < 0)
                {
                    t *= -1;//flip for neg values
                    connectionColor = Color.Lerp(settings.lineSettings.lineInactiveColor, settings.lineSettings.lineNegativeColor, t);
                }
                else
                {
                    connectionColor = Color.Lerp(settings.lineSettings.lineInactiveColor, settings.lineSettings.linePositiveColor, t);
                }
                
                lineRenderer.startColor = connectionColor;
                lineRenderer.endColor = connectionColor;
                float width = Mathf.Clamp(strength, settings.lineSettings.lineWidthMin, settings.lineSettings.lineWidthMax);
                width *= settings.lineSettings.lineWidthScale;
                lineRenderer.startWidth = width;
                lineRenderer.endWidth = width;
                // Update the LineRenderer positions
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, connection.transform.position);
            }
        }
    }
    LineRenderer GetLineRenderer(int index, Neuron connection)
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
