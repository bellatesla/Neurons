using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleNeuronSettings", menuName = "Neurons/Simple Neuron Settings")]
public class NeuronSettings : ScriptableObject
{
    public float signalSpeed = 1;//how fast the signal travels down the connections
    public float signalActivityDecayRate = .1f;//how fast the signal decays in the neuron
    
    public float signalActivityIncreaseAmount = 0.1f;//how much the activity increases when a signal is received
    public float connectionStrengthenRate = 0.1f;
    public float connectionWeakenRate = 0.001f;
    public float connectionStrengthDefault = 0.5f;
    public float connectionStrengthMax = 1f;
    public float removeConnectionThreshold = 0.01f;
    public int connectionAmountMax = 2;//add new neuron connections below this value   
    
    public Vector2 randomStartActivityThreshold = new Vector2(.5f, 1f);
    [Header("Grow Neuron Settings")]
    public float connectionAddRadius = 1;
    public int maxConnections = 3;    
    

    [Header("Neuron Colors Settings")]
    public Color positiveColor = new Color(1, 0, 0, 1);//red
    public Color negativeColor = new Color(0, 1, 0, 1);//green
    public Color inactiveColor = new Color(1, 1, 1, 1);//white
    public float signalColorActivityDecayDuration = .3f;//how fast the signal decays in the neuron
    
    
    [Header("Line Settings")]
    public float lineWidthMax = .2f;
    public float lineWidthMin = .01f;
    public float lineWidthScale = .1f;

    public Color linePositiveColor = new Color(1, 0, 0, 1);//red
    public Color lineNegativeColor = new Color(0, 1, 0, 1);//green
    public Color lineInactiveColor = new Color(1, 1, 1, 1);//white

    [Header("Particle Effects")]
    public GameObject particleEffectsPrefab;
    public float particleSize = .2f;

}
