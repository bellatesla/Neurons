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

    public Color linePositiveColor = Color.red;
    public Color lineNegativeColor = Color.green;
    public Color lineInactiveColor = Color.white;
    public float emission = 0;
    public Material baseLineMaterial;
}
