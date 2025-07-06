using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEditor.MemoryProfiler;


public class Neuron : MonoBehaviour
{
    public static Action<string> OnNeuronStateChange { get; internal set; }
    public event Action<Neuron> OnFired;
    public event Action<Neuron> OnReceived;
    public event Action<NeuronType> OnTypeChanged;
    

    public NeuronType neuronType = NeuronType.Excitory;
    
    public Connections connections;  
   
    public float voltage { get; private set; }//-1,1

    //public int additionalConnections = 0;
    public bool hasfireOnceOnStart;
    public float timeSinceLastSignal;
    float lastSignalReceivedTime;
    public float lastSignalValueIn;
    public float lastFiringTime;
    private bool signalBlocked;
    

    // Global settings
    public NeuronSettingsSO settings;   //manually assigned or is asssigned in prefab 
    private GlobalNeuronEvents globalStateManager; // remove and create upgrade to static class ref
       
    
    protected virtual void Start()
    {
        //send event on start
        OnTypeChanged?.Invoke(neuronType);
    }

    private void OnEnable()
    {
        globalStateManager = FindObjectOfType<GlobalNeuronEvents>();
        globalStateManager.RegisterNeuron(this);
    }

    private void OnDisable()
    {       
        globalStateManager.UnRegisterNeuron(this);
    }

    protected virtual void Update()
    {        
        CheckThresholdVoltage();
        DecayVoltage();
        
        //Coneection grow or weaken
        connections.Weaken(this);
        connections.RemoveWeakConnections(this);
        connections.AddConnection(this);        
    }

    void ReceiveSignal(float input)
    {
        if (signalBlocked)
        {
            return;
        }
        
        timeSinceLastSignal = Time.time - lastSignalReceivedTime;
        lastSignalReceivedTime = Time.time;
        lastSignalValueIn = input;
       
        voltage += input;       
        
        OnReceived?.Invoke(this);
        
    }   

    public void ForceFire(float value)
    {
        ReceiveSignal(value);             
    }

    internal void StopFiring()
    {
        StartCoroutine(BlockSignalsForSeconds(5));
    }

    protected void Fire()
    {
        // Supress firing if we are within cooldown time
        if ((Time.time - lastFiringTime) < settings.firedCooldownDuration)
        {
            voltage = 0;
            return;
        }

        foreach (var connection in connections)
        {
            connections.Strengthen(connection);

            var dist = Vector3.Distance(transform.position, connection.transform.position);
            var delay = dist / settings.signalSpeed;

            float signalOut = Mathf.Clamp(voltage, -1.0f, 1.0f);

            OnFired?.Invoke(this);

            // A delay for the receivng neuron to account for the transmission speed
            SendSignalDelayed(connection, signalOut, delay);

        }
        // has fired, reset
        voltage = 0;
        lastFiringTime = Time.time;
    }

    private IEnumerator BlockSignalsForSeconds(float duration)
    {
        //float duration = 5f;       // Total time to perform the action
        float elapsedTime = 0f;    // Track the elapsed time

        // Perform the action for duration
        while (elapsedTime < duration)
        {    
            elapsedTime += Time.deltaTime;
            //force zero
            signalBlocked = true;
            
            // Wait until the next frame
            yield return null;
        }
        signalBlocked = false;
    }
    
    private void CheckThresholdVoltage()
    { 
        // Fires neuron if threshold is reached

        if (neuronType == NeuronType.Excitory)
        {
            //fires on positive voltage
            if (voltage >= settings.firingThresold)
            {              
                Fire();                
            }
        }
        else // Inhibitory
        {
            //fires on positive voltage
            if (voltage >= settings.firingThresold && settings.inhibitoryFiresOnPositive)
            {
                // fire a negative voltage as inhibitor
                if (voltage > 0) voltage *= -1;
                Fire();
            }

            //fires on negative voltage
            if (voltage <= -settings.firingThresold && settings.inhibitoryFiresOnNegative)
            {                
                Fire();
            }
            
        }

    }   
   
    private void DecayVoltage()
    {
        float decayAmount = 1.0f/settings.signalActivityDecayDuration * Time.deltaTime;

        if (voltage > 0)
        {
            voltage -= decayAmount;
            voltage = Mathf.Clamp(voltage, 0, 1);
        }
        else if (voltage < 0)
        {
            voltage += decayAmount;
            voltage = Mathf.Clamp(voltage, -1, 0);
        }
    }
    
    private void SendSignalDelayed(Neuron neuron, float signal, float delay)
    {
        //Restrict to directly being called
        IEnumerator SendSignalDelay(Neuron neuron, float signal, float delay)
        {
            yield return new WaitForSeconds(delay);
            neuron.ReceiveSignal(signal);
        }

        StartCoroutine(SendSignalDelay(neuron, signal, delay));
    }
    
