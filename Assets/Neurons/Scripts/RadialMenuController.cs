using Rito.RadialMenu_v3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuController : MonoBehaviour
{
    public RadialMenu radialMenu;
    public KeyCode key = KeyCode.G;

    //Menu Butttons
    public static Action OnFiredPositive;
    public static Action OnFiredNegative;
    public static Action OnInvert;
    public static Action OnRemoveConnection;

    public List<Button> buttons;

    [Space]
    public Sprite[] sprites;

    

    private void Start()
    {
        GlobalNeuronEvents.OnMouseOverNeuron += OnMouseOverNeuron;
        //GlobalNeuronEvents.OnMouseExitNeuron += OnMouseExitNeuron;
        
        radialMenu.SetPieceImageSprites(sprites);
        buttons = radialMenu.GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnMouseExitNeuron(Neuron obj)
    {
        radialMenu.Hide();
    }

    private void OnMouseOverNeuron(Neuron neuron)
    {
        radialMenu.Show();
        UpdateMenuPosition();
    }
    void UpdateMenuPosition()
    {
        var rect = radialMenu.GetComponent<RectTransform>();
        //......
    }
    private void Update()
    {
        // if mouse if over neuron show
        // get global mouse over neuron event
        if (Input.GetKeyDown(key))
        {
            radialMenu.Show();
        }
        else if (Input.GetKeyUp(key))
        {
            int selected = radialMenu.Hide();
            Debug.Log($"Selected : {selected}");
        }
    }
    private void OnButtonClicked()
    {
        // The button index sent its used to call an event
        int selected = radialMenu.selectedIndex;
        InvokeSelectedButton(selected);
        Debug.Log($"Selected : {selected}");
    }

    private void InvokeSelectedButton(int selected)
    {
        if (selected == 0)
        {
            OnFiredPositive?.Invoke();
        }
        else if (selected == 1)
        {
            OnFiredNegative?.Invoke();
        }
        else if (selected == 2)
        {
            OnInvert?.Invoke();
        }
        else if (selected == 3)
        {
            OnRemoveConnection?.Invoke();
        }
    }


}
