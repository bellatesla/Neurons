using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronModel : MonoBehaviour
{
    [SerializeField] Mesh boxMesh;
    [SerializeField] Mesh sphereMesh;
    
    Neuron neuron;
    private void Awake()
    {
        neuron = GetComponent<Neuron>();
    }
    private void Start()
    {        
        ShapeUpdate();
    }
    private void OnEnable()
    {        
        neuron.OnTypeChanged += SetMesh;        
        ShapeUpdate();
    }
    private void OnDisable()
    {
        neuron.OnTypeChanged -= SetMesh;        
    }

    public void ShapeUpdate()
    {
        SetMesh(neuron.neuronType);
    }

    private void SetMesh(NeuronType neuronType)
    {
        if (neuronType == NeuronType.Excitory)
        {
            GetComponent<MeshFilter>().mesh = sphereMesh;
        }
        else
            GetComponent<MeshFilter>().mesh = boxMesh;
    }
    
}
