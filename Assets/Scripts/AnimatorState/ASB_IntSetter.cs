using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_IntSetter : StateMachineBehaviour
{
    [SerializeField]string targetInt = string.Empty;
    [SerializeField]int IntValue = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetInt != string.Empty)
        {
            animator.SetInteger(targetInt,IntValue);
        }
    }
}
