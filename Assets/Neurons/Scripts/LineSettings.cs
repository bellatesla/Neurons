using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineSettings
{   
    public float lineDecayDuration = .5f;
    public float lineWidthMin = .2f;
    public float lineWidthMax = 1f;
    public float lineStartWidth = 1f;
    public float lineEndWidth = .5f;
    public float lineWidthScale = 1f;
    
    public EffectColors effectColors;
    
    public float emission = 0;
    public Material baseLineMaterial;
}

[System.Serializable]
public class EffectColors
{
    public Color PositiveColor = Color.red;
    public Color NegativeColor = Color.green;
    public Color inactivePositiveColor = Color.white;
    public Color inactiveNegativeColor = Color.white;
}
