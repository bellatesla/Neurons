using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If attached to a neuron this will show a information panel on info button
/// </summary>
[RequireComponent(typeof(Neuron))]
public class NeuronInfoPanel : MonoBehaviour
{
    public GameObject infoPanelPrefab;
    private GameObject infoPanel;
    private TMPro.TMP_Text voltageText;
   
    Neuron neuron;
    float smoothedVoltage;
    
    private void Awake()
    {
        neuron = GetComponent<Neuron>();        
    }
    private void Start()
    {
        Show();
    }

    private void OnEnable()
    {
        neuron.OnReceived += OnReceived;
        neuron.OnFired += OnFired;
    }

    private void OnFired(Neuron obj)
    {
        smoothedVoltage = neuron.voltage;
    }

    private void OnReceived(Neuron obj)
    {
        smoothedVoltage = neuron.voltage;
    }
    
    private void Update()
    {
        if (infoPanel && infoPanel.gameObject.activeSelf)
        {
            // Interpolate smoothActivity toward target value
            float t = 1.0f / neuron.settings.infoPanelTextSpeed * Time.deltaTime;
            smoothedVoltage = Mathf.Lerp(smoothedVoltage, neuron.voltage, t);
            
            voltageText.text = smoothedVoltage.ToString("F01") + "v"; ; 
            
            // Used for testing position
            infoPanel.transform.position = neuron.transform.position + neuron.settings.infoPanelOffset;
        }
    }


    public void Toggle()
    {
        if (infoPanel && infoPanel.gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }


    public void Show()
    {
        if (!infoPanel)
        {
            SpawnPanel();
        }
        else
        {
            infoPanel.SetActive(true);
        }
    }


    public void Hide()
    {
        infoPanel.SetActive(false);
    }


    private void SpawnPanel()
    {
        infoPanel = Instantiate(infoPanelPrefab);
        infoPanel.transform.SetParent(this.transform);

        infoPanel.transform.position = transform.position + neuron.settings.infoPanelOffset;        
        voltageText = infoPanel.GetComponentInChildren<TMPro.TMP_Text>();
    }

}
