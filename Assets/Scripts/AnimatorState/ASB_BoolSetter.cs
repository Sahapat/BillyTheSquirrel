using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_BoolSetter : StateMachineBehaviour
{
    [SerializeField]string targetBool = string.Empty;
    [SerializeField]bool onEnter = false;
    [SerializeField]bool EnterValue = false;
    [SerializeField]bool onExit = false;
    [SerializeField]bool ExitValue = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetBool ==string.Empty)return;
        if(onEnter)
        {
            animator.SetBool(targetBool,EnterValue);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetBool ==string.Empty)return;
        if(onExit)
        {
            animator.SetBool(targetBool,ExitValue);
        }
    }
}
