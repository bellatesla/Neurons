using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UNUSED
/// </summary>
public class Signal
{
    Neuron from;
    Neuron to;
    public float voltage;
}

public class Connection
{
    public float strength;
    public Neuron neuron;

    public Connection(Neuron neuron, float value)
    {
        this.strength = value;
        this.neuron = neuron;
    }
}
