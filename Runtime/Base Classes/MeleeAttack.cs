using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sickbow.Melee
{
    [Serializable]
    public class MeleeAttack
    {
        public MeleeAttack(MeleeActor sender, MeleeWeapon weapon, AttackType attackType, AnimatorStateInfo stateInfo)
        {
            this.sender = sender;
            this.weapon = weapon;
            this.attackType = attackType;
            this.stateInfo = stateInfo;
        }

        public int GetDamage()
        {
            return Mathf.RoundToInt(weapon.GetDamage() * attackTypeScalars[attackType]);
        }

        public bool HitAlreadyRegistered(GameObject gameObject) => gameObjectsHit.Contains(gameObject);
        public void RegisterHit(GameObject gameObject) => gameObjectsHit.Add(gameObject);
        public static void SetGlobalAttackTypeDamageScale(AttackType attackType, float damageScale) => attackTypeScalars[attackType] = damageScale;
        public MeleeActor sender;
        public MeleeWeapon weapon;
        public AttackType attackType;
        public AnimatorStateInfo stateInfo;
        public List<GameObject> gameObjectsHit = new List<GameObject>();

        static Dictionary<AttackType, float> attackTypeScalars = new Dictionary<AttackType, float>(){
        {AttackType.Light, 1f},
        {AttackType.Heavy, 1.5f},
        {AttackType.Unblockable, 2f},
        {AttackType.Critical, 2.5f},
        {AttackType.Launcher, 1f}
    };
    }
}
