using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

public class ActionPatrol : FSMAction
{
    [Header("Config")]
    [SerializeField] private float speed;
    private Waypoint waypoint;
    private int PointIndex;
    private Vector3 nextPosition;
    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }
    public override void Act()
    {
        FallowPath();
    }
    private void FallowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetCurrentPosition(), speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, GetCurrentPosition()) < 0.1f)
        {
            UpdateNextPosition();
        }
    }
    private void UpdateNextPosition()
    {
        PointIndex++;
        if(PointIndex > waypoint.Points.Length-1)
        {
            PointIndex = 0;
        }
    } 
    private Vector3 GetCurrentPosition()
    {
        return waypoint.GetPosition(PointIndex);
    }
}
