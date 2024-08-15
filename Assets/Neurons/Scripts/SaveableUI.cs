using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveableUI : Saveable
{
    [Header("Auto finds the following types")]
    public TMP_Text tmp_text;//save/load this text

    void Awake()
    {        
        tmp_text = GetComponent<TMP_Text>();     
    }
    private int GetID() => gameObject.GetInstanceID();

    public override void SaveState(ref SaveSystem.DataContainer savedData)
    {
        UITextdata newData = new UITextdata
        {
            id = GetID(),
            text = tmp_text.text, // Save the UI text, adjust as needed
            //position = transform.position
        };

        savedData.objectSaves.Add(newData);
    }

    public override void LoadState(SaveableTypeBase state)
    {
        UITextdata data = (UITextdata)state;
        
        // Ensure we're loading the correct data
        if (data.id != GetID())//this might not work as intended if 
        {
            Debug.LogWarning("ID mismatch when loading UI state.");
            return;
        }
        //transform.position = data.position;
        tmp_text.text = data.text; // Load the UI text, adjust as needed
    }

    public override void PostLoadState() { /* UI elements may not need post-load logic */ }

    public override void Dispose()
    {
        // UI elements are not destroyed, so this might not be needed.        
    }
}

