using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalGeneratorNeuron : Neuron
{
    public float frequency = 1;
    public float amplitude = 1;
   
    public int maxConnections = 3;
    public float connectionAddRadius = 1;
    float _time;

    protected override void Update()
    {   
        _time += Time.deltaTime;

        if (_time >= 1 / frequency)
        {            
            _time = 0;//reset            
            ForceFire(amplitude);//send a signal to this neuron (self)
        }

        AddNewConnections();
        RemoveConnections();
        //No base.Update(); since we don't car about anything but the fire method
    }

    void RemoveConnections()
    {
       //remove if distance too far
        List<Neuron> toRemove = new List<Neuron>();

        foreach (var connection in connections)
        {
            if (connection == null)
            {
                print("Removing Null Connection");
                toRemove.Add(connection);
            }
            else
            {
                var distance = Vector3.Distance(transform.position, connection.transform.position);
                if (distance > connectionAddRadius)
                {
                    toRemove.Add(connection);
                }
            }

        }        

        foreach (var neuron in toRemove)
        {            
            connections.Remove(neuron);            
        }
    }     

    private void AddNewConnections()
    {

        if (connections.Count < maxConnections) // Arbitrary max connections
        {

            Neuron randomNeuron = FindRandomNeuron();

            if (randomNeuron != null && !connections.Contains(randomNeuron))
            {
                print("Adding connection");

                connections.Add(randomNeuron);

            }
        }
    }

    Neuron FindRandomNeuron()
    {
        //find neurons by radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, connectionAddRadius);
        List<Neuron> allNeurons = new List<Neuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            if (n) allNeurons.Add(n);
        }

        if (allNeurons.Count > 1)
        {
            Neuron randomNeuron = this;
            while (randomNeuron == this)
            {
                randomNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            }

            return randomNeuron;
        }
        return null;
    }

    protected override void DrawConnectionRadius()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, connectionAddRadius);
        Gizmos.color = Color.white;
    }
}
