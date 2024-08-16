using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 날짜 : 2021-04-26 PM 3:12:50
// 작성자 : Rito

namespace Rito.RadialMenu_v3.Test
{

    //Global radial UI states
    public class Test_RadialMenu : MonoBehaviour
    {
        public RadialMenu radialMenu;
        public KeyCode key = KeyCode.G;

        //output for behavior
        public UnityEvent OnFiredPositive;
        public UnityEvent OnFiredNegative;
        public UnityEvent OnInvert;
        public UnityEvent OnRemoveConnection;
        
        public List<Button> buttons;

        [Space]
        public Sprite[] sprites;

        private void Start()
        {
            radialMenu.SetPieceImageSprites(sprites);
            buttons = radialMenu.GetComponentsInChildren<Button>().ToList();            

            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];                
                button.onClick.AddListener(OnButtonClicked);
            }           
        }

        private void OnButtonClicked()
        {
            // The button index sent its used to call an event
            int selected = radialMenu.selectedIndex;
            CallUnityEvent(selected);           
            Debug.Log($"Selected : {selected}");           
        }

        private void CallUnityEvent(int selected)
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
        private void Update()
        {
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
    }
}