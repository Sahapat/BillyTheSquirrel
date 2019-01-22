using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_RootMotionSetter : StateMachineBehaviour
{
    [SerializeField]bool rootMotionStatus = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.applyRootMotion = rootMotionStatus;
    }
}
