using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_LayerWeightSetter : StateMachineBehaviour
{
    [SerializeField] int EnterTargetLayer = 0;
    [SerializeField, Range(0, 1)] float EnterTargerWeight = 0f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(EnterTargetLayer, EnterTargerWeight);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(EnterTargetLayer, EnterTargerWeight);
    }
}
