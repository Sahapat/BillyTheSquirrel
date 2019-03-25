using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_LayerWeightSetter : StateMachineBehaviour
{
    [SerializeField] bool OnEnter = false;
    [SerializeField] int EnterTargetLayer = 0;
    [SerializeField, Range(0, 1)] float EnterTargerWeight = 0f;
    [SerializeField] bool OnExit = false;
    [SerializeField] int ExitTargetLayer = 0;
    [SerializeField, Range(0, 1)] float ExitTargerWeight = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnEnter)
        {
            animator.SetLayerWeight(EnterTargetLayer, EnterTargerWeight);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit)
        {
            animator.SetLayerWeight(ExitTargetLayer, ExitTargerWeight);
        }
    }
}
