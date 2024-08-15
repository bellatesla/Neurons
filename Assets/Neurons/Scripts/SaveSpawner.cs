using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveSpawner : MonoBehaviour
{
    public GameObject neuronPrefab;
    
    public GameObject SpawnObject(SaveableTypeBase savedItem)
    {
        if (savedItem is NeuronSaveData neuronData)
        {
            print($"Spawner found prefab name: {neuronData.prefabName}");
            return Instantiate(neuronPrefab);
        }

        return null;
    }
}
