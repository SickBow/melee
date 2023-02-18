using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sickbow.Melee;

public class Health : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int MAX_HEALTH;

    void Awake() => health = MAX_HEALTH;
    void OnEnable() {
        GetComponent<MeleeActor>().TookHit += ProcessMeleeHit;
    }
    public int GetHealth() => health;
    
    public void ProcessMeleeHit(MeleeHitInfo meleeHitInfo){
        TakeDamage(meleeHitInfo.damage);
    }

    public void TakeDamage(int value){
        health = Mathf.Max (health - Mathf.Abs(value), 0);
    }
    public void Heal(int value){
        health = Mathf.Min(health + Mathf.Abs(value), MAX_HEALTH );
    }
}
