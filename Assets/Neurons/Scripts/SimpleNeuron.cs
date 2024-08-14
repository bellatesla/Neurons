using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNeuron : MonoBehaviour
{
    
    public float activityLevel = 0;//-1,1  
    public float searchRadius = 1;

    public List<SimpleNeuron> connections_IN = new List<SimpleNeuron>();
    public List<SimpleNeuron> connections_OUT = new List<SimpleNeuron>();
    public Dictionary<SimpleNeuron, float> connectionStrengths = new Dictionary<SimpleNeuron, float>();
    [SerializeField] NeuronSettingsSO settings;

    public event System.Action<SimpleNeuron> OnFired;
    public event System.Action<SimpleNeuron> OnReceviedSignal;

    private void Start()
    {
        SetInitialConnectionStrengths();
    }

    internal void ReceiveSignal(SimpleNeuron sender, float signal)
    {        
        activityLevel += signal;
        OnReceviedSignal?.Invoke(this);

        print($"{this.name} received {signal.ToString("f02")} signal from {sender.name}");
    }
   

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0)) ReceiveSignal(this,1);
        if (Input.GetMouseButton(1)) ReceiveSignal(this,-1);
    }   

    private void Update()
    {
        activityLevel = Mathf.Clamp(activityLevel, -1.01f, 1.01f);

        if (activityLevel >= 1)
        {            
            Fire();           
        }
        if (activityLevel <= -1)
        {            
            Fire();            
        }
    }

    void Fire()
    {
        // Send signal to every out connection
        
        //Debug.Log($"Signal Send to {connections_OUT.Count}, from: {this.name}");
        foreach (var connection in connections_OUT)
        {
            var dist = Vector3.Distance(transform.position, connection.transform.position);
            var delay = dist / settings.signalSpeed;
            ReceiveSignal(connection, activityLevel);
        }
        
        //invoke event
        OnFired?.Invoke(this);
        //reset activity
        activityLevel = 0;
    }
    
    void SetInitialConnectionStrengths()
    {
        // Output neurons
        foreach (var connection in connections_OUT)
        {
            connectionStrengths.Add(connection,settings.connectionStrengthDefault);
        }

        // Input neurons - hookup my input to the others output
        foreach (var connection in connections_IN)
        {
            connection.connections_OUT.Add(this);
            //connection.connectionStrengths.Add(this, settings.connectionStrengthDefault);
        }
    }

    private void RemoveConnections()
    {
        // Prune weak connections
        List<SimpleNeuron> toRemove = new List<SimpleNeuron>();
        foreach (var connection in connections_OUT)
        {
            if (connectionStrengths.ContainsKey(connection) && connectionStrengths[connection] < .001f)
            {
                toRemove.Add(connection);
            }
        }

        foreach (var neuron in toRemove)
        {
            print("Removing connection");
            connections_OUT.Remove(neuron);
            connectionStrengths.Remove(neuron);
        }
    }
    private void AddNewConnections()
    {
        SimpleNeuron randomNeuron = FindRandomNeuron(searchRadius);

        if (randomNeuron != null && !connections_OUT.Contains(randomNeuron))
        {
            print("Adding connection");
            connections_OUT.Add(randomNeuron);
            connectionStrengths[randomNeuron] = .5f;// Initialize connection strength
        }
    }

    private void DecayConnectionStrengths(List<SimpleNeuron> connections)
    {
        foreach (var connection in connections)
        {
            if (connectionStrengths.ContainsKey(connection))
            {
                connectionStrengths[connection] -= .01f * Time.deltaTime;
            }
        }
    }

    SimpleNeuron FindRandomNeuron(float radius)
    {
        //find neurons by radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<SimpleNeuron> allNeurons = new List<SimpleNeuron>();

        foreach (var collider in colliders)
        {
            var n = collider.GetComponent<SimpleNeuron>();
            if (n) allNeurons.Add(n);
        }

        if (allNeurons.Count > 1)
        {
            SimpleNeuron randomNeuron = this;
            while (randomNeuron == this)
            {
                randomNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            }

            return randomNeuron;
        }
        return null;
    }
}
