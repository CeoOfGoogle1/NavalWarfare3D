using UnityEngine;
using UnityEngine.VFX;

public class Turret : MonoBehaviour
{
    public Transform target;
    public GameObject projectile;
    public Transform barrel;
    public Transform spawn;
    public GameObject firingEffect;
    public float firerate = 1;
    public float inaccuracy;
    public float magazineSize;
    public float reloadTime;
    public int tracerSpacing;
    int tracerSpace;
    float reloadTimer;
    float magazine;
    float nextFireTime = 0;
    public float pitch;
    public float yaw;
    public float traverseSpeed;
    public float targetPitch;
    public float targetYaw;
    public float maxPitch;
    public float minPitch;
    public float maxYaw;
    public float minYaw;
    public bool autoFire;
    public bool autoTarget;
    public int team;
    AimCalculator aimCalculator;
    Shell shell;
    Projectile p;
    FriendsAndEnemies iff;
    VisualEffect tracer;

    void Start()
    {
        aimCalculator = FindFirstObjectByType<AimCalculator>();
        shell = projectile.GetComponent<Shell>();
        p = projectile.GetComponent<Projectile>();
        iff = FindFirstObjectByType<FriendsAndEnemies>();
        tracer = projectile.GetComponent<VisualEffect>();
        tracer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) 
            || (autoFire = true && Mathf.Abs(pitch - targetPitch) < 1 && Mathf.Abs(yaw - targetYaw) < 1)
            && Time.time >= nextFireTime 
            && magazine > 0)
        {
            nextFireTime = Time.time + firerate;
            magazine--;
            if (tracerSpacing > 0)
            {
                tracerSpace++;
                if (tracerSpace >= tracerSpacing)
                {
                    tracer.enabled = true;
                    tracerSpace = 0;
                }
                else
                {
                    tracer.enabled = false;
                }
            }
            Shoot();
        }

        if (magazine <= 0)
        {
            reloadTimer += Time.deltaTime;

            if (reloadTimer >= reloadTime)
            {
                magazine = magazineSize;
                reloadTimer = 0;
            }
        }

        if (autoTarget)
        {
            foreach (var enemy in iff.spottedEnemies)
            {
                float horizontalMaxRange = shell.muzzleVelocity * shell.muzzleVelocity / Physics.gravity.y;
                if (Vector3.Distance(enemy.transform.position, transform.position) <= horizontalMaxRange)
                {
                    bool canReach = aimCalculator.CheckReachability(transform.position, enemy.transform.position, shell.muzzleVelocity);
                    if (canReach)
                    {
                        target = enemy.transform;
                    }
                }
            }
        }

        if (target != null)
        {
            Vector3 shooterVelocity = Vector3.zero;

            if (aimCalculator.CalculateAim(
                target.position,
                target.GetComponent<Rigidbody>().linearVelocity,
                spawn.position,
                shooterVelocity,
                shell.muzzleVelocity,
                out Vector3 aimDirection))
            {
                targetYaw = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
                targetPitch = Mathf.Asin(aimDirection.y) * Mathf.Rad2Deg;
                //Debug.Log(target.GetComponent<Rigidbody>().linearVelocity);
            }
            else
            {
                target = null;
                return; // No solution exists
            }
        }

        yaw = Mathf.MoveTowardsAngle(yaw, targetYaw, traverseSpeed * Time.deltaTime);
        pitch = Mathf.MoveTowardsAngle(pitch, targetPitch, traverseSpeed * Time.deltaTime);
        
        yaw = Mathf.Clamp(yaw, minYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        barrel.localRotation = Quaternion.Euler(-pitch, 0, 0);
    }

    void Shoot()
    {
        AudioManager.instance.Play("fire", transform);

        Quaternion correction = Quaternion.Euler(0, -90, 0);
        GameObject vfxInstance = Instantiate(firingEffect, spawn.position, spawn.rotation * correction);

        Quaternion offset = Quaternion.Euler(
            Random.Range(-inaccuracy, inaccuracy), 
            Random.Range(-inaccuracy, inaccuracy),
            Random.Range(-inaccuracy, inaccuracy));
        GameObject newProjectile = Instantiate(projectile, spawn.position, spawn.rotation * offset);

        Destroy(vfxInstance, 10f);
    }
}
