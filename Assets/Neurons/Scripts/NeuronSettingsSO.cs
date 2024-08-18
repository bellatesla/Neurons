using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeuronSettings", menuName = "Neurons/Neuron Settings", order = 1)]
public class NeuronSettingsSO : ScriptableObject
{
    // todo public NeuronSettings 
    [Header("Neuron Signal Settings")]
    public float signalSpeed = 1;//how fast the signal travels down the connections
    public float signalActivityDecayDuration = .1f;//how fast the signal decays in the neuron
    public float signalActivityIncreaseAmount = 0.1f;//how much the activity increases when a signal is received
    public float firingThresold = .7f;
    [Header("Neuron Connection Settings")]
    public float connectionStrengthenRate = 0.1f;
    public float connectionWeakenRate = 0.001f;

    public float connectionStrengthMax = 1f;
    public float connectionStrengthDefault = 0.5f;

    public float removeConnectionThreshold = 0.01f;

    [Header("Neuron Growth Settings")]
    public float connectionAddRadius = 1;
    public int maxConnections = 3;

    [Header("Effects Settings")]

    [Header("Colors Settings")]
    public EffectColors neuronColors; 
    public float emission = 1;
    public float signalColorActivityDecayDuration = .3f;//how fast the signal decays in the neuron

    // line settings
    public LineSettings lineSettings;

    [Header("Particle Effects")]
    public EffectColors particleColors;
    public GameObject particleEffectsPrefab;
    public float particleSize = .2f;
    public float scaleStrength = 1;
    
    [Header("UI Panels")]
    public Vector3 infoPanelOffset;
    internal int infoPaneltextSpeed=1;
}
