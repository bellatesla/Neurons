
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVisualizer : MonoBehaviour
{    
    //NeuronManager manager;
    public bool isActive = true;
    
    private void Start()
    {
        //manager = GetComponent<NeuronManager>();
    }

    //void Update()
    //{
    //    if (!isActive) return;

    //    foreach (var neuron in manager.neurons)
    //    {
    //        //UpdateNeuronColor(neuron);
    //        DrawLines(neuron);
    //    }
    //}    

    //void DrawLines(Neuron neuron)
    //{
    //    var lineFX = neuron.GetComponent<NeuronConnectionEffect>();
    //    if (neuron == null || lineFX == null) return;
    //    neuron.GetComponent<NeuronConnectionEffect>().DrawLines();
    //}

    //void UpdateNeuronColor(Neuron neuron)
    //{
    //    var color = neuron.GetComponent<NeuronColor>();
    //    if (neuron == null || color == null) return;
    //    neuron.GetComponent<NeuronColor>().ColorUpdate();
    //}

}
