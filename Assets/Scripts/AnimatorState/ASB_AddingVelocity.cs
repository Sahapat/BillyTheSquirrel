using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASB_AddingVelocity : StateMachineBehaviour
{
    [SerializeField]string targetVelocityX = string.Empty;
    [SerializeField]string targetVelocityY = string.Empty;
    [SerializeField]string targetVelocityZ = string.Empty;
    [SerializeField]float delayWhenEnter = 0f;
    private float delayTime = 0f;
    Rigidbody m_rigidbody = null;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_rigidbody)
        {
            m_rigidbody = animator.GetComponent<Rigidbody>();
        }
        delayTime = Time.time + delayWhenEnter;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var addingVelocityX = (targetVelocityX!=string.Empty)?animator.GetFloat(targetVelocityX):m_rigidbody.velocity.x;
        var addingVelocityY = (targetVelocityY!=string.Empty)?animator.GetFloat(targetVelocityY):m_rigidbody.velocity.y;
        var addingVelocityZ = (targetVelocityZ!=string.Empty)?animator.GetFloat(targetVelocityZ):m_rigidbody.velocity.z;

        var addVelocity = new Vector3(addingVelocityX,addingVelocityY,addingVelocityZ);
        
        if(delayTime < Time.time)
        {
            m_rigidbody.velocity = addVelocity;
        }
    }
}
