using System;
using System.Collections;
using UnityEngine;

public class RadialMenuToNeuron : MonoBehaviour
{
    private void Start()
    {        
        // UI radial button events
        RadialMenuController.OnFiredPositive += OnFiredPositiveButton;
        RadialMenuController.OnFiredNegative += OnFiredNegativeButton;
        RadialMenuController.OnInvert += OnInvertButton;
        RadialMenuController.OnRemoveConnection += OnRemoveConnectionButton;
        RadialMenuController.OnToggleInfoPanel += OnToggleInfoPanel;

        // Neuron mouse events
        GlobalNeuronEvents.OnMouseOverNeuron += OnMouseOverNeuron;
        GlobalNeuronEvents.OnMouseOverNeuron += OnSelectedNeuron;
        GlobalNeuronEvents.OnMouseExitNeuron += OnMouseExitNeuron;
    }

    private void OnToggleInfoPanel(Neuron neuron)
    {
        var panel = neuron.GetComponent<NeuronInfoPanel>();
        if (panel)
        {
            panel.Toggle();
        }
    }

    private void OnFiredPositiveButton(Neuron neuron)
    {
        neuron.ForceFire(1);
    }
    private void OnFiredNegativeButton(Neuron neuron)
    {
        neuron.ForceFire(-1);
    }
    private void OnInvertButton(Neuron neuron)
    {
        neuron.Invert();
    }
    private void OnRemoveConnectionButton(Neuron neuron)
    {
        int count = neuron.connections.Count;
        if (count > 0)
        {
            neuron.connections.RemoveAt(count - 1);
            //add remove last conn in neuron
        }
    }    
    private void OnMouseExitNeuron(Neuron neuron)
    {
        //currentOnMouseOverNeuron = null;
    }
    void OnSelectedNeuron(Neuron neuron)
    {
        //selectedNueron = neuron;
    }
    void OnMouseOverNeuron(Neuron neuron)
    {       
       // currentOnMouseOverNeuron = neuron;       
    }
   
}