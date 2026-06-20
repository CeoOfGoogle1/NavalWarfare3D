using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public float power;
    public float ap;
    public int team;

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        velocity += Physics.gravity * dt;

        ap *= velocity.magnitude;

        transform.position += velocity * dt;
    }
}
