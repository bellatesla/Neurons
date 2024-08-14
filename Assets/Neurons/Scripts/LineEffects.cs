using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Neuron))]
public class LineEffects : NeuronEffect
{
    public LineSettings lineSettings;
    public bool overrideGlobalLineSettins;

    protected List<LineRenderer> connectionLines = new List<LineRenderer>();    

    void Update()
    {
        float t = 1 / LineSettings().lineDecayDuration * Time.deltaTime;
        smoothActivity = Mathf.Lerp(smoothActivity, 0, t);

        UpdateConnectionColors(smoothActivity);
        TurnOffUnsedLines();
    }    
    protected override void OnFired(Neuron neuron)
    {
        base.OnFired(neuron);
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
                    Color connectionColor;
                    if (neuron.neuronType == NeuronType.Inhibitory)
                    {
                        //t *= -1;//flip for neg values
                        connectionColor = Color.Lerp(LineSettings().lineInactiveColor, LineSettings().lineNegativeColor, value);
                    }
                    else
                    {
                        connectionColor = Color.Lerp(LineSettings().lineInactiveColor, LineSettings().linePositiveColor, value);
                    }

                    lineRenderer.startColor = connectionColor;
                    lineRenderer.endColor = connectionColor;
                    //lineRenderer.material.SetColor("_EmissionColor", connectionColor * Mathf.GammaToLinearSpace(lineSettings.emission));
                //}

                float strength = neuron.connectionStrengths.ContainsKey(connection) ? neuron.connectionStrengths[connection] : 0;
                float width = Mathf.Clamp(strength, LineSettings().lineWidthMin, LineSettings().lineWidthMax) * LineSettings().lineWidthScale; ;
                
                lineRenderer.startWidth = width * LineSettings().lineStartWidth;
                lineRenderer.endWidth = width * LineSettings().lineEndWidth;
                
                // Update the LineRenderer positions
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, connection.transform.position);
            }
        }
    }
    private void TurnOffUnsedLines()
    {
        // Turn off lines that lose connection -- simple pooling
        for (int i = 0; i < connectionLines.Count; i++)
        {
            if (neuron.connections.Count <= i)
            {
                connectionLines[i].gameObject.SetActive(false);
            }
            else
            {
                connectionLines[i].gameObject.SetActive(true);
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
            lineRenderer.material = LineSettings().baseLineMaterial;
            //lineRenderer.
            connectionLines.Add(lineRenderer);
        }
        
        return connectionLines[index];
    }
    LineSettings LineSettings()
    {
        if (overrideGlobalLineSettins)
        {
            return lineSettings;
        }

        return neuron.settings.lineSettings;
    }

}
