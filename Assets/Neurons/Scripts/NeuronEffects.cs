using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronEffects : MonoBehaviour
{
    //Attach to any Neuron GameObject
    //public GameObject effectPrefab;
    public List<ParticleSystem> pss = new List<ParticleSystem>();//one per connection
    List<GameObject> effectObjects = new List<GameObject>();
    NeuronSettingsSO settings;
    Neuron neuron;
    Color connectionColor;
    
    
    private void Awake()
    {
        neuron = GetComponent<Neuron>();
        settings = neuron.settings;
             
    }
    private void Start()
    {
        UpdateEffectObjects();
    }

    private void OnEnable()
    {
        neuron.OnReceived += OnReceived;
        neuron.OnFired += OnNeuronFired;
    }

    private void OnNeuronFired(Neuron obj)
    {
        PlayEffects(obj);
    }

    private void OnReceived(Neuron obj)
    {
        //PlayEffects(obj);
    }

    private void OnDisable()
    {
        neuron.OnReceived -= OnReceived;
        neuron.OnFired -= OnNeuronFired;
    }

    private void UpdateEffectObjects()
    {
        foreach (var item in effectObjects)
        {
            Destroy(item.gameObject);
        } 

        effectObjects = new List<GameObject>();
        pss = new List<ParticleSystem>();
        
        foreach (var connection in neuron.connections)
        {
            if (connection != null) 
            {
                CreateEffectObject();
            }            

        }        
    }

    void CreateEffectObject()
    {
        //creating
        //print("Creating particle object");
        var go = Instantiate(settings.particleEffectsPrefab);
        go.transform.SetParent(neuron.transform);
        effectObjects.Add(go);
        var ps = go.GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        pss.Add(ps);
    }

    public void PlayEffects(Neuron neuron)
    {       
        if(effectObjects.Count < neuron.connections.Count)
        {
            UpdateEffectObjects();
        }

       
        for (int i = 0; i < neuron.connections.Count; i++)
        {            
            //play effect 
            effectObjects[i].transform.position = transform.position;
            effectObjects[i].transform.LookAt(neuron.connections[i].transform.position);

            var main = pss[i].main;
            main.startSpeed = settings.signalSpeed;
            var dist = Vector3.Distance(transform.position, neuron.connections[i].transform.position);
            var lifetime = dist / settings.signalSpeed;
            main.startLifetime = lifetime;
            main.startSize = settings.particleSize;
            
            if (neuron.neuronType == NeuronType.Inhibitory )
            {                
                connectionColor = Color.Lerp(settings.inactiveColor, settings.negativeColor, 1);
            }
            else
            {
                connectionColor = Color.Lerp(settings.inactiveColor, settings.positiveColor, 1);
            }

            main.startColor = connectionColor;
            pss[i].Play();
           
        }

    }    

}
