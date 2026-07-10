using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool canBeMoved;
    public bool beingMoved;
    Renderer waypointRenderer;
    Collider waypointCollider;
    public Navigation ship;
    public int waypointID;

    void Start()
    {
        waypointRenderer = GetComponent<Renderer>();
        waypointCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!canBeMoved)
        {
            waypointRenderer.enabled = false;
            waypointCollider.enabled = false;
        }
        else
        {
            waypointRenderer.enabled = true;
            waypointCollider.enabled = true;

            if (!beingMoved) { return; }

            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                transform.position = hitPoint;

                ship.waypoints[waypointID] = hitPoint;
            }

            if (!Input.GetMouseButton(0)) { beingMoved = false; }
        }
    }

    void OnMouseOver()
    {
        if (!Input.GetMouseButton(0) || !canBeMoved) { return; }

        beingMoved = true;
    }
}
