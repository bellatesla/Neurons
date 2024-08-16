using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalNeuronEvents : MonoBehaviour
{
    //UI interface
    // list of ui butttons that need to be hooked up

    //Neurons that register can be given a global command
    private List<Neuron> neurons = new List<Neuron>();
    //public static event Action<Neuron> OnNeuronFired;

    //neuron global events
    public static event Action<Neuron> OnMouseOverNeuron;
    public static event Action<Neuron> OnMouseUpNeuron;
    public static event Action<Neuron> OnMouseDownNeuron;
    public static event Action<Neuron> OnMouseExitNeuron;
    public static event Action<Neuron> OnDragNeuron;
    public static event Action<Neuron> OnDragEndNeuron;
   
    //not used yet
    public void RegisterNeuron(Neuron neuron)
    {
        neurons.Add(neuron);
    }
    void OnEnable()
    {
        Neuron.OnNeuronStateChange += HandleNeuronStateChange;
    }

    void OnDisable()
    {
        Neuron.OnNeuronStateChange -= HandleNeuronStateChange;
    }

    // Events set from neurons   
    public static void SetOnMouseOverNeuron(Neuron neuron)
    {        
        OnMouseOverNeuron?.Invoke(neuron);
    }
    public static void SetOnMouseUpNeuron(Neuron neuron)
    {
        OnMouseUpNeuron?.Invoke(neuron);
    }
    public static void SetOnMouseDownNeuron(Neuron neuron)
    {
        OnMouseDownNeuron?.Invoke(neuron);
    }
    public static void SetOnMouseExitNeuron(Neuron neuron)
    {
        OnMouseExitNeuron?.Invoke(neuron);
    }
    public static void SetOnDragNeuron(Neuron neuron)
    {
        OnDragNeuron?.Invoke(neuron);
    }
    public static void SetOnDragEndNeuron(Neuron neuron)
    {
        OnDragEndNeuron?.Invoke(neuron);
    }
   
    
    public static void HandleNeuronStateChange(string state)
    {
        // Process state change in the global system
    }

    public void SetGlobalState(string state)
    {
        foreach (Neuron neuron in neurons)
        {
            //neuron.ChangeState(state);
        }
    }

    public void StopAllNeurons()
    {
        foreach (Neuron neuron in neurons)
        {
            //neuron.StopFiring();
        }
    }
}
