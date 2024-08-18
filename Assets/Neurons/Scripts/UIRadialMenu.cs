using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIRadialMenu : MonoBehaviour
{
   
    [SerializeField] private List<RectTransform> pieceRects;
    [SerializeField] private GameObject pieceSample;
    [SerializeField] private int pieceCount;
    [SerializeField] private int radius = 200;
    [SerializeField] private float currentDistance;
    
    public float distanceThreshold = 60;
    public int rotationOffsetDeg = 90;
    [Range(0,360)]public float arcDegrees = 360f;//if using semi circle
    public float debug_autoRadius;
   
    public bool mouseIsInsideRadius => currentDistance < distanceThreshold;

    void Start()
    {
        //SpawnElements(_pieceSample, _pieceCount, _radius);
        if (Application.isPlaying)
        {
            Hide();
        }
        
    }
    void Update()
    {   
        //move only required if you want adjust in playmode
        if (Application.isPlaying)
        {
            CheckDistanceFromCenter();
        }
        else MovePiecePosition();

    }
    private void CheckDistanceFromCenter()
    {
        var rect = GetComponent<RectTransform>();
        currentDistance = Vector2.Distance(rect.position, Input.mousePosition);        
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void MovePiecePosition()
    {
        for (int i = 0; i < pieceRects.Count; i++)
        {
            var center = Vector3.zero;
            Vector3 position = GetCircularPosition(pieceCount, radius, i);
            pieceRects[i].anchoredPosition = center + position;
        }
    }
    
    [ContextMenu("Delete Icons")]
    private void DeleteIcons()
    {
        foreach (var item in pieceRects)
        {
            if (Application.isPlaying)
            {
                Destroy(item.gameObject);
            }
            else
            {
                DestroyImmediate(item.gameObject);
            }

        }
        pieceRects = new List<RectTransform>();
    }
    
    [ContextMenu("Spawn Icons")]
    private void SpawnIcons()
    {
        DeleteIcons();
        SpawnElements(pieceSample, pieceCount, radius);
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
            pieceRects.Add(rect);
        }
        elementPrefab.SetActive(false);
    }
    private Vector3 GetCircularPosition(int numberOfElements, float radius, int i)
    {
        // Calculate the base angle for this element based on the arc angle
        float angle = i * Mathf.Deg2Rad * arcDegrees / (numberOfElements - 1);//semi-circle
        // Calculate the angle for this element
        //float angle = i * Mathf.PI * 2f / numberOfElements;//full circle
        
        //rotate angle 90 as radians to make 0 on top
        angle += Mathf.Deg2Rad * rotationOffsetDeg; //Mathf.PI/2f;
        
        // Calculate the position for this element
        Vector2 position = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
        //uses negative on the x position to rotate in opposite direction
        return new Vector3(-position.x, position.y, 0);
    }   
    

}

