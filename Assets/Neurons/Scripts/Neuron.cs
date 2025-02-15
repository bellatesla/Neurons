using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Neuron : MonoBehaviour
{
    public NeuronType neuronType = NeuronType.Excitory;
    public List<Neuron> connections = new List<Neuron>();
    public Dictionary<Neuron, float> connectionStrengths = new Dictionary<Neuron, float>();
    public float voltage { get; private set; }//-1,1
    public static System.Action<string> OnNeuronStateChange { get; internal set; }
          
    public int additionalConnections = 0;    
    public bool hasfireOnceOnStart;

    public event System.Action<Neuron> OnFired;
    public event System.Action<Neuron> OnReceived;    
    public event System.Action<NeuronType> OnTypeChanged;
   
    public float timeSinceLastSignal;
    float lastSignalReceivedTime;
    public float lastSignalValueIn;
    public float lastFiringTime;


    // Global settings
    public NeuronSettingsSO settings;   //manually assigned or is asssigned in prefab 
    // Global states
    private GlobalNeuronEvents globalStateManager;
    
    private bool signalBlocked;
    public static int connectionsCreated;
   
    
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
        //debug_voltage = voltage;
        CheckThresholdVoltage();
        DecayVoltage();        
        DecayConnectionStrengths();
        RemoveWeakConnections();
        AddNewConnections();        
    }


    void ReceiveSignal(float singnalInput)
    {
        if (signalBlocked)
        {
            return;
        }
        
        timeSinceLastSignal = Time.time - lastSignalReceivedTime;
        lastSignalReceivedTime = Time.time;
        lastSignalValueIn = singnalInput;
       
        voltage += singnalInput;       
        
        OnReceived?.Invoke(this);
        
    }   
    public void ForceFire(float value)
    {
        ReceiveSignal(value);             
    }  
    internal void StopFiring()
    {
        StartCoroutine(BlockSignalsOverTime(5));
    }
    

    private IEnumerator BlockSignalsOverTime(float duration)
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

   
    protected void Fire()
    {
        // Supress firing if we are within cooldown time
        if((Time.time - lastFiringTime ) < settings.firedCooldownDuration)
        {            
            voltage = 0;
            return;
        }


        foreach (var connection in connections)
        {
            if (connection == null)
            {
                return;
            }

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

          

            var dist = Vector3.Distance(transform.position, connection.transform.position);
            var delay = dist / settings.signalSpeed;
            
            float signalOut = Mathf.Clamp(voltage, -1.0f, 1.0f);            

            OnFired?.Invoke(this);

            // A delay for the receivng neuron to account for the transmission speed
            SendDelayedSignal(connection, signalOut, delay);            

        }
        // has fired, reset
        voltage = 0;
        lastFiringTime = Time.time;
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
            Neuron randomNeuron = FindRandomNeuron();//near by distance

            //1 of 10 should be a random multiple of the range
            if (connectionsCreated % 10 == 0)
            {
                FindDistantRandomNeuron(settings.connectionAddRadius * 5);
                print($"Added Distant Connection. Connections Total: {connectionsCreated} ");
            }

            if (randomNeuron != null && !connections.Contains(randomNeuron))
            {
                //print("Adding connection");
                connectionsCreated++;
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

    private Neuron FindDistantRandomNeuron(float radius) 
    {
        //find neurons by bigger radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<Neuron> allNeurons = new List<Neuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<Neuron>();
            //if we are distant and not null
            if (n && Vector3.Distance(transform.position, n.transform.position) > settings.connectionAddRadius)
            {
                allNeurons.Add(n);
            }
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

    private Neuron FindRandomNeuron()
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
