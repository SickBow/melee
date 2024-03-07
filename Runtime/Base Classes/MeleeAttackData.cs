using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace Sickbow.Melee
{
    [Serializable]
    public class MeleeAttackData
    {
        [SerializeField] AnimationClip clip;
        [SerializeField] string meleeWeapon;
        [SerializeField] AttackType attackType;
        [SerializeField] float damageStartTime, damageStopTime;
        [SerializeField] float pushScale;
        
        public AnimationClip GetAnimationClip() => clip;
        public string GetMeleeWeapon() => meleeWeapon;
        public AttackType GetAttackType() => attackType;
        public float GetDamageStartTime() => damageStartTime;
        public float GetDamageStopTime() => damageStopTime;
        public float GetPushScale() => pushScale;
    }
}
