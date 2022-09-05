using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class HealthManagerSample : MonoBehaviour
{
    [SerializeField] private float toHeal = 1f;
    [SerializeField] private float toDamage = 1f;
    
    private HealthManager health;

    private void Start()
    {
        health = GetComponent<HealthManager>();
    }

    public void DealDamage()
    {
        health.TakeDamage(toDamage);
    }

    public void Heal()
    {
        health.Heal(toHeal);
    }

    public void Ouch()
    {
        Debug.Log("Ouch!");
    }

    public void Death()
    {
        Debug.Log("Dead!");
    }

    public void Healed()
    {
        Debug.Log("Healed");
    }
}
