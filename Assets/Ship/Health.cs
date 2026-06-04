using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float armor;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(float damage, float ap)
    {
        if (armor > ap) return;
        health -= damage;
    }
}
