using UnityEngine;

public class Propulsion : MonoBehaviour
{
    public WaterHeightSampler water;
    public Transform propeller;
    public float engineForce;
    public float throttle;
    public float rudderStrength;
    public float rudderInput;
    public float forwardDrag;
    public float sidewaysDrag;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //rb.centerOfMass = new Vector3(0, -1f, 0);
    }

    void FixedUpdate()
    {
        float waterHeight = water.GetWaterHeight(transform.position);
        if (propeller.transform.position.y > waterHeight) { return; }

        float efficiency = 1f / (1 + rb.GetPointVelocity(propeller.position).magnitude * 0.1f);
        Vector3 thrust = transform.forward * throttle * engineForce * efficiency;
        rb.AddForceAtPosition(thrust, propeller.position);

        // Turning
        float speed = Vector3.Dot(rb.GetPointVelocity(propeller.position), transform.forward);
        float turnForce = speed * rudderInput * rudderStrength;
        rb.AddForceAtPosition(transform.right * turnForce, propeller.position);
        //rb.AddTorque(Vector3.up * turnForce);

        // Drag
        Vector3 localVelocity = transform.InverseTransformDirection(rb.GetPointVelocity(propeller.position));
        float forward = localVelocity.z;
        float sideways = localVelocity.x;
        Vector3 drag = -transform.forward * forward * Mathf.Abs(forward) * forwardDrag + 
        -transform.right * sideways * Mathf.Abs(sideways) * sidewaysDrag;
        rb.AddForce(drag);
    }
}
