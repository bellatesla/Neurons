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
        var saveables = GetAllActiveSaveables();       
        
        SaveData saveData = new SaveData();
        
        // Add save data from all saveables
        foreach (var saveable in saveables)
        {
            saveable.SaveState(ref saveData);
        }
        
        string path = SaveToFile(saveData);
        print($"Saved data for {saveables.Count} objects to {path} ");
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
        SaveData saveData = LoadFromFile();
        LoadGame(saveData);
    }

    private static SaveData LoadFromFile()
    {
        string jsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + filename);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);
        return saveData;
    }

    void LoadGame(SaveData saveData)
    {                
        var saveables = GetAllActiveSaveables();
        // Dispose or Pre-load state
        foreach (var saveable in saveables)
        {
            saveable.Dispose();
        }
        saveables = new List<ISaveable>();
        // Load state    
        var uiSaveables = LoadUI(saveData);         
        var neuronSaveables = LoadNeurons(saveData);        
        saveables.AddRange(uiSaveables);
        saveables.AddRange(neuronSaveables);
        
        //Post-load state all saveables
        foreach (var saveable in saveables)
        {
            saveable.PostLoadState();
        }
        print($"Loaded {saveables.Count} saveables");
    }

    private List<ISaveable> LoadNeurons(SaveData saveData)
    {
        var saveables = new List<ISaveable>();
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
        return saveables;
    }
    private static List<ISaveable> LoadUI(SaveData saveData)
    {
        var saveables = GetSelectableUI();

        print($"Found {saveables.Count} Active UI elements");

        //might cause error - on test when object are inactive/active mismatch 
        // which might send mismatched data
        for (int i = 0; i < saveData.uiData.Count; i++)
        {
            // active elements
            if (saveables.Count > i)
            {
                //saveables.Add(uiElements[i]);
                saveables[i].LoadState(saveData, i);
            }

        }

        return saveables;
    }

    static List<ISaveable> GetAllActiveSaveables()
    {
        var saveables = new List<ISaveable>();
       
        saveables.AddRange(GetSelectableNeurons());        
        saveables.AddRange(GetSelectableUI());
        print($"Saveable Count{saveables.Count}");
        return saveables;
    }
    static List<ISaveable> GetSelectableNeurons()
    {
        var saveables = new List<ISaveable>();       

        // inactive saveables are not allowed
        FindObjectsSortMode sortMode = FindObjectsSortMode.None;
        var saveableUI = FindObjectsByType<SaveableNeuron>(sortMode);
        saveables.AddRange(saveableUI);
        
        return saveables;
    }
    static List<ISaveable> GetSelectableUI()
    {
        var saveables = new List<ISaveable>();       

        // inactive saveables are not allowed
        FindObjectsSortMode sortMode = FindObjectsSortMode.None;
        var saveableUI = FindObjectsByType<SaveableUI>(sortMode);
        saveables.AddRange(saveableUI);
        
        return saveables;
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