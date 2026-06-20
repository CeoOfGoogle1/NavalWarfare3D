using System.Collections.Generic;
using UnityEngine;

public class MissileSeeker : MonoBehaviour
{
    public float fieldOfView;
    public float rangeOfView;
    public int team;
    FriendsAndEnemies iff;

    void Start()
    {
        iff = FindFirstObjectByType<FriendsAndEnemies>();
    }

    void Update()
    {
        foreach (var enemy in iff.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) > rangeOfView)
            {
                continue;
            }

            Vector3 enemyDirection = enemy.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, enemyDirection.normalized);
            if (angle <= fieldOfView * 0.5f)
            {
                GetComponent<Missile>().target = enemy.transform;
            }
        }
    }
}
