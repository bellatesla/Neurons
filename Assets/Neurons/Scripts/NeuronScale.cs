using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronScale : MonoBehaviour
{    
    Neuron neuron;
    NeuronSettingsSO settings;
    float smoothActivity;
    Vector3 _originalScale;

    private void Awake()
    {
        neuron = GetComponent<Neuron>();
        settings = neuron.settings;
        //neuron.OnFired += OnFired;
        //neuron.OnReceived += OnReceived;
        _originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        neuron.OnFired += OnFired;
        neuron.OnReceived += OnReceived;
    }
    private void OnDisable()
    {
        neuron.OnFired -= OnFired;
        neuron.OnReceived -= OnReceived;
    }

    private void OnReceived(Neuron obj)
    {
        smoothActivity = neuron.voltage;
        UniformScaleObject(smoothActivity);
    }

    private void OnFired(Neuron neuron)
    {
        smoothActivity = neuron.voltage;
        UniformScaleObject(smoothActivity);
    }
    
    private void Update()
    {
        // Interpolate smoothActivity toward zero
        float t = 1 / settings.signalColorActivityDecayDuration * Time.deltaTime;
        smoothActivity = Mathf.Lerp(smoothActivity, 0, t);

        //smoothActivity = neuron.activityLevel;
        UniformScaleObject(smoothActivity);
    }
    public void SetDefaultScale(Vector3 scale)
    {
        _originalScale = scale;
    }
    public Vector3 GetDefaultScale()
    {
        return _originalScale;
    }

    void UniformScaleObject(float scale)
    {
        Vector3 newScale;
        if (neuron.neuronType == NeuronType.Inhibitory)
        {
            //scales goes up either polariity on inhibitory
            scale = Mathf.Abs(scale);
            newScale = _originalScale + (scale * settings.scaleStrength * Vector3.one);
        }
        else
        {
            newScale = _originalScale + (scale/4 * settings.scaleStrength * Vector3.one);
        }
        

        //Vector3 newScale = originalScale + (Vector3.one * (MathF.Abs(scale) * settings.scaleStrength));
        transform.localScale = newScale;

    }
}
