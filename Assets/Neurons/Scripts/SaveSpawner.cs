using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveSpawner : MonoBehaviour
{
    public GameObject neuronPrefab;
    GameObject currentPrefab;
    public GameObject SpawnObject(NeuronSaveData data)
    {
        if (data.prefabName =="Neuron")
        {
            print($"Found prefabNamed {data.prefabName}..Spawning");
            currentPrefab = neuronPrefab;
        }

        return Instantiate(currentPrefab);        
    }
    public GameObject SpawnObject(int data)
    {
        if (data==0)
        {
            print("Found OtherType..Spawning");
            //currentPrefab = neuronPrefab;
        }

        return null;//Instantiate(currentPrefab);
    }
}
