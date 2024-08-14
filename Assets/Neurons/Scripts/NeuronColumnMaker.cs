using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronColumnMaker : MonoBehaviour
{
    [SerializeField] GameObject shape;
    [SerializeField] GameObject neuronPrefab;
    public int maxNuerons = 8;
    public float scale = .25f;
    public List<Neuron> neurons;
    public int seed;    

    [ContextMenu("Random Seed Neuron Column")]
    void RespawnNeurons()
    {
        seed = System.DateTime.Now.Millisecond;
        DeleteNeurons();
        SpawnNeurons();
    }
    [ContextMenu("Delete Neuron Column")]
    void DeleteNeurons()
    {
        foreach (var n in neurons)
        {
            DestroyImmediate(n.gameObject);
        }
        neurons = new List<Neuron>();
    }

    [ContextMenu("Spawn Neuron Column")]
    void SpawnNeurons()
    {
        Random.InitState(seed);

        var collider = shape.GetComponent<Collider>();

        for (int i = 0; i < maxNuerons; i++)
        {
            //find random position in cylinder
            float x = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
            float y = Random.Range(collider.bounds.min.y, collider.bounds.max.y);
            float z = Random.Range(collider.bounds.min.z, collider.bounds.max.z);
            
            Vector3 pos = new Vector3(x,y,z);
            var rot = Quaternion.identity;

            GameObject go = Instantiate(neuronPrefab, pos, rot);
            go.transform.localScale = new Vector3(scale, scale, scale);
            go.transform.SetParent(shape.transform);
            var neuron = go.GetComponent<Neuron>();

            if (Random.value > 0.5f) neuron.neuronType = NeuronType.Inhibitory;
            //var colorFX = go.GetComponent<NeuronColor>();
            //colorFX.autoUpdate = false;

            neurons.Add(neuron);            
        }
    }



    
}
