using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateHandler : MonoBehaviour
{
    [SerializeField] string currentAnimationState = string.Empty;
    [Space]
    [Header("Movement")]
    [SerializeField] float normalSpeed = 0f;
    [SerializeField] float runSpeed = 0f;
    private Animator m_animator;
    private Rigidbody m_rigidbody;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        UpdateState();
    }
    public void MovementSetter(Vector3 Axis)
    {
        if(Axis == Vector3.zero)
        {
            m_animator.SetFloat("MovementSpeed",0);
            return;
        }
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x,relativeVector.y);
        var degree = (radian *180)/Mathf.PI;

        transform.eulerAngles = new Vector3(0,degree,0);
        var weight = new Vector2(Axis.x,Axis.y).magnitude;
        if(weight > 0.5f)
        {
            m_animator.SetFloat("MovementSpeed",2);
            m_rigidbody.velocity = new Vector3(Axis.x*runSpeed,m_rigidbody.velocity.y,Axis.y*runSpeed);
        }
        else
        {
            m_animator.SetFloat("MovementSpeed",1);
            m_rigidbody.velocity = new Vector3(Axis.x*normalSpeed,m_rigidbody.velocity.y,Axis.y*normalSpeed);
        }
    }
    void UpdateState()
    {
        currentAnimationState = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
