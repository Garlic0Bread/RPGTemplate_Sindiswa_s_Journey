using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float pauseDuration = 2f;

    private int currentWaypointIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = true;

    private void Start()
    {

        SetTargetPosition();
    }

    private void Update()
    {

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    private void SetTargetPosition()
    {
        targetPosition = waypoints[currentWaypointIndex].position;
    }

    private void MoveTowardsTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(PauseAtWaypoint());
        }
    }

    private IEnumerator PauseAtWaypoint()
    {
        isMoving = false;
        yield return new WaitForSeconds(pauseDuration);
        isMoving = true;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetTargetPosition();
    }
}
