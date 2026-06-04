using UnityEngine;

public class Shell : MonoBehaviour
{
    public float muzzleVelocity;
    Projectile p;

    void Start()
    {
        p = GetComponent<Projectile>();
        p.velocity += transform.forward * muzzleVelocity;
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(p.velocity);
    }
}
