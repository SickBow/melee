using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sickbow.Melee
{
    [Serializable]
    public class MeleeAttack
    {
        public MeleeAttack(MeleeActor sender, MeleeWeapon weapon, AttackType attackType, AnimatorStateInfo stateInfo, float damageStart, float damageEnd, float pushScale, MeleeAttackData attackData)
        {
            this.sender = sender;
            this.weapon = weapon;
            this.attackType = attackType;
            this.stateInfo = stateInfo;
            this.damageStartNormalized = damageStart;
            this.damageEndNormalized = damageEnd;
            this.pushScale = pushScale;
            this.attackData = attackData;
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
        public MeleeAttackData attackData;
        public AnimatorStateInfo stateInfo;
        public float damageStartNormalized, damageEndNormalized;
        public float pushScale;
        public List<GameObject> gameObjectsHit = new List<GameObject>();

        static Dictionary<AttackType, float> attackTypeScalars = new Dictionary<AttackType, float>(){
        {AttackType.Light, 1f},
        {AttackType.Heavy, 1.5f},
        {AttackType.Unblockable, 2f},
        {AttackType.Critical, 2.5f},
        {AttackType.Launcher, 1f},
        {AttackType.WeaponThrow, 1f},
        {AttackType.Charge, 1.5f}
    };
    }
}
