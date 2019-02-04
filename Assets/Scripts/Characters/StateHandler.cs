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
    [SerializeField] float groundDrag = 8f;
    [SerializeField] float fallDrag =0f;
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private GroundChecker m_groundChecker = null;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
    }
    void Update()
    {
        m_rigidbody.drag = (m_groundChecker.isOnGround)?groundDrag:fallDrag;
        UpdateState();
    }
    public void MovementSetter(Vector3 Axis)
    {
        if(Axis == Vector3.zero)
        {
            m_animator.SetFloat("MovementMagnitude",0);
            return;
        }
        var weight = new Vector2(Axis.x,Axis.y).magnitude;
        var assignMoveSpeed = (weight < 0.5f) ? Axis*normalSpeed:Axis*runSpeed;
        m_animator.SetFloat("MovementMagnitude",weight);
        m_animator.SetFloat("MovementX",assignMoveSpeed.x);
        m_animator.SetFloat("MovementZ",assignMoveSpeed.y);
        if(m_animator.GetBool("Controlable"))RotateToAxis(Axis);
    }
    void RotateToAxis(Vector3 Axis)
    {
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x,relativeVector.y);
        var degree = (radian *180)/Mathf.PI;

        transform.eulerAngles = new Vector3(0,degree,0);
    }
    void UpdateState()
    {
        currentAnimationState = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
