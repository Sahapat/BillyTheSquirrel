using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_AddVelocity : StateMachineBehaviour
{
    [SerializeField]float velocityAdd = 120f;
    [SerializeField]ForceMode forceMode = ForceMode.Impulse;
    Rigidbody m_rigidbody = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_rigidbody)
        {
            m_rigidbody = animator.GetComponent<Rigidbody>();
        }
        m_rigidbody?.AddForce(animator.transform.forward*velocityAdd,forceMode);
    }
}
