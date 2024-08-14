using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronSignal : MonoBehaviour
{
    public bool sendSignals;
    
    public float pulseFrequency = 1;
    public float pulseAmplitude = 1;
    public bool forceExitoryMode = true;

    public List<Neuron> neurons;
    float time;
    
   
    private void Start()
    {
        var neuron = GetComponent<Neuron>();
        if (neuron)
            neurons.Add(neuron);
    }

    void Update()
    {
        if (forceExitoryMode)
        {
            foreach (var neuron in neurons)
            {
                neuron.neuronType = NeuronType.Excitory;
            }
        }

        if (!sendSignals) return;
        time += Time.deltaTime;

        if(time >= 1/pulseFrequency)
        {
            //OUTPUT = pulseAmplitude;
            time = 0;//reset
            SendSignal();
        }
        
    }
    private void LateUpdate()
    {
        if (forceExitoryMode)
        {
            foreach (var neuron in neurons)
            {
                neuron.neuronType = NeuronType.Excitory;
            }
        }
    }
    void SendSignal()
    {
        foreach (var neuron in neurons)
        {
            neuron.ForceFire(pulseAmplitude);
        }
    }
}
