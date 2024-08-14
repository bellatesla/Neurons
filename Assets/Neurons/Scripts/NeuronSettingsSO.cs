using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeuronSettings", menuName = "Neurons/Neuron Settings", order = 1)]
public class NeuronSettingsSO : ScriptableObject
{
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

    [Header("Polarization Settings")]
    public float polarizationTriggerDuration = 0.5f;// if two signals recieved within this time
    public float depolarizationTriggerDuration = .1f;
    //public Vector2 randomStartActivityThreshold = new Vector2(.5f, 1f);

    [Header("Neuron Growth Settings")]
    public float connectionAddRadius = 1;
    public int maxConnections = 3;
    
    [Header("Effects Settings")]

    [Header("Colors Settings")]
    public Color positiveColor = new Color(1, 0, 0, 1);//red
    public Color negativeColor = new Color(0, 1, 0, 1);//green
    public Color inactiveColor = new Color(1, 1, 1, 1);//white
    public Color inactiveColor2 = new Color(0, 0, 0, 1);//black

    public float emission = 1;

    public float signalColorActivityDecayDuration = .3f;//how fast the signal decays in the neuron

    // line settings
    public LineSettings lineSettings;

    //public float lineWidthMax = .2f;
    //public float lineWidthMin = .01f;
    //public float lineWidthScale = .1f;

    //public Color linePositiveColor = new Color(1, 0, 0, 1);//red
    //public Color lineNegativeColor = new Color(0, 1, 0, 1);//green
    //public Color lineInactiveColor = new Color(1, 1, 1, 1);//white
    //public float lineColorActivityDecayDuration = 1.0f;//how fast the signal decays in the neuron
    
    [Header("Particle Effects")]
    public GameObject particleEffectsPrefab;
    public float particleSize = .2f;
    public float scaleStrength = 1;
}
