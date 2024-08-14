using System.Collections.Generic;
using UnityEngine;

public class EyeSensor : MonoBehaviour
{
    public RenderTexture renderTexture;
   
    private Texture2D texture2D;
    public List<float> pixelBrightness;
    public List<Neuron> neurons;
    //public List<Neuron> inputNeurons;
    public GameObject neuronPrefab;
    public float positionOffset=1.5f;
    void Start()
    {
        renderTexture.width = 3;
        renderTexture.height = 3;
        var count = renderTexture.width * renderTexture.height;
        pixelBrightness = new List<float>();
        for (int i = 0; i < count ; i++)
        {
            pixelBrightness.Add(0);
        }

        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        CreateNeuronGrid(renderTexture.width, renderTexture.height);

    }

    void CreateNeuronGrid(float w,float h)
    {
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Vector3 position = new Vector3(j-positionOffset, i, 0);
               
                GameObject go = Instantiate(neuronPrefab);
                go.transform.SetParent(transform);
                go.transform.localScale = .25f * Vector3.one;
                go.transform.localPosition = position;
                neurons.Add(go.GetComponent<Neuron>());
                //var colorFX = go.GetComponent<NeuronColor>();
                //colorFX.autoUpdate = false;
            }
        }
    }

    void Update()
    {
        CaptureRenderTexture();
        ProcessPixelData();
    }

    void CaptureRenderTexture()
    {
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;
    }

    void ProcessPixelData()
    {
        Color[] pixels = texture2D.GetPixels(); 

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            float brightness = pixel.grayscale; // Simplified as brightness
            pixelBrightness[i] = brightness;

            neurons[i].GetComponent<Renderer>().material.color = pixel;
            neurons[i].debug_voltage += brightness;
            //if (brightness > 0.5f)
            //{
            //    excitatorySum += brightness;
            //}
            //else
            //{
            //    inhibitorySum += 1.0f - brightness;
            //}
        }

        //excitatoryNeuron.SendSignal(excitatorySum / pixels.Length);
        //inhibitoryNeuron.SendSignal(inhibitorySum / pixels.Length);
    }
}

