using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveableUI : MonoBehaviour, ISaveable
{
   
    public TMP_Text tmp_text;//save/load this text
    public TMP_InputField inputField;
    void Awake()
    {        
        tmp_text = GetComponent<TMP_Text>();///Winner! for normal text       
    }
    private int GetID() => gameObject.GetInstanceID();

    public void SaveState(ref SaveSystem.SaveData savedData)
    {
        UISaveData newData = new UISaveData
        {
            id = GetID(),
            uiText = tmp_text.text, // Save the UI text, adjust as needed
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };

        savedData.uiData.Add(newData);
    }

    public void LoadState(SaveSystem.SaveData state, int index)
    {
        UISaveData data = state.uiData[index];
        
        // Ensure we're loading the correct data
        if (data.id != GetID())//this might not work as intended if 
        {
            Debug.LogWarning("ID mismatch when loading UI state.");
            return;
        }
        //transform.position = data.position;
        tmp_text.text = data.uiText; // Load the UI text, adjust as needed
    }

    public void PostLoadState() { /* UI elements may not need post-load logic */ }

    public void Dispose()
    {
        // UI elements are not destroyed, so this might not be needed.
    }
}

