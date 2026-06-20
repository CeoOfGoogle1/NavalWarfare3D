using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float armor;
    public int team;
    public bool spotted;
    public bool engaged;
    public float visualRange;
    Turret[] turrets;
    Boyancy[] floatingPoints;
    Propulsion propulsion;
    Navigation navigation;
    

    void Start()
    {
        propulsion = GetComponent<Propulsion>();
        navigation = GetComponent<Navigation>();
        turrets = GetComponentsInChildren<Turret>();
        floatingPoints = GetComponentsInChildren<Boyancy>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Sink();
        }
    }

    public void Damage(float damage, float ap)
    {
        if (armor > ap) return;
        health -= damage;
    }

    public void Sink()
    {
        propulsion.enabled = false;
        navigation.enabled = false;

        foreach (var turret in turrets)
        {
            turret.enabled = false;
        }

        foreach (var point in floatingPoints)
        {
            point.enabled = false;
        }
    }
}
