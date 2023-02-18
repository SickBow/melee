using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee{
public class MeleeAttack
{
    public MeleeAttack(MeleeActor sender, MeleeWeapon weapon, AttackType attackType){
        this.sender = sender;
        this.weapon = weapon;
        this.attackType = attackType;
    }

    public int GetDamage(){
        return Mathf.RoundToInt(weapon.GetDamage() * attackTypeScalars[attackType]);
    }

    public bool HitAlreadyRegistered(GameObject gameObject) => gameObjectsHit.Contains(gameObject);
    public void RegisterHit(GameObject gameObject) => gameObjectsHit.Add(gameObject);
    public void SetGlobalAttackTypeDamageScale(AttackType attackType, float damageScale) => attackTypeScalars[attackType] = damageScale;    
    public MeleeActor sender;
    public MeleeWeapon weapon;
    public AttackType attackType;
    public List<GameObject> gameObjectsHit = new List<GameObject>();

    static Dictionary<AttackType,float> attackTypeScalars = new Dictionary<AttackType, float>(){ 
        {AttackType.Light, 1f},
        {AttackType.Heavy, 1.5f},
        {AttackType.Unblockable, 2f}
    };
}
}