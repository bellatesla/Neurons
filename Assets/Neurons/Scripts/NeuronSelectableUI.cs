using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronSelectableUI : MonoBehaviour
{
    Neuron neuron;

    private void Start()
    {
        neuron = GetComponent<Neuron>();
    }
    private void OnMouseExit()
    {
        SetMouseExitNueron();
    }
    
    private void OnMouseDrag()
    {
        //begin drag
        //add new connection
        SetOnDragNeuron();

    }
    private void OnMouseUp()
    {
        //drag is over
        if (NeuronUI.instance == null) return;
        NeuronUI.instance.SetOnMouseUpNeuron(neuron);
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetSelectedNeuron();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetSelectedNeuron();
        }
    }
    // Continuous
    private void OnMouseOver()
    {
        SetOnMouseOverNeuron();
    }

    private void SetOnDragNeuron()
    {
        if (NeuronUI.instance == null) return;
        NeuronUI.instance.SetOnDragNeuron(neuron);
    }
    private void SetOnDragEndNeuron()
    {
        if (NeuronUI.instance == null) return;
        NeuronUI.instance.SetOnDragEndNeuron(neuron);
    }

    private void SetMouseExitNueron()
    {
        if (NeuronUI.instance == null) return;
        NeuronUI.instance.SetMouseExitNeuron(neuron);
    }


    private void SetOnMouseOverNeuron()
    {
        if (NeuronUI.instance == null) return;
            NeuronUI.instance.SetOnMouseOver(neuron);
    }

    private void SetSelectedNeuron()
    {
        if (NeuronUI.instance == null) return;
            NeuronUI.instance.SetSelectedNeuron(neuron);
    }
}
