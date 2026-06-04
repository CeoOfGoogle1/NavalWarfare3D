using UnityEngine;

public class Boyancy : MonoBehaviour
{
    public WaterHeightSampler water;
    public float boyancyForce;
    public float waterDrag;
    public float waterAngularDrag;
    public float depthBeforeSubmersion;
    public int floaterCount = 1;
    public int inertiaTensor;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();

        rb.inertiaTensor *= inertiaTensor;
    }

    void FixedUpdate()
    {
        float waterHeight = water.GetWaterHeight(transform.position);

        float forcePerFloater = rb.mass * Physics.gravity.magnitude / floaterCount;

        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float depth = waterHeight - transform.position.y;
        if (depth > 0)
        {
            float displacementMultiplier = Mathf.Clamp01(depth / depthBeforeSubmersion) * boyancyForce;
            rb.AddForceAtPosition(Vector3.up * forcePerFloater * displacementMultiplier, transform.position);
            rb.AddForceAtPosition(displacementMultiplier * -rb.GetPointVelocity(transform.position) * waterDrag * Time.fixedDeltaTime, transform.position);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime);
        }
    }
}
