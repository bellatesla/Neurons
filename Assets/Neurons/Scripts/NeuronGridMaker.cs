using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronGridMaker : MonoBehaviour
{
    public Vector3 size = new Vector3(3,3,3);    
    public Vector3 spacing = Vector3.one;
    public GameObject neuronPrefab;
    public List<Neuron> neurons;
    public float inhibitoryPercent = .5f;
    public int seed;
    public NeuronSettingsSO settings;
    [ContextMenu("Update Neuron Grid")]
    void RespawnNeurons()
    {
        seed = System.DateTime.Now.Millisecond;
        DeleteNeurons();
        CreateNeuronGrid();
    }
    [ContextMenu("Delete Grid")]
    void DeleteNeurons()
    {
        foreach (var n in neurons)
        {
            if (Application.isPlaying)
            {
                Destroy(n.gameObject);
            }
            else DestroyImmediate(n.gameObject);
        }
        neurons = new List<Neuron>();
    }

    [ContextMenu("Create Grid")]
    void CreateNeuronGrid()
    {
        Vector3 gridCenter = new Vector3((size.x - 1) * spacing.x / 2.0f, (size.y - 1) * spacing.y / 2.0f, (size.z - 1) * spacing.z / 2.0f);
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < size.z; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                for (int k = 0; k < size.y; k++)
                {
                    Vector3 position = new Vector3(j * spacing.x, k * spacing.y, i * spacing.z) - gridCenter;

                    GameObject go = Instantiate(neuronPrefab);
                    go.transform.SetParent(transform);
                    go.transform.localScale = 0.25f * Vector3.one;
                    go.transform.localPosition = position;

                    var neuron = go.GetComponent<Neuron>();

                    // auto polarizes now
                    if (Random.value < inhibitoryPercent) neuron.neuronType = NeuronType.Inhibitory;                   

                    neurons.Add(neuron);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        //draw box
        var center = transform.position;
        var _size = Vector3.Scale(size, spacing);
        Gizmos.DrawWireCube(center, _size);
        //DrawConnectionRadius();

    }
   
}
