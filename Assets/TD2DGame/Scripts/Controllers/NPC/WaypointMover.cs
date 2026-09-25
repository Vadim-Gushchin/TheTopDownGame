using System;
using System.Collections;
using UnityEngine;
using static GlobalHelper;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 3f;
    public bool loopWaipoint = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            animator.SetBool(AnimatorConstants.IsWalking, false);
            return;
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat(AnimatorConstants.CurrentInputX, direction.x);
        animator.SetFloat(AnimatorConstants.CurrentInputY, direction.y);
        animator.SetBool(AnimatorConstants.IsWalking,direction.magnitude > 0);


        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitWaypoint());

        }
    }
    IEnumerator WaitWaypoint()
    {
        isWaiting = true;
        animator.SetBool(AnimatorConstants.IsWalking, false);
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaipoint ? (currentWaypointIndex + 1) % waypoints.Length : Math.Max(currentWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}
