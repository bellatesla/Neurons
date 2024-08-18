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
    public static Action<Neuron> OnToggleInfoPanel;
    private List<Button> buttons;
   
    [Space]
    //public Sprite[] sprites;//assign manually in editor
    protected Neuron lastOnMouseOverNeuron;//the last neuron that the mouse was over   
    protected Neuron currentOnMouseOverNeuron;//the last neuron that the mouse was over   
    public bool alwaysShow;

    private void Start()
    {
        GlobalNeuronEvents.OnMouseOverNeuron += OnMouseOverNeuron;
        GlobalNeuronEvents.OnMouseExitNeuron += OnMouseExitNeuron;
        // get buttons in radial menu
        buttons = radialMenu.GetComponentsInChildren<Button>().ToList();
       
        // set button listeners
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;//needed for lambda closure
            buttons[i].onClick.AddListener(() => ButtonClicked(index));
        }

        //// set sprites
        //for (int i = 0; i < sprites.Length; i++)
        //{
        //    Sprite icon = sprites[i];
        //    buttons[i].GetComponent<Image>().sprite = icon;
        //}
    }

    private void OnMouseExitNeuron(Neuron obj)
    {
        currentOnMouseOverNeuron = null;
    }

    private void Update()
    {
        //if (alwaysShow) return;
        if (!radialMenu.mouseIsInsideRadius && !currentOnMouseOverNeuron)
        {
            radialMenu.Hide();            
        }
    }
    private void ButtonClicked(int index)
    {
        print("Button returned: " + index);
        InvokeSelectedButton(index);
    }   
    private void OnMouseOverNeuron(Neuron neuron)
    {
        currentOnMouseOverNeuron = neuron;

        if (radialMenu.mouseIsInsideRadius && lastOnMouseOverNeuron) 
        {           
            //print("Mouse is inside menu");            
        }
        else
        {            
            lastOnMouseOverNeuron = neuron;
        }
        radialMenu.Show();
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
    
    /// <summary>
    /// Selected index to proper button event
    /// </summary>
    /// <param name="selected"></param>
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
        else if (selected == 4)
        {
            OnToggleInfoPanel?.Invoke(lastOnMouseOverNeuron);
        }
    }

}
