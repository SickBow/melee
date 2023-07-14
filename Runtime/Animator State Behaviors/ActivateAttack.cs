using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee{
public class ActivateAttack : StateMachineBehaviour
{
    [SerializeField] AttackType attackType;
    [SerializeField, Range(0,1)] float damageStartTime, damageStopTime;
    private MeleeActor _actor;

    public float GetDamageStartTime() => damageStartTime;
    public float GetDamageStopTime() => damageStopTime;
    public AttackType GetAttackType() => attackType;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_actor == null)
            _actor = animator.transform.root.GetComponent<MeleeActor>();

        if ((damageStartTime == 0 && damageStopTime == 0) || damageStopTime < damageStartTime){
            damageStartTime = .3f; //default
            damageStopTime = .7f; //default
        }
        _actor.InitializeAttack(attackType);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float normalizedTime = stateInfo.normalizedTime % 1;
        if (InActiveTimeRange(normalizedTime))
            _actor.Attack();
    }

    private bool InActiveTimeRange(float normalizedTime){
        return ((normalizedTime >= damageStartTime) && (normalizedTime <= damageStopTime));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor.Default();
    }
}
}
