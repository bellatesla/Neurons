using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNeuronParticles : MonoBehaviour
{
    
    public List<ParticleSystem> pss = new List<ParticleSystem>();//one per connection
    List<GameObject> effectObjects = new List<GameObject>();// this object contains one or more particle systems
    //NeuronManager manager;
    [SerializeField]NeuronSettings settings;
    private void Start()
    {        
        UpdateEffectObjects();
    }

    private void UpdateEffectObjects()
    {
        
        foreach (var item in effectObjects)
        {
            Destroy(item.gameObject);
        }

        var neuron = GetComponent<SimpleNeuron>();
        //neuron.ClearSubscriptions();

        effectObjects = new List<GameObject>();
        pss = new List<ParticleSystem>();

        foreach (var connection in neuron.connections_OUT)
        {
            CreateEffectObject();                       
        }

        neuron.OnFired += PlayEffects;
    }

    void CreateEffectObject()
    {
        var go = Instantiate(settings.particleEffectsPrefab);
        go.transform.SetParent(transform);
        effectObjects.Add(go);
        var ps = go.GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        pss.Add(ps);
    }

    public void PlayEffects(SimpleNeuron neuron)
    {
        
        if (effectObjects.Count < neuron.connections_OUT.Count)
        {
            UpdateEffectObjects();            
        }

        for (int i = 0; i < neuron.connections_OUT.Count; i++)
        {
            //play effect 
            effectObjects[i].transform.position = transform.position;
            effectObjects[i].transform.LookAt(neuron.connections_OUT[i].transform.position);

            var main = pss[i].main;
            main.startSpeed = settings.signalSpeed;
            float dist = Vector3.Distance(transform.position, neuron.connections_OUT[i].transform.position);
            float lifetime = dist / settings.signalSpeed;
            main.startLifetime = lifetime;
            main.startSize = settings.particleSize;
            pss[i].Play(); 
        }

    }
}
