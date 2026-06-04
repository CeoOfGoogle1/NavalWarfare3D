using System.Collections.Generic;
using UnityEngine;

public class Fleet : MonoBehaviour
{
    public List<Transform> fleet;
    //public List<Vector2> slots;
    public int LeaderID;
    public Transform flagship;
    public int spacing;
    public int wedgeWidthMultiplier;
    public int gridColumns;
    public int spiralTurns;
    public Vector2[] slots;

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
            int placed = 0, rank = 0;
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

        Vector2[] GetLineAheadSlots(int count, float spacing) 
        {
            var slots = new Vector2[count];
            for (int i = 0; i < count; i++) 
            {
                slots[i] = new Vector2(0f, -i * spacing);
            }
            return slots;
        }

        Vector2[] GetSpiralSlots(int count, float spacing) 
        {
            var slots = new Vector2[count];
            for (int i = 0; i < count; i++) 
            {
                float t = (i + 1f) / count;
                float angle = 6f * Mathf.PI * 2f * t - Mathf.PI / 2f;
                float radius = spacing * (i + 1f) * 0.5f;
                slots[i] = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
                // Note: scale radius to world units as needed.
                // In the playground this is auto-scaled to fit the canvas.
            }
            return slots;
        }
        
        // Converts each to world space using the flagship's rotation
        Vector3 GetRelativeSlotPosition(int slotIndex) 
        {
            Vector2 local = slots[slotIndex];
            return flagship.position
            + flagship.right   * local.x   // sideways
            + flagship.forward * local.y;  // forward/back
        }

        int GetClosestSlot(Vector3 shipWorldPosition)
        {
            int best = 0;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < slots.Length; i++)
            {
                float distance = Vector3.Distance(shipWorldPosition, GetRelativeSlotPosition(i));
                if (distance < bestDistance) { bestDistance = distance; best = i; }
            }
            return best;
        }
    }
}
