using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronMotor : MonoBehaviour
{
    public List<Neuron> connections;//0=left, 1 = middle, right = right
    public float rotationSpeed = 1;
    public float currentRotation;
    public float signalValue;

    void Start()
    {
        currentRotation = transform.eulerAngles.y;       
    }

    void Update()
    {
        //base.Update();

        float signalSum = 0;
         
        foreach (var neuron in connections)
        {
            //SignalReceived(neuron);
            signalSum += neuron.voltage;
        }
        
        MoveOnSignal(signalSum);
       
    }

    void MoveOnSignal(float signal)
    {
        //print("Motor updating");        
        //go left or right -1,1
        signal = Mathf.Clamp(signal, -1, 1);
        signalValue = signal;
        currentRotation = rotationSpeed * signal * Time.deltaTime;        
        transform.Rotate(Vector3.up,currentRotation);
    }
}
