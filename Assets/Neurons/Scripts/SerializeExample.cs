using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SerializeExample : MonoBehaviour
{
    public Container container = new Container();

    void Start()
    {
        // Adding derived class instances to the list
        container.items.Add(new NeuronSaveData { commonProperty = "Base A", specificPropertyA = "Specific Property A" });
        container.items.Add(new UITextdata { commonProperty = "Base B", specificPropertyB = 5.5f });

        // Serialize the container with ReferenceLoopHandling.Ignore
        string json = JsonConvert.SerializeObject(container, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        Debug.Log("Serialized JSON: " + json);

        //// Deserialize it back
        Container deserializedContainer = JsonConvert.DeserializeObject<Container>(json);

        foreach (var item in deserializedContainer.items)
        {
            Debug.Log("Common Property: " + item.commonProperty);

            if (item is NeuronSaveData derivedA)
                Debug.Log("Specific Property A: " + derivedA.specificPropertyA);
            else if (item is UITextdata derivedB)
                Debug.Log("Specific Property B: " + derivedB.specificPropertyB);
        }
    }
}
public class Container
{
    public List<SaveableTypeBase> items = new List<SaveableTypeBase>();
}
