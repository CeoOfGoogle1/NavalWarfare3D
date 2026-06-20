using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fleet : MonoBehaviour
{
    public List<Transform> fleet;
    public int LeaderID;
    public Transform flagship;
    public int spacing;
    public int wedgeWidthMultiplier;
    public int gridColumns;
    public int spiralTurns;
    public Vector2[] slots;
    Dictionary<Transform, int> assignments = new Dictionary<Transform, int>();
    int previousFleet = 0;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2[] GetCircleSlots(int count, float spacing) 
        {
            var slots = new Vector2[count];
            float radius = spacing * 1.6f;
            for (int i = 0; i < count; i++) 
            {
                float angle = Mathf.PI * 2f * i / count - Mathf.PI / 2f;
                slots[i] = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            }
            return slots;
        }

        Vector2[] GetWedgeSlots(int count, float spacing) 
        {
            float hSpacing = spacing * 1.0f;
            float vSpacing = spacing;
            var slots = new List<Vector2>();
            int placed = 0, rank = 1;
            while (placed < count) 
            {
                int perRow = rank == 0 ? 1 : rank * 2;
                for (int c = 0; c < perRow && placed < count; c++) 
                {
                    float x = rank == 0 ? 0f : (c - (perRow - 1) / 2f) * hSpacing;
                    slots.Add(new Vector2(x, -rank * vSpacing));
                    placed++;
                }
                rank++;
            }
            return slots.ToArray();
        }

        Vector2[] GetGridSlots(int count, float spacing) 
        {
            int cols = 4;
            var slots = new Vector2[count];
            for (int i = 0; i < count; i++) 
            {
                int c = i % cols;
                int r = i / cols;
                int totalCols = Mathf.Min(cols, count);
                float x = (c - (totalCols - 1) / 2f) * spacing;
                float y = r * spacing;
                slots[i] = new Vector2(x, y);
            }
            return slots;
        }

        Vector2[] GetLineSlots(int count, float spacing)
        {
            var slots = new Vector2[count];
            for (int i = 0; i < count; i++) 
            {
                slots[i] = new Vector2(0f, -i * spacing);
            }
            return slots;
        }
        
        Vector3 GetWorldSlotPosition(int slotIndex) 
        {
            Vector2 local = slots[slotIndex];
            Vector3 localOffset = new Vector3(local.x, 0f, local.y);
            return flagship.position + localOffset;
        }

        slots = GetCircleSlots(fleet.Count, spacing);
        //slots = GetWedgeSlots(fleet.Count, spacing);
        //slots = GetGridSlots(fleet.Count, spacing);
        //slots = GetLineSlots(fleet.Count, spacing);
        bool[] occupied = new bool[slots.Length];

        int GetClosestSlot(Vector3 shipWorldPosition)
        {
            int best = -1;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < slots.Length; i++)
            {
                if(occupied[i]) continue;
                float distance = Vector3.Distance(shipWorldPosition, GetWorldSlotPosition(i));
                if (distance < bestDistance) { bestDistance = distance; best = i; }
            }
            occupied[best] = true;
            return best;
        }

        if (previousFleet != fleet.Count)
        {
            assignments.Clear();

            float minSpeed = flagship.GetComponent<Propulsion>().engineForce;
            foreach(Transform ship in fleet)
            {
                int slot = GetClosestSlot(ship.position);
                assignments[ship] = slot;

                if (minSpeed > ship.GetComponent<Propulsion>().engineForce)
                {
                    minSpeed = ship.GetComponent<Propulsion>().engineForce;
                }
                ship.GetComponent<Propulsion>().engineForce = minSpeed;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                GameObject obj = new GameObject("MyObject");
                obj.transform.position = GetWorldSlotPosition(i);
            }
            previousFleet = fleet.Count;
        }

        foreach(Transform ship in fleet)
        {
            int slot = assignments[ship];
            float arrivalDistance = 50;
            float minDistance = 100;

            Vector3 slotPosition = GetWorldSlotPosition(slot);
            ship.GetComponent<Navigation>().target = slotPosition;
            float slotDistance = Vector3.Distance(ship.position, slotPosition);
            if (slotDistance < arrivalDistance)
            {
                ship.GetComponent<Navigation>().target = ship.position;
                ship.GetComponent<Navigation>().forcedHeading = flagship.eulerAngles.y;
            }
            else
            {
                ship.GetComponent<Navigation>().forcedHeading = 0;
            }

            foreach(Transform otherShip in fleet)
            {
                float distance = Vector3.Distance(ship.position, otherShip.position);
                if (distance < minDistance && distance > 0)
                {
                    Vector3 shipDest = ship.GetComponent<Navigation>().target;
                    float shipDistance = Vector3.Distance(ship.position, shipDest);
                    Vector3 otherDest = otherShip.GetComponent<Navigation>().target;
                    float otherDistance = Vector3.Distance(otherShip.position, otherDest);
                    if (shipDistance < otherDistance)
                    {
                        ship.GetComponent<Navigation>().target = ship.position;
                    }
                    else
                    {
                        otherShip.GetComponent<Navigation>().target = otherShip.position;
                    }
                }
            }
        }
    }
}
