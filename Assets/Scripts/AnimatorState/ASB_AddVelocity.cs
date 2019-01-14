using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_AddVelocity : StateMachineBehaviour
{
    [SerializeField]Vector3 velocityAdd = Vector3.zero;
    [SerializeField]bool onEnter = false;
    [SerializeField]bool onExit = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(onEnter)
        {
            Rigidbody m_rigidbody = animator.GetComponent<Rigidbody>();
            m_rigidbody.AddForce(velocityAdd,ForceMode.Impulse);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(onExit)
        {
            Rigidbody m_rigidbody = animator.GetComponent<Rigidbody>();
            m_rigidbody.AddForce(velocityAdd,ForceMode.Impulse);
        }
    }
}
