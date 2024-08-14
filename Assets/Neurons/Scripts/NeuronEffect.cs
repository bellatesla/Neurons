using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeuronEffect : MonoBehaviour
{
    public EffectsType effectsType;
    protected Neuron neuron;
    public float smoothActivity;
   
    protected virtual void Awake()
    {
        neuron = GetComponent<Neuron>();
    }
    private void OnEnable()
    {
        neuron.OnReceived += OnReceived;
        neuron.OnFired += OnFired;
    }
    private void OnDisable()
    {
        neuron.OnReceived -= OnReceived;
        //neuron.OnFired -= OnFired;
    }

    private void OnReceived(Neuron neuron)
    {
        smoothActivity = neuron.voltage;        
    }

    protected virtual void OnFired(Neuron neuron)
    {
        smoothActivity = neuron.voltage;       
    }
}

public enum EffectsType { None, Colors, Lines, Particles, Shape }
