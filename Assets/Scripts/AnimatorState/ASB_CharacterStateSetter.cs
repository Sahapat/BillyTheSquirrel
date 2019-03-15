using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_CharacterStateSetter : StateMachineBehaviour
{
    [SerializeField]string targetIntName = string.Empty;
    [SerializeField]CharacterState state;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetIntName != string.Empty)
        {
            animator.SetInteger(targetIntName,(int)state);
        }
    }

}
