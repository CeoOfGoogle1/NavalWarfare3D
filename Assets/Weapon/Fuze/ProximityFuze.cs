using UnityEngine;

public class ProximityFuze : MonoBehaviour
{
    public float distance;
    public float armingTime;

    void Start()
    {
        
    }

    void Update()
    {
        if (armingTime > 0)
        {
            armingTime -= Time.deltaTime;
            return;
        }

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, distance, transform.forward, out hit))
        {
            Destroy(gameObject);
        }
    }
}
