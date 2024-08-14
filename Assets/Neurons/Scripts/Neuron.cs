using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Neuron : MonoBehaviour
{
    public NeuronType neuronType = NeuronType.Excitory;
    public NeuronFunction functionType = NeuronFunction.FeedforwardInhibition;
    public List<Neuron> connections = new List<Neuron>();
    public Dictionary<Neuron, float> connectionStrengths = new Dictionary<Neuron, float>();
    public float voltage { get; private set; }//-1,1
    public float debug_voltage;    
    public int additionalConnections = 0;    
    public bool hasfireOnceOnStart;

    public event System.Action<Neuron> OnFired;
    public event System.Action<Neuron> OnReceived;    
    public event System.Action<NeuronType> OnTypeChanged;
    public NeuronSettingsSO settings;   //manually assigned or is asssigned in prefab 
    public float timeSinceLastSignal;
    float lastSignalReceivedTime;
    public float lastSignalValue;

    private void Start()
    {
        //send event on start
        OnTypeChanged?.Invoke(neuronType);
    }
    protected virtual void Update()
    {
        debug_voltage = voltage;
        CheckThresholdVoltage();
        DecayVoltage();        
        DecayConnectionStrengths();
        RemoveWeakConnections();
        AddNewConnections();        
    }
    void ReceiveSignal(float singnalInput)
    {       

        float duration = Time.time - lastSignalReceivedTime;
        timeSinceLastSignal = duration;
        lastSignalReceivedTime = Time.time;
        lastSignalValue = singnalInput;
        //voltage += signal * settings.signalActivityIncreaseAmount;
        voltage += singnalInput;
        //debug
        debug_voltage = voltage;
        
        OnReceived?.Invoke(this);
        
    }   
    public void ForceFire(float value)
    {
        ReceiveSignal(value);
        //Bypasses ReceiveSignal(value) checks
        //activityLevel = value;
        //Fire();
        //activityLevel = 0;        
    }    
    private void CheckThresholdVoltage()
    { 
        if (neuronType == NeuronType.Excitory)
        {
            //this allows firing on either -1 or 1 activity
            if (voltage >= settings.firingThresold)
            {
                //if (GetCanFire())
                //{
                //    print($"Can fire excite was true. Test firing!{voltage}");
                //}
                //bool canFire = GetCanFire(voltage);

                Fire();                
                voltage = 0;
            }
        }
        if (neuronType == NeuronType.Inhibitory)
        {
            //In
            // in + or - adds to current voltage can fire on either threshold + or -,
            // fires out negative only on positive
            if (voltage >= settings.firingThresold)
            {
                // Fires on an incoming positive and outputs a negative
                if (voltage > 0) voltage *= -1;
                //print($"Fired Inhibitory{voltage}");
                Fire();
                voltage = 0;
            }
            //else a negative voltage will just get more negative and not fire but inhibit firing on self
        }

    }   
    private void DecayVoltage()
    {
        float decayAmount = 1 / settings.signalActivityDecayDuration * Time.deltaTime;

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
    protected void Fire()
    {
        foreach (var connection in connections)
        {
            // Ensure the connection strength entry exists
            if (!connectionStrengths.ContainsKey(connection))
            {                
                connectionStrengths[connection] = settings.connectionStrengthDefault; // Initialize connection strength
            }

            // Strengthen the connection
            connectionStrengths[connection] += settings.connectionStrengthenRate;

            if(connectionStrengths[connection] > settings.connectionStrengthMax)
            {
                connectionStrengths[connection] = settings.connectionStrengthMax;
            }
            if (connection == null)
            {
                return;
            }
            var dist = Vector3.Distance(transform.position, connection.transform.position);
            var delay = dist / settings.signalSpeed;
            
            float signalOut = Mathf.Clamp(voltage, -1.0f, 1.0f);

            //if(neuronType == NeuronType.Inhibitory)
            //{ 
            //    // make neg
            //    if (signalOut > 0) 
            //    { 
            //        signalOut *= -1;
            //    }
            //    print("Fired inhib " + signalOut.ToString("f02"));

            //}
            //else
            //{
            //    if (signalOut < 0)
            //    {
            //        signalOut *= -1;
            //    }
            //    print("Fired! Excit:" + signalOut.ToString("f02"));
            //}
            //print("Fired! Signal Out:" + signalOut.ToString("f02"));

            OnFired?.Invoke(this);

            // A delay for the receivng neuron to account for the transmission speed
            SendDelayedSignal(connection, signalOut, delay);
           

        }
    }    
    private void SendDelayedSignal(Neuron neuron,float signal, float delay)
    {
        //Restrict to directly being called
        IEnumerator SendSignalDelay(Neuron neuron, float signal, float delay)
        {
            yield return new WaitForSeconds(delay);
            neuron.ReceiveSignal(signal);
        }

        StartCoroutine(SendSignalDelay(neuron, signal, delay));
    }    
    private void RemoveWeakConnections()
    {        
        // Example: Prune weak connections
        List<Neuron> toRemove = new List<Neuron>();
        foreach (var connection in connections)
        {
            if (connectionStrengths.ContainsKey(connection) && connectionStrengths[connection] < settings.removeConnectionThreshold)
            {
                toRemove.Add(connection);
            }
            if (connection == null)
            {
                print("Removing Null Connection");
                toRemove.Add(connection);
            }
        }

        foreach (var neuron in toRemove)
        {
            print("Removing connection");
            connections.Remove(neuron);
            connectionStrengths.Remove(neuron);
        }
    }
    private void AddNewConnections()
    {
        // Example: Add new connections if below connection amount
        if (connections.Count < settings.maxConnections + additionalConnections) // Arbitrary max connections
        {

            Neuron randomNeuron = FindRandomNeuron();

            if (randomNeuron != null && !connections.Contains(randomNeuron))
            {
                print("Adding connection");

                connections.Add(randomNeuron);
                connectionStrengths[randomNeuron] = settings.connectionStrengthDefault;// Initialize connection strength
            }
        }
    }
    private void DecayConnectionStrengths()
    {
        foreach (var connection in connections)
        {
            if (connectionStrengths.ContainsKey(connection))
            {
                connectionStrengths[connection] -= settings.connectionWeakenRate * Time.deltaTime;
            }
        }
    }
    Neuron FindRandomNeuron()
    {
        //find neurons by radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, settings.connectionAddRadius);
        List<Neuron> allNeurons = new List<Neuron>();
        
        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            if (n) allNeurons.Add(n);
        }   

        if (allNeurons.Count > 1)
        {           
            Neuron randomNeuron = this;
            while (randomNeuron == this)
            {
                randomNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            }

            return randomNeuron;
        }
        return null;
    }
    
    
    internal void Invert()
    {
        if (neuronType == NeuronType.Excitory)
        {
            neuronType = NeuronType.Inhibitory;
            //OnPolarized?.Invoke(this);
            OnTypeChanged?.Invoke(neuronType);
        }
        else
        {
            neuronType = NeuronType.Excitory;
            //OnDepolarized?.Invoke(this);
            OnTypeChanged?.Invoke(neuronType);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        DrawConnectionRadius();
        // Visualize connections
        Gizmos.color = Color.red;
        foreach (var connection in connections)
        {
            Gizmos.DrawLine(transform.position, connection.transform.position);
        }
        Gizmos.color = Color.white;
    }
    protected virtual void DrawConnectionRadius()
    {        

        Gizmos.color = Color.green;        
        Gizmos.DrawWireSphere(transform.position, settings.connectionAddRadius);
        Gizmos.color = Color.white;
    }

}
