using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.Melee{
public class ActivateBlock : StateMachineBehaviour
{

    private MeleeActor _actor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_actor == null)
            _actor = animator.transform.root.GetComponent<MeleeActor>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor.Block();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _actor.Default();
    }
}
}