using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHandler : MonoBehaviour
{
    [Header("Action Properties")]
    [SerializeField]float attack1ForceToAdd = 250f;
    [SerializeField]float attack2ForceToAdd = 250f;
    [SerializeField]float attack3ForceToAdd = 265f;
    [SerializeField]float HeavyAttackForceToAdd = 300f;
    private Rigidbody m_rigidbody = null;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Attack1()
    {
        m_rigidbody.AddForce(transform.forward*attack1ForceToAdd,ForceMode.Impulse);
    }
    public void Attack2()
    {
        m_rigidbody.AddForce(transform.forward*attack2ForceToAdd,ForceMode.Impulse);
    }
    public void Attack3()
    {
        m_rigidbody.AddForce(transform.forward*attack3ForceToAdd,ForceMode.Impulse);
    }
    public void HeavyAttack()
    {
        m_rigidbody.AddForce(transform.forward*HeavyAttackForceToAdd,ForceMode.Impulse);
    }
}
