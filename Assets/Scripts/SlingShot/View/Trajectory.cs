using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int numPoints = 50;
    public float timeBetweenPoints = 0.1f;
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Vector2 gravity;

    private List<GameObject> points = new List<GameObject>();

    void Start()
    {
        gravity = Physics2D.gravity;
    }

    public void DrawTrajectory(Vector2 initialPosition, Vector2 initialVelocity)
    {
        ClearPoints();
        for (int i = 0; i < numPoints; i++)
        {
            float time = i * timeBetweenPoints;
            Vector2 position = initialPosition + initialVelocity * time + 0.5f * gravity * time * time;
            GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
            points.Add(point);
        }
    }

    public void ClearPoints()
    {
        foreach (GameObject point in points)
        {
            Destroy(point);
        }
        points.Clear();
    }
}
