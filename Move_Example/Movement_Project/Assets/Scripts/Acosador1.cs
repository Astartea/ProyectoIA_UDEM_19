using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acosador1 : BaseEnemy
{
    [Header("Atributos exclusivos de Acosador1")]
    public Transform[] waypoints;

    public int indexWaypoints = 0;

    public float currentDistance = 0;

    public override void InitEnemy()
    {
        base.InitEnemy();
        destination.target = waypoints[indexWaypoints];
    }

    public override void BehaviourEnemy()
    {
        //Comportamiento básico o esencial
        base.BehaviourEnemy();

        if (curState == StateEnemy.Walk)
        {
            //Comportamiento exclusivo del Acosador1
            currentDistance = Vector3.Distance(transform.position, waypoints[indexWaypoints].position);
            if (currentDistance < 1f)
            {
                indexWaypoints = (indexWaypoints + 1) % waypoints.Length;
                destination.target = waypoints[indexWaypoints];
            }
        }
    }
}
