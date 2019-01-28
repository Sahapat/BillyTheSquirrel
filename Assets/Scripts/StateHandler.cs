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
    public void MoveCharacter(Vector3 Axis)
    {
        if(Axis == Vector3.zero)
        {
            m_animator.SetFloat("MovementSpeed",0);
            return;
        }
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right,Vector3.up).normalized * Axis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward,Vector3.up).normalized * Axis.x;
        Vector3 direction = newForward +newRight;
        Vector3 newAxis = new Vector3(direction.x,direction.z);

        var target = transform.position + newAxis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x,relativeVector.y);
        var degree = (radian *180)/Mathf.PI;

        transform.eulerAngles = new Vector3(0,degree,0);
        var weight = new Vector2(Axis.x,Axis.y).magnitude;
        if(weight > 0.5f)
        {
            m_animator.SetFloat("MovementSpeed",2);
            m_rigidbody.velocity = direction * runSpeed;
        }
        else
        {
            m_animator.SetFloat("MovementSpeed",1);
            m_rigidbody.velocity = direction * normalSpeed;
        }
    }
    void UpdateState()
    {
        currentAnimationState = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
