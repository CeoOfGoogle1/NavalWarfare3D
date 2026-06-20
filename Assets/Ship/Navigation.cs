using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 target;
    public List<Vector3> waypoints;
    public float waypointArrivalDistance;
    [SerializeField] PIDController headingPID;
    [SerializeField] PIDController throttlePID;
    public Vector3 targetDirection;
    public float headingError;
    float currentHeading;
    float targetHeading;
    public float forcedHeading;
    public float targetDistance;
    Propulsion ship;
    Rigidbody rb;

    void Start()
    {
        ship = GetComponent<Propulsion>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        if (targetTransform != null) target = targetTransform.position;
        if (target == null)
        {
            ship.rudderInput = 0;
            ship.throttle = 0;
            return;
        }

        if (waypoints != null)
        {
            target = waypoints.First();
            if (waypointArrivalDistance > Vector3.Distance(transform.position, target))
            {
                waypoints.Remove(waypoints.First());
            } 
        }

        targetDirection = target - transform.position;
        currentHeading = transform.eulerAngles.y;
        targetHeading = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
        if (forcedHeading != 0)
        {
            targetHeading = forcedHeading;
        }
        ship.rudderInput = -headingPID.Update(dt, currentHeading, targetHeading, true);

        targetDistance = Vector3.Distance(transform.position, target);
        ship.throttle = -throttlePID.Update(dt, targetDistance, 0);
    }
}
