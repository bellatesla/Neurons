using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class NeuronInputs : MonoBehaviour
{
    public int additionalInputs = 5;
    NeuronSettingsSO settings;
    Neuron neuron;
    List<Connection> inputs = new List<Connection>();
    
    private void Start()
    {       
        neuron = GetComponent<Neuron>();
        settings = neuron.settings;
        
    }

    private void FixedUpdate()
    {
        //need to delete old connection reference that have been removed somehow

        if (inputs.Count < additionalInputs)
        {
            AddInputs();
        }
    }

    [ContextMenu("Add Additional Inputs")]
    void AddInputs()
    {
        for (int i = 0; i < additionalInputs; i++)
        {
            CreateNewInput();
        }
    }
    

    private void CreateNewInput()
    {
        Neuron randomNeuron = FindRandomNeuron(settings.connectionAddRadius);

        if (randomNeuron != null && !randomNeuron.connections.Contains(neuron))
        {
            print("Adding random connection");
            inputs.Add(new Connection(randomNeuron, settings.connectionStrengthDefault));
            randomNeuron.connections.Add(neuron);
            randomNeuron.connectionStrengths[neuron] = settings.connectionStrengthDefault;// Initialize connection strength
        }
    }

    

    Neuron FindRandomNeuron(float radius)
    {
        //find neurons by radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<Neuron> allNeurons = new List<Neuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            if (n) allNeurons.Add(n);
        }

        if (allNeurons.Count > 1)
        {
            Neuron randomNeuron = neuron;
            while (randomNeuron == neuron)
            {
                randomNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            }

            return randomNeuron;
        }
        return null;
    }
}
