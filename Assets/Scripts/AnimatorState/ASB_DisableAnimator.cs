using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_DisableAnimator : StateMachineBehaviour
{
    [SerializeField]float waitTime = 0.3f;

    private float CounterTime = 0f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CounterTime = Time.time + waitTime;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(CounterTime <= Time.time)
        {
            animator.enabled = false;
        }
    }
}
