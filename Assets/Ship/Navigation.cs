using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform target;
    Propulsion ship;
    Rigidbody rb;

    void Start()
    {
        ship = GetComponent<Propulsion>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, targetDir.normalized, Vector3.up);
        if (Mathf.Abs(angle) > 5)
        {
            float turn = Mathf.Clamp(angle / 45, 1, -1);
            ship.rudderInput = Mathf.Lerp(ship.rudderInput, turn, Time.deltaTime * 2f);
        }
        else
        {
            ship.rudderInput = 0f;
        }

        float alignment = Vector3.Dot(targetDir.normalized, transform.forward);
        ship.throttle = Mathf.Clamp01(alignment);
        //Debug.Log(angle);
    }
}
