using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableNeuron : MonoBehaviour, ISaveable
{    
    private List<int> connectionIds;
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
    public void SaveState(ref SaveSystem.SaveData savedData)
    {        
        NeuronSaveData newData = new NeuronSaveData();

        newData.id = GetID();//no need to set this id        
        
        // Save position
        newData.x = transform.position.x;
        newData.y = transform.position.y;
        newData.z = transform.position.z;
        newData.neuronType = GetComponent<Neuron>().neuronType;
        
        // Save Scale
        NeuronScale scaleComp = GetComponent<NeuronScale>();
        Vector3 scale = new Vector3();
        if (scaleComp)
        {
            scale = scaleComp.GetDefaultScale();
        }
        else scale = transform.lossyScale;

        newData.scale_x = scale.x;
        newData.scale_y = scale.y;
        newData.scale_z = scale.z;

        // save every neuron connection by game object id
        List<Neuron> connections = GetComponent<Neuron>().connections;
        newData.connectionIds = new List<int>();
               
        foreach (var neuron in connections)
        {
            newData.connectionIds.Add(neuron.GetComponent<SaveableNeuron>().GetID());           
        }
        newData.prefabName = "Neuron";

        savedData.neuronData.Add(newData);       
    }
    public void LoadState(SaveSystem.SaveData state, int index)
    {       
        // Apply saved data        
        NeuronSaveData data = state.neuronData[index];
        //Addobject
        //SaveSystem.idToObjectDictionary.Add(data.id,gameObject);
        // Position
        transform.position = data.position;
        // scale        
        neuron.neuronType = data.neuronType;//set type excite ot inhib       
        var scaler = GetComponent<NeuronScale>();
        if (scaler)
        {
            scaler.SetDefaultScale(data.scale);
        }

        // Initialize connectionIds here
        connectionIds = data.connectionIds ?? new List<int>();
        //print($"Connection Id {data.connectionIds}");

    }
    public void PostLoadState()
    {
        if (connectionIds == null || connectionIds.Count == 0)
        {
            Debug.LogWarning("No connections found to restore.");
            //return;
        }

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
    public void Dispose()
    {
        //handle when we need to delete self if we are not needed in the scene once loading completed
        print("Destroying unused object");
        Destroy(gameObject);
    }
    //public GameObject GetGameObject()
    //{
    //    return gameObject;
    //}
       
}


