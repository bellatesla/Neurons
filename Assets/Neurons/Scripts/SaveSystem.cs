using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using JsonSubTypes;

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
        
        DataContainer saveData = new DataContainer();
        
        // Add save data from all saveables
        foreach (var saveable in saveables)
        {
            saveable.SaveState(ref saveData);
        }
        //print($"Found data for {saveData.objectSaves.Count} ");
        string path = SaveToFile(saveData);
        print($"Saved data for {saveables.Count} objects to {path} ");
    }    

    [ContextMenu("Load Game State")]
    public void Load()
    {
        IDToObjectDictionary = new Dictionary<int, GameObject>();
        DataContainer saveData = LoadFromFile();
        StartCoroutine(LoadGame(saveData));
    }
    private static string SaveToFile(DataContainer saveData)
    {
        // Save to file
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented,
           new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //string jsonData = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/" + filename;
        System.IO.File.WriteAllText(path, json);
        return path;
    }
    private static DataContainer LoadFromFile()
    {
        string jsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + filename);       
        DataContainer saveData = JsonConvert.DeserializeObject<DataContainer>(jsonData);
        return saveData;
    }

    IEnumerator LoadGame(DataContainer saveData)
    {                
        var saveables = GetAllActiveSaveables();
        
        // Dispose or Pre-load state
        foreach (var saveable in saveables)
        {
            saveable.Dispose();
        }

        //After dispose we wait a few frames
        //to allow the destroy methods to finish.
        yield return null;
        yield return null;

        //Find saveables that are still active
        saveables = GetAllActiveSaveables();//get ui
        print($"Found {saveables.Count} elements still in scene");

        // Load UI
        for (int i = 0; i < saveables.Count; i++)
        {
            SaveableTypeBase item = saveData.objectSaves[i];
            if (item is UITextdata uiTextData)
            {     
                saveables[i].LoadState(uiTextData);
            }
        }

        // Load Neurons       
        for (int i = 0; i < saveData.objectSaves.Count; i++)
        {
            SaveableTypeBase item = saveData.objectSaves[i];
            
            //load common properties
            Debug.Log("Common Property: " + item.commonProperty);
            if (item is NeuronSaveData neuronData)
            {
                //specific properties
                Debug.Log("Specific Property A: " + neuronData.prefabName);
                GameObject newObject = Spawner.SpawnObject(neuronData);
                Saveable saveable = newObject.GetComponent<Saveable>();

                //Add object id for look up in dictionary
                SaveSystem.IDToObjectDictionary.Add(neuronData.id, newObject);

                saveable.LoadState(neuronData);
                saveables.Add(saveable);
            }                
        }

        print("Wait for spawned objects initialization");
        yield return null;
        
        //Post-load state all saveables
        foreach (var saveable in saveables)
        {
            saveable.PostLoadState();
        }

        print($"Loaded Complete: {saveables.Count} saveables");
    }
    static List<Saveable> GetAllActiveSaveables()
    {
        var saveables = new List<Saveable>();       

        // inactive saveables are not allowed        
        bool includeInactive = false;
        var saveable = FindObjectsOfType<Saveable>(includeInactive);
        saveables.AddRange(saveable);
        
        return saveables;
    }     
    
    [System.Serializable]
    public class DataContainer
    {
        public List<SaveableTypeBase> objectSaves;       

        public DataContainer()
        {            
            objectSaves = new List<SaveableTypeBase>();
        }
    }

}


[System.Serializable]
[JsonConverter(typeof(JsonSubtypes), "Type")]
[JsonSubtypes.KnownSubType(typeof(NeuronSaveData), "A")]
[JsonSubtypes.KnownSubType(typeof(UITextdata), "B")]
public class SaveableTypeBase
{

    public string commonProperty = "Common to all derived";
    public string Type;  // This property will help to distinguish between different derived types
    // All properties that are common to all saves
    //id
    public int id;
    //position
    public float x;
    public float y;
    public float z;
    //scale
    public float scaleX;
    public float scaleY;
    public float scaleZ;
    //rotation
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    [JsonIgnore]
    public Quaternion rotation { get => new Quaternion(rotX, rotY, rotZ, rotW); set { rotX = value.x; rotY = value.y; rotZ = value.z; rotW = value.w; } }
    [JsonIgnore]
    public Vector3 scale { get => new Vector3(scaleX, scaleY, scaleZ); set { scaleX = value.x; scaleY = value.y; scaleZ = value.z; } }
    [JsonIgnore]
    public Vector3 position { get => new Vector3(x, y, z); set { x = value.x; y = value.y; z = value.z; } }
}

[System.Serializable]
public class UITextdata : SaveableTypeBase
{
    public float specificPropertyB;
    public string text;
    
    public UITextdata()
    {
        Type = "B";
    }
}


[System.Serializable]
public class NeuronSaveData : SaveableTypeBase
{
    public string specificPropertyA;    
    public NeuronType neuronType;
    public List<int> connectionIds;
    public string prefabName;

    public NeuronSaveData()
    {
        Type = "A";
    } 
}