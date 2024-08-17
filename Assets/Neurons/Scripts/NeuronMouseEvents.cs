using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any neuron to get global events.
/// This sends local on mouse: over, up, down, drag and exit
/// </summary>
[RequireComponent(typeof(Neuron))]
public class NeuronMouseEvents : MonoBehaviour
{
    Neuron neuron;

    private void Start()
    {
        neuron = GetComponent<Neuron>();
    }
    private void OnMouseExit()
    {       
        GlobalNeuronEvents.SetOnMouseExitNeuron(neuron);
    }    
    private void OnMouseDrag()
    {       
        GlobalNeuronEvents.SetOnDragNeuron(neuron);
    }    
    private void OnMouseUp()
    {
        GlobalNeuronEvents.SetOnMouseUpNeuron(neuron);        
    }
    private void OnMouseDown()
    {
        GlobalNeuronEvents.SetOnMouseDownNeuron(neuron);        
    }
    // Continuous
    private void OnMouseOver()
    {       
        GlobalNeuronEvents.SetOnMouseOverNeuron(neuron);
    }

    
}
