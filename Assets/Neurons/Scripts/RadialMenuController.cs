using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{    
    public UIRadialMenu radialMenu;
    public KeyCode testkey = KeyCode.G;

    //Menu Butttons
    public static Action<Neuron> OnFiredPositive;
    public static Action<Neuron> OnFiredNegative;
    public static Action<Neuron> OnInvert;
    public static Action<Neuron> OnRemoveConnection;

    public List<Button> buttons;
   
    [Space]
    public Sprite[] sprites;
    protected Neuron lastOnMouseOverNeuron;//the last neuron that the mouse was over
    private Neuron currentOnMouseOverNeuron;

    private void Start()
    {
        GlobalNeuronEvents.OnMouseOverNeuron += OnMouseOverNeuron;
        //GlobalNeuronEvents.OnMouseExitNeuron += OnMouseExitNeuron;

        //radialMenu.SetPieceImageSprites(sprites);
        buttons = radialMenu.GetComponentsInChildren<Button>().ToList();
       

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;//needed for lambda closure
            buttons[i].onClick.AddListener(() => ButtonClicked(index));
        }

        //sprites
        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite icon = sprites[i];
            buttons[i].GetComponent<Image>().sprite = icon;
        }
    }
   
    //private void Update()
    //{
    //    if (lastOnMouseOverNeuron != null)
    //    {
    //        UpdateMenuPosition();
    //    }  
    //}
    private void ButtonClicked(int index)
    {
        print("Button returned: " + index);
        InvokeSelectedButton(index);
    }
    private void OnMouseExitNeuron(Neuron obj)
    {    
        //radialMenu.Hide();
    }
    private void OnMouseOverNeuron(Neuron neuron)
    {
        radialMenu.Show();
        //currentOnMouseOverNeuron = neuron;

        if (radialMenu.isInsideRadius && lastOnMouseOverNeuron) 
        {           
            //print("Mouse is inside menu");            
        }
        else
        {            
            lastOnMouseOverNeuron = neuron;
        }

        UpdateMenuPosition();
    } 
    private void UpdateMenuPosition()
    {
        if (lastOnMouseOverNeuron == null) return;

        var rect = radialMenu.GetComponent<RectTransform>();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(lastOnMouseOverNeuron.transform.position);
        screenPos.z = 0;       
        rect.position = screenPos;
    }   
    private void InvokeSelectedButton(int selected)
    {
        if (selected == 0)
        {
            OnFiredPositive?.Invoke(lastOnMouseOverNeuron);
        }
        else if (selected == 1)
        {
            OnFiredNegative?.Invoke(lastOnMouseOverNeuron);
        }
        else if (selected == 2)
        {
            OnInvert?.Invoke(lastOnMouseOverNeuron);
        }
        else if (selected == 3)
        {
            OnRemoveConnection?.Invoke(lastOnMouseOverNeuron);
        }
    }

}