    internal void Invert()
    {
        if (neuronType == NeuronType.Excitory)
        {
            neuronType = NeuronType.Inhibitory;
            OnTypeChanged?.Invoke(neuronType);
        }
        else
        {
            neuronType = NeuronType.Excitory;
            OnTypeChanged?.Invoke(neuronType);
        }
    } 

    ///Move to debug script
    //void OnDrawGizmosSelected()
    //{
    //    DrawConnectionRadius();
    //    // Visualize connections
    //    Gizmos.color = Color.red;
    //    foreach (var connection in dendrites.connections)
    //    {
    //        Gizmos.DrawLine(transform.position, connection.transform.position);
    //    }
    //    Gizmos.color = Color.white;
    //}

    /// move to debug script
    //protected virtual void DrawConnectionRadius()
    //{        

    //    Gizmos.color = Color.green;        
    //    Gizmos.DrawWireSphere(transform.position, settings.connectionAddRadius);
    //    Gizmos.color = Color.white;
    //}

}



[Serializable]
public class Connections: List<Neuron>
{
    public static event Action<Neuron, Neuron> OnRemoveFromConnectionTo;
    public static event Action<Neuron, Neuron> OnAddFromConnectionTo;
     
    public Dictionary<Neuron, float> strengths = new Dictionary<Neuron, float>();

    public int totalConnections => Count;

    public void RemoveWeakConnections(Neuron neuron)
    {
        // Example: Prune weak connections
        List<Neuron> toRemove = new List<Neuron>();
        foreach (var n in this)
        {
            if (strengths.ContainsKey(n) && strengths[n] < neuron.settings.removeConnectionThreshold)
            {
                toRemove.Add(n);
            }

            if (n == null)
            {
                Debug.Log("Removing Null Connection");
                toRemove.Add(n);
            }
        }

        foreach (var weekConnection in toRemove)
        {
            Debug.Log("Removing weak connection");
            Remove(weekConnection);
            strengths.Remove(weekConnection);
            OnRemoveFromConnectionTo?.Invoke(neuron, weekConnection);
        }
    }

    public void AddConnection(Neuron neuron)
    {
        // Try to add a new connections if below amount
        if (Count < neuron.settings.maxConnections) 
        {

            Neuron randomNeuron = null;

            // Creates a distant connection with find conditions         
            // test 1: 1out of 10 connections are distanced over near
            if (Count % 10 == 0)
            {
                randomNeuron = FindRandomNeuronByDistance(neuron, neuron.settings.connectionAddRadius * 5);
            }
            else
            {
                randomNeuron = FindRandomNeuron(neuron);//near by distance
            }

            // if we found a possible new connection, connect to it
            if (randomNeuron != null && !Contains(randomNeuron))
            {
                Add(randomNeuron);
                strengths[randomNeuron] = neuron.settings.connectionStrengthDefault;// Initialize connection strength
                OnAddFromConnectionTo?.Invoke(neuron, randomNeuron);
            }
        }
    }

    public void Strengthen(Neuron neuron)
    {
        if (neuron == null)
        {
            return;
        }

        // Ensure the connection strength entry exists
        if (!strengths.ContainsKey(neuron))
        {
            strengths[neuron] = neuron.settings.connectionStrengthDefault; // Initialize connection strength
        }

        // Strengthen the connection
        strengths[neuron] += neuron.settings.connectionStrengthenRate;

        if (strengths[neuron] > neuron.settings.connectionStrengthMax)
        {
            strengths[neuron] = neuron.settings.connectionStrengthMax;
        }
    }

    public void Weaken(Neuron neuron)
    {
        foreach (var connection in this)
        {
            if (strengths.ContainsKey(connection))
            {
                strengths[connection] -= neuron.settings.connectionWeakenRate * Time.deltaTime;
            }
        }
    }
    
    Neuron FindRandomNeuronByDistance(Neuron neuron, float distance)
    {
        //find neurons by bigger radius
        Collider[] colliders = Physics.OverlapSphere(neuron.transform.position, distance);
        List<Neuron> allNeurons = new List<Neuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            //if we are distant and not null
            if (n && Vector3.Distance(neuron.transform.position, n.transform.position) > neuron.settings.connectionAddRadius)
            {
                allNeurons.Add(n);
            }
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

    Neuron FindRandomNeuron(Neuron neuron)
    {
        //find neurons by radius
        Collider[] colliders = Physics.OverlapSphere(neuron.transform.position, neuron.settings.connectionAddRadius);
        List<Neuron> allNeurons = new List<Neuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            if (n) allNeurons.Add(n);
        }

        if (allNeurons.Count > 1)
        {
            Neuron randomToNeuron = neuron;
            while (randomToNeuron == neuron)
            {
                randomToNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            }

            return randomToNeuron;
        }
        return null;
    }

}
