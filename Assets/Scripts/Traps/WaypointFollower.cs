using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    private int currentWaypointIndex = 0;
    private enum Axis { X, Y }

    [SerializeField] private GameObject[] waypoints; //Use GameObject for empty game object

    [SerializeField] private float speed = 2f;
    [SerializeField] private bool isMovingSaw = false;
    [SerializeField] Axis axes;

    // Update is called once per frame
    private void Update()
    {
        if (isMovingSaw)
        {
            speed = 1f;

            switch (axes)
            {
                case Axis.X:
                    MoveSawOnXAxis();
                    break;

                case Axis.Y:
                    MoveSawOnYAxis();
                    break;

                default:
                    Debug.Log("Axis selection issue select X or Y");
                    break;
            }
            
        }
        else
        {
            MoveTowardsWaypoint();
        }
    }

    private void MoveTowardsWaypoint()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    private void MoveSawOnYAxis()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, waypoints[currentWaypointIndex].transform.position.y)) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, waypoints[currentWaypointIndex].transform.position.y), Time.deltaTime * speed);
    }

    private void MoveSawOnXAxis()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(waypoints[currentWaypointIndex].transform.position.x, transform.position.y)) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(waypoints[currentWaypointIndex].transform.position.x, transform.position.y), Time.deltaTime * speed);
    }
}
