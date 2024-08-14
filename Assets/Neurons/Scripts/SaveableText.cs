using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveableText : MonoBehaviour,ISaveable
{
    public TMPro.TMP_InputField uiInputText;

    void Awake()
    {
        uiInputText = GetComponent<TMP_InputField>();
    }
    void Update()
    {

    }
    public void Dispose()
    {
        //??
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void LoadState(SaveSystem.SaveData gameState, int index)
    {
       //uiInputText.text = gameState.otherTypeData[index];
    }

    public void PostLoadState()
    {
        
    }

    public void SaveState(ref SaveSystem.SaveData savedata)
    {
        //savedata.otherTypeData.Add(uiInputText.text);
    }

    
}
