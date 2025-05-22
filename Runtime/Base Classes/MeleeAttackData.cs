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
        [SerializeField] bool damageOverride;
        [SerializeField] int damage;
        [SerializeField] float damageStartTime, damageStopTime;
        [SerializeField] float pushScale;
        [SerializeField] int combo;
        [SerializeField] AnimationCurve orientSpeedCurve;
        [SerializeField] AnimationCurve moveSpeedCurve;
        [SerializeField] int topMoveSpeed;
        [SerializeField] AnimationCurve pushSpeedCurve;
        [SerializeField] int topPushSpeed;

        public AnimationCurve GetOrientSpeedCurve() => orientSpeedCurve;
        public AnimationCurve GetMoveSpeedCurve() => moveSpeedCurve;
        public int GetTopMoveSpeed() => topMoveSpeed;
        public AnimationCurve GetPushSpeedCurve() => pushSpeedCurve;
        public int GetTopPushSpeed() => topPushSpeed;

        public AnimationClip GetAnimationClip() => clip;
        public string GetMeleeWeapon() => meleeWeapon;
        public AttackType GetAttackType() => attackType;
        public bool GetDamageOverride() => damageOverride;
        public int GetDamage() => damage;
        public float GetDamageStartTime() => damageStartTime;
        public float GetDamageStopTime() => damageStopTime;
        public float GetPushScale() => pushScale;
        public int GetCombo() => combo;
    }
}
