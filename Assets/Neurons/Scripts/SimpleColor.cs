using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColor : MonoBehaviour
{        
    [SerializeField] NeuronSettings settings;
    
    SimpleNeuron neuron;
    Material material;
    bool hasUpdated;

    void Start()
    {
        neuron = GetComponent<SimpleNeuron>();
        material = neuron.GetComponent<Renderer>().material;
        neuron.OnFired += Fired;
        neuron.OnReceviedSignal += OnReceived;
    }

    //this neuron received a signal
    private void OnReceived(SimpleNeuron neuron)
    {
        print("fired recieved signal color change");
        smoothActivity = neuron.activityLevel;
        ColorUpdate(smoothActivity);
    }

    //this neuron was fired
    private void Fired(SimpleNeuron neuron)
    {
        print("fired color change");
        smoothActivity = neuron.activityLevel;
        ColorUpdate(smoothActivity);
    }
    float smoothActivity;
    public float decaytime = 1;
    private void Update()
    {
        /// Ensure smoothActivity decays to zero over time
        if (Mathf.Abs(smoothActivity) > Mathf.Epsilon)
        {
            // Interpolate smoothActivity toward zero
            float t = 1/settings.signalColorActivityDecayDuration * Time.deltaTime;
            smoothActivity = Mathf.Lerp(smoothActivity, 0, t);
        }
        // prevents updating color twice in one frame
        //if (hasUpdated)
        //{
        //    hasUpdated = false;
        //    return;
        //}

        ColorUpdate(smoothActivity);
    }

    public void ColorUpdate(float value)
    {
        hasUpdated = true;

        float t = value;        

        if (t <= 0)
        {
            t *= -1;//flip for neg values
            material.color = Color.Lerp(settings.inactiveColor, settings.negativeColor, t);
        }
        else
        {
            material.color = Color.Lerp(settings.inactiveColor, settings.positiveColor, t);
        }
    }

}
