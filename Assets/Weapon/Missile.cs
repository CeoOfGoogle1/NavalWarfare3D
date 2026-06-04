using UnityEngine;
using UnityEngine.VFX;

public class Missile : MonoBehaviour
{
    [Header("Stats")]
    public float acceleration;
    public float fuel;
    public float turnRate;
    
    [Header("Aerodynamics")]
    public float dragCoefficient = 0.001f;
    public float liftCoefficient = 0.01f;

    [Header("Guidance")]
    public Transform target;
    public bool cruise;
    public float cruiseHeight;
    public float N;
    //Vector3 previousLOS;
    Projectile p;
    VisualEffect trail;

    void Start()
    {
        p = GetComponent<Projectile>();
        trail = GetComponent<VisualEffect>();
        //previousLOS = (target.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float airDensity = Atmosphere.GetAirDensity(transform.position.y);

        // Acceleration
        if (fuel > 0)
        {
            p.velocity += transform.forward * acceleration * dt;
            fuel -= dt;
        }
        else
        {
            trail.Stop();
        }

        // Drag
        Vector3 drag = p.velocity * p.velocity.magnitude * dragCoefficient * airDensity * dt;
        p.velocity -= drag;

        // Lift
        float liftFactor = liftCoefficient * airDensity * dt;
        p.velocity = Vector3.Lerp(p.velocity, transform.forward * p.velocity.magnitude, liftFactor);

        // Induced drag
        float turnAmount = Vector3.Angle(p.velocity, transform.forward) / 180f; // 0–1
        float turnDrag = turnAmount * turnAmount * dragCoefficient * airDensity * dt;
        p.velocity -= p.velocity.normalized * turnDrag * p.velocity.magnitude;

        Quaternion targetRotation = Quaternion.LookRotation(p.velocity.normalized);
        if (target != null)
        {
            //Vector3 LOSRate = (relativePosition - previousLOS) / dt;
            //previousLOS = relativePosition;

            Vector3 relativePosition = (target.position - transform.position).normalized;
            Vector3 relativeVelocity = target.GetComponent<Rigidbody>().linearVelocity - p.velocity;
            Vector3 AngularLOSRate = Vector3.Cross(relativePosition, relativeVelocity) / relativePosition.sqrMagnitude;
            Vector3 LOSRate = Vector3.Cross(AngularLOSRate, relativePosition);
            Vector3 desiredTurn = N * LOSRate;
            Vector3 desiredForward = (transform.forward + desiredTurn).normalized;
            targetRotation = Quaternion.LookRotation(desiredForward);
        }
        float speedTurnRate = turnRate * (1f + p.velocity.magnitude * 0.05f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speedTurnRate * dt);
    }
}
