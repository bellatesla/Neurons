using UnityEngine;
using UnityEngine.EventSystems;
using System;


//Add to a UI canvas element to detect mouse over
public class HoverableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnMouseEnterUIElement;
    public event Action OnMouseExitUIElement;

    public bool isMouseOver { get; private set; }
    public bool debug_isMouseOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        OnMouseEnterUIElement?.Invoke();
        debug_isMouseOver = isMouseOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        OnMouseExitUIElement?.Invoke();
        debug_isMouseOver = isMouseOver;
    }
}
