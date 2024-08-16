using System;
using System.Collections;
using UnityEngine;

public class RadialMenuToNeuron : MonoBehaviour
{
    private Neuron selectedNueron;//the nueron we last clicked
    private Neuron currentOnMouseOverNeuron;// current neuron the mouse is over
    private Neuron lastOnMouseOverNeuron;//the last neuron that the mouse was over
    
    private void Start()
    {        
        RadialMenuController.OnFiredPositive += OnFiredPositiveButton;
        RadialMenuController.OnFiredNegative += OnFiredNegativeButton;
        RadialMenuController.OnInvert += OnInvertButton;
        RadialMenuController.OnRemoveConnection += OnRemoveConnectionButton;
        GlobalNeuronEvents.OnMouseOverNeuron += OnMouseOverNeuron;
        GlobalNeuronEvents.OnMouseOverNeuron += OnSelectedNeuron;
        GlobalNeuronEvents.OnMouseExitNeuron += OnMouseExitNeuron;
    }
    private void OnRemoveConnectionButton()
    {
        int count = lastOnMouseOverNeuron.connections.Count;
        if (count > 0)
        {
            lastOnMouseOverNeuron.connections.RemoveAt(count - 1);
        }
    }
    private void OnInvertButton()
    {
        lastOnMouseOverNeuron.Invert();
    }

    private void OnMouseExitNeuron(Neuron neuron)
    {
        currentOnMouseOverNeuron = null;
    }

    void OnSelectedNeuron(Neuron neuron)
    {
        selectedNueron = neuron;
    }
    void OnMouseOverNeuron(Neuron neuron)
    {       
        currentOnMouseOverNeuron = neuron;
        lastOnMouseOverNeuron = neuron;
    }
    private void OnFiredPositiveButton()
    {       
        lastOnMouseOverNeuron.ForceFire(1);
    }

    private void OnFiredNegativeButton()
    {        
        lastOnMouseOverNeuron.ForceFire(-1);
    }
}