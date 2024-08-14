using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronColor : MonoBehaviour
{    
    Neuron neuron;
    Material material;
    NeuronSettingsSO settings;
    public Color currentColor { get; private set; }  
    float smoothActivity;

    void Awake()
    {
        neuron = GetComponent<Neuron>();
        settings = neuron.settings;        
        material = neuron.GetComponent<Renderer>().material;
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
    private void Update()
    {
        if (smoothActivity <= float.Epsilon)
        {
            smoothActivity = 0;           
            ColorUpdate(smoothActivity);
            return;
        }

        // Interpolate smoothActivity toward zero
        float t = 1 / settings.signalColorActivityDecayDuration * Time.deltaTime;
        smoothActivity = Mathf.Lerp(smoothActivity, 0, t);
       
        
        ColorUpdate(smoothActivity);
    }

    //this neuron received a signal
    private void OnReceived(Neuron neuron)
    {        
        smoothActivity = neuron.voltage;
        ColorUpdate(smoothActivity);        
    }

    //this neuron was fired
    private void OnNeuronFired(Neuron neuron)
    {       
        smoothActivity = neuron.voltage;
        ColorUpdate(smoothActivity);        
    }

    public void ColorUpdate(float signal)
    {
        // not--Neuron can change to any color deppending on it's current activity voltage signalActivity;
        // currently Neuron cannnot change to any color deppending on it's current activity voltage signalActivity;
        if (signal < 0)
        {            
            if (neuron.neuronType == NeuronType.Inhibitory)
            {
                SetNegativeColors(-signal);
            }
            else
            {
                SetNegativeColors(-signal);
                //SetPositiveColors(-signal);               
            }  
        }
        if (signal >= 0)
        {
            //  Positive
            if (neuron.neuronType == NeuronType.Inhibitory)
            {
                SetNegativeColors(signal);               
            }
            else
            {
                SetPositiveColors(signal);
                //if (signal == 0)
                //{
                    
                //}
                //else SetNegativeColors(signal);
            }
        }

        currentColor = material.color;
    }

    private void SetPositiveColors(float value)
    {
        Color positiveColor = Color.Lerp(settings.inactiveColor, settings.positiveColor, value);
        Color emissiveColor = Color.Lerp(settings.inactiveColor, settings.positiveColor * Mathf.GammaToLinearSpace(settings.emission), value);
        material.SetColor("_EmissionColor", emissiveColor);
        material.color = positiveColor;
    }

    private void SetNegativeColors(float value)
    {
        Color negativeColor = Color.Lerp(settings.inactiveColor2, settings.negativeColor, value);
        Color emissiveColor = Color.Lerp(settings.inactiveColor2, settings.negativeColor * Mathf.GammaToLinearSpace(settings.emission), value);
        material.SetColor("_EmissionColor", emissiveColor);
        material.color = negativeColor;
    }
}
