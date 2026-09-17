using System;
using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 3f;
    public bool loopWaipoint = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting = false;

    void Start()
    {
        waypoints = new Transform[waypointParent.childCount];
       for(int i=0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    private void Update()
    {
        if(PauseController.IsGamePaused || isWaiting)
        {
            return;
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitWaypoint());

        }
    }
    IEnumerator WaitWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaipoint ? (currentWaypointIndex + 1) % waypoints.Length : Math.Max(currentWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}
