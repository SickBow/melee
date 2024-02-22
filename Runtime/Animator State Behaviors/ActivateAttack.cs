using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee{
public class ActivateAttack : StateMachineBehaviour
{
    [SerializeField] MeleeAttackDatas attackDatas;
    [SerializeField] AttackType attackType;
    [SerializeField, Range(0,1)] float damageStartTime, damageStopTime;
    [SerializeField] string meleeWeapon;
    private MeleeActor _actor;
    private MeleeAttack _attack;
    
    public float GetDamageStartTime() => damageStartTime;
    public float GetDamageStopTime() => damageStopTime;
    public AttackType GetAttackType() => attackType;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var clips = (animator.IsInTransition(layerIndex)) ? animator.GetNextAnimatorClipInfo(layerIndex) : animator.GetCurrentAnimatorClipInfo(layerIndex);
        MeleeAttackData matchingAttackData = null;
        if (clips.Length > 0) {
            AnimationClip stateClip = clips[0].clip;
            matchingAttackData = attackDatas.data.Find(data => data.GetAnimationClip() == stateClip);
        }

        if (matchingAttackData != null)
        {
            attackType = matchingAttackData.GetAttackType();
            damageStartTime = matchingAttackData.GetDamageStartTime();
            damageStopTime = matchingAttackData.GetDamageStopTime();
            meleeWeapon = matchingAttackData.GetMeleeWeapon();
        }
        
        if (_actor == null)
            _actor = animator.transform.root.GetComponent<MeleeActor>();

        if ((damageStartTime == 0 && damageStopTime == 0) || damageStopTime < damageStartTime){
            damageStartTime = .3f; //default
            damageStopTime = .7f; //default
        }
        _actor.InitializeAttack(attackType, stateInfo, damageStartTime, damageStopTime, meleeWeapon);
        _attack = _actor.GetActiveAttack();
        _actor.AttackStart?.Invoke(_attack);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _attack.stateInfo = stateInfo;
        float normalizedTime = stateInfo.normalizedTime % 1;
        if (InActiveTimeRange(normalizedTime) && _actor.GetActiveAttack() == _attack)
            _actor.Attack();
    }

    private bool InActiveTimeRange(float normalizedTime){
        return ((normalizedTime >= damageStartTime) && (normalizedTime <= damageStopTime));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor.Default();
        _actor.AttackEnd?.Invoke(_attack);
    }
}
}
