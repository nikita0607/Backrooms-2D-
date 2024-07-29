using System.Collections;
using System.Collections.Generic;
using BHSCamp;
using UnityEngine;

public class DeathRouitine : MonoBehaviour
{
    private Health _health;
    
    void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += Die;
    }

    private void Die()
    {
    }
}
