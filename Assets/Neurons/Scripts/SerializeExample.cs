using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using JsonSubTypes;
using UnityEngine;

public class SerializeExample : MonoBehaviour
{
    public Container container = new Container();
    public GameObject testObject;
    public Transform testTransform;
    void Start()
    {
        // Adding derived class instances to the list
        container.items.Add(new DerivedClassA { commonProperty = "Base A", transformPos = testTransform.position }) ;
        container.items.Add(new DerivedClassB { commonProperty = "Base B", specificPropertyB = testObject.name });

        // Serialize the container with ReferenceLoopHandling.Ignore
        string json = JsonConvert.SerializeObject(container, Formatting.Indented,
           new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore ,});
        //string jsonData = JsonUtility.ToJson(saveData, true);//does not handle derived data
        string path = Application.persistentDataPath + "/" + "Serialize Test.json";
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Saved to path: " + path);

        // Deserialize it back
        
        string jsonData = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + "Serialize Test.json");
        //DataContainer saveData = JsonConvert.DeserializeObject<DataContainer>(jsonData);
        Container deserializedContainer = JsonConvert.DeserializeObject<Container>(jsonData);

        foreach (var item in deserializedContainer.items)
        {
            Debug.Log("Common Property: " + item.commonProperty);

            if (item is DerivedClassA derivedA)
                Debug.Log("Specific Property A: " + derivedA.specificPropertyA);
            else if (item is DerivedClassB derivedB)
                Debug.Log("Specific Property B: " + derivedB.specificPropertyB);
        }
    }
}
public class Container
{
    public List<BaseClass> items = new List<BaseClass>();
}

[JsonConverter(typeof(JsonSubtypes), "Type")]
[JsonSubtypes.KnownSubType(typeof(DerivedClassA), "A")]
[JsonSubtypes.KnownSubType(typeof(DerivedClassB), "B")]
public class BaseClass
{
    public string commonProperty;
    public string Type;
    public Vector3 position;  // Example of a Unity type

}

public class DerivedClassA : BaseClass
{
    public string specificPropertyA;
    public Vector3 transformPos;
    public DerivedClassA()
    {
        Type = "A";
    }
}

public class DerivedClassB : BaseClass
{
    public string specificPropertyB;
    // not working public GameObject gameObject;
    public DerivedClassB()
    {
        Type = "B";
    }
}

