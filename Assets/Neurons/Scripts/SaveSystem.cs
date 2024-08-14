using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SaveSpawner))]
public class SaveSystem : MonoBehaviour
{
    private const string filename = "GameSavefile.json";
    public static Dictionary<int, GameObject> IDToObjectDictionary;    
    private SaveSpawner _spawner;   
    SaveSpawner Spawner
    {
        get
        {
            if (!_spawner)
                _spawner = GetComponent<SaveSpawner>();

            return _spawner;
        }
    }
    
    [ContextMenu("Save Game State")]
    public void SaveGame()
    {
        //var saveables = FindAllInterfaces<ISaveable>();
        var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList();

        SaveData saveData = new SaveData();
        
        // Add save data from all saveables
        foreach (var saveable in saveables)
        {
            saveable.SaveState(ref saveData);
        }
        
        string path = SaveToFile(saveData);
        print($"Saved {saveables.Count} saveables to {path} ");
    }

    private static string SaveToFile(SaveData saveData)
    {
        // Save to file
        string jsonData = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/"+ filename;
        System.IO.File.WriteAllText(path, jsonData);
        return path;
    }


    [ContextMenu("Load Game State")]
    void Load()
    {
        IDToObjectDictionary = new Dictionary<int, GameObject>();
        SaveData saveData = GetSavedData();
        LoadGame(saveData);
    }

    private static SaveData GetSavedData()
    {
        string jsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + filename);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);
        return saveData;
    }

    void LoadGame(SaveData saveData)
    {        
        //var saveables = FindAllInterfaces<ISaveable>();
        var saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToList();
        
        // Dispose or Pre-load state
        foreach (var saveable in saveables)
        {
            saveable.Dispose();
        }
        saveables = new List<ISaveable>();
        
        // Load state    
        LoadUI(saveables, saveData);         
        LoadNeurons(saveables, saveData);
       
        //Post-load state all saveables
        foreach (var saveable in saveables)
        {
            saveable.PostLoadState();
        }
    }

    private void LoadNeurons(List<ISaveable> saveables, SaveData saveData)
    {
        for (int i = 0; i < saveData.neuronData.Count; i++)
        {
            GameObject newObject = Spawner.SpawnObject(saveData.neuronData[i]);
            //Addobject
            SaveSystem.IDToObjectDictionary.Add(saveData.neuronData[i].id, newObject);
            ISaveable saveable = newObject.GetComponent<ISaveable>();
            //add the new saveable to list
            saveables.Add(saveable);
            saveable.LoadState(saveData, i);
        }
    }

    private static void LoadUI(List<ISaveable> saveables, SaveData saveData)
    {
        List<SaveableUI> uiElements = FindObjectsOfType<SaveableUI>().ToList();
        print($"Found {uiElements.Count} ui remaining");
        
        for (int i = 0; i < saveData.uiData.Count; i++)
        {
            saveables.Add(uiElements[i]);
            uiElements[i].LoadState(saveData, i);
        }
    }

    public List<T> FindAllInterfaces<T>() where T : class
    {
        List<T> results = new List<T>();
        MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();

        foreach (var monoBehaviour in allMonoBehaviours)
        {
            T component = monoBehaviour as T;
            if (component != null)
            {
                results.Add(component);
            }
        }

        return results;
    }
       
    [System.Serializable]
    public class SaveData
    {
        public List<NeuronSaveData> neuronData;
        public List<UISaveData> uiData;

        public SaveData()
        {
            neuronData = new List<NeuronSaveData>();
            uiData = new List<UISaveData>();
        }
    }    

}
[System.Serializable]
public class UISaveData
{
    public int id;
    public string uiText;
    public float x;
    public float y;
    public float z;

    public Vector3 position => new Vector3(x, y, z);
}


[System.Serializable]
public class NeuronSaveData
{
    public int id;
    public float x;
    public float y;
    public float z;
    public float scale_x;
    public float scale_y;
    public float scale_z;
    public NeuronType neuronType;
    public List<int> connectionIds;
    public string prefabName;

    public Vector3 position => new Vector3(x, y, z);
    public Vector3 scale => new Vector3(scale_x, scale_y, scale_z);

}