using UnityEngine;

public class WaypointPlacer : MonoBehaviour
{
    public bool placementEnabled;
    public float height;
    public Navigation ship;
    public GameObject waypointPrefab;

    void Update()
    {
        if (!placementEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, height, 0)); // y = 0 plane

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                ship.waypoints.Add(hitPoint);
                //Instantiate(waypointPrefab, hitPoint, transform.rotation).GetComponent<Waypoint>().ship = ship;
                GameObject waypoint = Instantiate(waypointPrefab, hitPoint, transform.rotation);
                waypoint.GetComponent<Waypoint>().ship = ship;
                waypoint.GetComponent<Waypoint>().waypointID = ship.waypoints.IndexOf(hitPoint);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    return;
                }
                else
                {
                    placementEnabled = false;
                }
            }
        }
    }
}
