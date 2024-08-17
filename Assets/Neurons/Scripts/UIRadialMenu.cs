using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIRadialMenu : MonoBehaviour
{
    [SerializeField] private List<Image> _pieceImages;
    [SerializeField] private List<RectTransform> _pieceRects;
    [SerializeField] private GameObject _pieceSample;
    [SerializeField] private int _pieceCount;
    [SerializeField] private int _radius = 200;
    [SerializeField] private float currentDistance;
    private HoverableUI hoverable;
    public float distanceThreshold = 60;
    public int rotationOffsetDeg = 90;
    [Range(0,360)]public float arcDegrees = 360f;
    public float debug_autoRadius;
    public bool isMouseOver => hoverable.isMouseOver;
    public bool mouseIsInsideRadius => currentDistance < distanceThreshold;

    void Awake()
    {
        hoverable = gameObject.AddComponent<HoverableUI>();
        SpawnElements(_pieceSample, _pieceCount, _radius);
        Hide();
    }
    void Update()
    {
        
        CheckDistanceFromCenter();
        MovePiecePosition();
        
    }

    private void CheckDistanceFromCenter()
    {
        var rect = GetComponent<RectTransform>();
        currentDistance = Vector2.Distance(rect.position, Input.mousePosition);        
    }

    public void Show()
    {
        ShowGameObject();
    }
    public void Hide()
    {
        HideGameObject();       
    }
    private void MovePiecePosition()
    {
        for (int i = 0; i < _pieceRects.Count; i++)
        {
            var center = Vector3.zero;
            Vector3 position = GetCircularPosition(_pieceCount, _radius, i);
            _pieceRects[i].anchoredPosition = center + position;
        }

        //auto find correct threshold radius
        var rect = GetComponent<RectTransform>();
        debug_autoRadius = Vector2.Distance( rect.position, _pieceRects[0].position);
    } 
    private void SpawnElements(GameObject elementPrefab, int numberOfElements, float radius)
    {
        elementPrefab.SetActive(true);
        Vector2 center = Vector2.zero;
        // Loop to create each element
        for (int i = 0; i < numberOfElements; i++)
        {
            
            Vector2 position = GetCircularPosition(numberOfElements, radius, i);

            // Instantiate the UI element as a child of the specified parent
            GameObject newElement = Instantiate(elementPrefab, transform);

            // Set the position relative to the center point
            var rect = newElement.GetComponent<RectTransform>();
            rect.anchoredPosition = center + position;
            _pieceRects.Add(rect);
        }
        elementPrefab.SetActive(false);
    }
    private Vector3 GetCircularPosition(int numberOfElements, float radius, int i)
    {
        // Calculate the base angle for this element based on the arc angle
        float angle = i * Mathf.Deg2Rad * arcDegrees / (numberOfElements - 1);
        // Calculate the angle for this element
        //float angle = i * Mathf.PI * 2f / numberOfElements;
        
        //rotate angle 90 as radians to make 0 on top
        angle += Mathf.Deg2Rad * rotationOffsetDeg; //Mathf.PI/2f;
        
        // Calculate the position for this element
        Vector2 position = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        //uses negative on the x position to rotate in opposite direction
        return new Vector3(-position.x, position.y, 0);
    }   
    private void ShowGameObject()
    {
        gameObject.SetActive(true);
    }
    private void HideGameObject()
    {
        gameObject.SetActive(false);
    }

}

