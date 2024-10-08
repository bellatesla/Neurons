using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalNeuronEvents : MonoBehaviour
{
    //UI interface
    // ui butttons to be hooked up

    //Neurons that register can be given a global command
    private List<Neuron> neurons = new List<Neuron>();
    
    public static event Action OnCameraDrag;
    public static event Action OnCameraDragEnd;
    //neuron events   
    public static event Action<Neuron> OnMouseOverNeuron;
    public static event Action<Neuron> OnMouseUpNeuron;
    public static event Action<Neuron> OnMouseDownNeuron;
    public static event Action<Neuron> OnMouseExitNeuron;
    public static event Action<Neuron> OnDragNeuron;
    //public static event Action<Neuron> OnDragEndNeuron;//none use mouse up
    //public static event Action<Neuron> OnClickedNeuron;//none use on down


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
   
   
    
    public static void HandleNeuronStateChange(string state)
    {
        // Process state change in the global system
    }

    // Camera Events
    internal static void SetOnCameraDrag()
    {
        OnCameraDrag?.Invoke();
    }
    internal static void SetOnCameraDragEnd()
    {
        OnCameraDragEnd?.Invoke();
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
            neuron.StopFiring();
        }
    }
    public void FireAllNeurons()
    {
        foreach (Neuron neuron in neurons)
        {
            neuron.ForceFire(1);
        }
    }
    public void DisconnectAllNeurons()
    {
        foreach (Neuron neuron in neurons)
        {
            neuron.connections = new List<Neuron>();
        }
    }
}
