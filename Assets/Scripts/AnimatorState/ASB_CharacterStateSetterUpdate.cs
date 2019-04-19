using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_CharacterStateSetterUpdate : StateMachineBehaviour
{
    [SerializeField]string targetIntName = string.Empty;
    [SerializeField]CharacterState state = CharacterState.NONE;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetIntName != string.Empty)
        {
            animator.SetInteger(targetIntName,(int)state);
        }
    }
}
