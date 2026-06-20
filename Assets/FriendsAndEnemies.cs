using System.Collections.Generic;
using UnityEngine;

public class FriendsAndEnemies : MonoBehaviour
{
    public int team;
    public List<Health> enemies;
    public List<Health> friends;
    public List<Health> spottedEnemies;
    int previousUnitCount;
    
    void Start()
    {
        
    }

    void Update()
    {
        Health[] unitsArray = FindObjectsByType<Health>(FindObjectsSortMode.None);
        List<Health> unitsList = new List<Health>(unitsArray);
        if (unitsList.Count != previousUnitCount)
        {
            foreach (var unit in unitsList)
            {
                if (unit.team != team)
                {
                    enemies.Add(unit);
                }
                else
                {
                    friends.Add(unit);
                }
            }
            previousUnitCount = unitsList.Count;
        }

        foreach (var enemy in enemies)
        {
            bool spotted = false;

            foreach (var friend in friends)
            {
                
                if (Vector3.Distance(friend.transform.position, enemy.transform.position) <= friend.visualRange)
                {
                    spotted = true;
                    break;
                }
            }

            enemy.spotted = spotted;

            if (spotted)
            {
                if (!spottedEnemies.Contains(enemy))
                {
                    spottedEnemies.Add(enemy);

                    enemy.GetComponent<Renderer>().enabled = true;
                    foreach (Renderer r in enemy.GetComponentsInChildren<Renderer>())
                    {
                        r.enabled = true;
                    }
                }
            }
            else
            {
                spottedEnemies.Remove(enemy);
                
                enemy.GetComponent<Renderer>().enabled = false;
                foreach (Renderer r in enemy.GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }
            }
        }
    }
}
