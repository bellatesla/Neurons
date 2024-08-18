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
    private TMPro.TMP_Text textComponent;
    Neuron neuron;
    float smoothedVoltage;
    
    private void Awake()
    {
        neuron = GetComponent<Neuron>();        
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
            // Interpolate smoothActivity toward zero
            float t = 1 / neuron.settings.infoPaneltextSpeed * Time.deltaTime;
            smoothedVoltage = Mathf.Lerp(smoothedVoltage, 0, t);

            //smoothedVoltage = Mathf.Lerp(targetVoltage, 0, speed * Time.deltaTime);
            var text = smoothedVoltage.ToString("F01") +"v";
            textComponent.text = text;            
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
        infoPanel.transform.position = transform.position + neuron.settings.infoPanelOffset;        
        textComponent = infoPanel.GetComponentInChildren<TMPro.TMP_Text>();
    }
}
