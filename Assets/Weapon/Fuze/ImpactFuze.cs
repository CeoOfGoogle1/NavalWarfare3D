using UnityEngine;

public class ImpactFuze : MonoBehaviour
{
    public float delay;
    public float ricochetAngle = 20;
    Vector3 newPosition;
    Projectile p;

    void Start()
    {
        p = GetComponent<Projectile>();
    }

    void FixedUpdate()
    {
        newPosition = transform.position + p.velocity * Time.fixedDeltaTime;

        if (Physics.Linecast(transform.position, newPosition, out RaycastHit hit))
        {
            float impactAngle = Vector3.Angle(-p.velocity.normalized, hit.normal);
            if (impactAngle < ricochetAngle)
            {
                p.velocity = Vector3.Reflect(p.velocity.normalized, hit.normal) * p.velocity.magnitude;
                transform.position = hit.point + p.velocity.normalized;
            }
            else
            {
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                    return;
                };

                if (hit.transform.GetComponent<Health>() != null)
                {
                    Health health = hit.transform.GetComponent<Health>();
                    health.Damage(p.power, p.ap);
                }

                Destroy(gameObject);
            }
            return;
        }
    }
}
