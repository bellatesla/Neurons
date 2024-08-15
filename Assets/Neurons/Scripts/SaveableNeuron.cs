using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableNeuron : Saveable
{    
    private List<int> connectionIds = new List<int>();
    Neuron _neuron;
    private Neuron neuron
    {
        get
        {
            if (_neuron==null)
            {
                _neuron = GetComponent<Neuron>();
            }
            return _neuron;
        }
    }

    public int GetID()
    {        
        return gameObject.GetInstanceID();
    }       
    public override void SaveState(ref SaveSystem.DataContainer savedData)
    {        
        NeuronSaveData newData = new NeuronSaveData();

        newData.id = GetID();      
        // Save position
        newData.position = transform.position; 
        // Neuron Type
        newData.neuronType = GetComponent<Neuron>().neuronType;        
        // Save scale
        NeuronScale scaleComp = GetComponent<NeuronScale>();
        
        if (scaleComp)
        {
            newData.scale = scaleComp.GetDefaultScale();
        }
        else newData.scale = transform.lossyScale;
        
       // Prepare connections
        List<Neuron> connections = GetComponent<Neuron>().connections;
        newData.connectionIds = new List<int>();
        
        // Save connections as ids 
        foreach (var neuron in connections)
        {
            newData.connectionIds.Add(neuron.GetComponent<SaveableNeuron>().GetID());           
        }

        //Save prefab name
        newData.prefabName = "Neuron";

        //Add the data to be saved
        savedData.objectSaves.Add(newData);       
    }
    public override void LoadState(SaveableTypeBase state)
    {        
        // get saved data entry     
        NeuronSaveData data = (NeuronSaveData)state;        
        // Position
        transform.position = data.position;
        // scale        
        neuron.neuronType = data.neuronType;//set type excite ot inhib       
        var scaler = GetComponent<NeuronScale>();
        if (scaler)
        {
            scaler.SetDefaultScale(data.scale);
        }

        // Initialize connectionIds
        connectionIds = data.connectionIds;       

    }
    public override void PostLoadState()
    {
        neuron.connections = new List<Neuron>();

        foreach (int conID in connectionIds)
        {
            if (SaveSystem.IDToObjectDictionary.TryGetValue(conID, out GameObject other))
            {
                Neuron newConnection = other.GetComponent<Neuron>();
                if (newConnection != null)
                {
                    neuron.connections.Add(newConnection);
                }
            }
            else
            {
                Debug.LogWarning($"No GameObject found with ID {conID} in the dictionary.");
            }
        }

        var modelComponent = GetComponent<NeuronModel>();
        if (modelComponent)
        {
            modelComponent.ShapeUpdate();
        }

        neuron.ForceFire(1);
    }
    public override void Dispose()
    {
        if (!gameObject.activeInHierarchy)
        {
            print($"GameObject {gameObject.name} is not active. Skipping dispose for this object.");
            return;
        }
        //handle when we need to delete self if we are not needed in the scene once loading completed
        print("Destroying self");
        Destroy(gameObject);
    }
    
       
}


