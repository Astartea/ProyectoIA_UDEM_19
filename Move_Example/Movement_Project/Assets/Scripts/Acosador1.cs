using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Acosador1 : MonoBehaviour
{
    AIDestinationSetter destination;

    public Transform[] waypoints;

    public int indexWaypoints = 0;

    public float currentDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        destination = GetComponent<AIDestinationSetter>();
        destination.target = waypoints[indexWaypoints];
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = Vector3.Distance(transform.position, waypoints[indexWaypoints].position);
        if(currentDistance < 1f)
        {
            indexWaypoints = (indexWaypoints + 1) % waypoints.Length;
            destination.target = waypoints[indexWaypoints];
        }
    }
}
