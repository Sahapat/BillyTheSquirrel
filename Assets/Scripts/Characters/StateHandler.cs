using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterState
{
    IDLE = 0,
    RUN = 1,
    ATTACK1 = 2,
    ATTACK2 = 3,
    ATTACK3 = 4,
    ATTACKHEAVY = 5,
    GUARD = 6,
    JUMP = 7,
    DASH = 8,
    INAIR = 9,
    RESET = 10,
    NONE = 11,
    JUMPINAIR = 12
};
public class StateHandler : MonoBehaviour
{
    [SerializeField] CharacterState m_characterState = CharacterState.IDLE;
    [Space]
    [Header("Movement")]
    [SerializeField] float runSpeed = 0f;
    [SerializeField] float shieldRunSpeed = 0f;
    [SerializeField] float jumpStartDelay = 0.2f;
    [SerializeField] float jumpUpScale = 3.2f;
    [SerializeField] float jumpMoveScale = 2.5f;
    [Header("Drag")]
    [SerializeField] float normalDrag = 10f;
    [SerializeField] float fallDrag = 0;
    private Animator m_animator = null;
    private Rigidbody m_rigidbody = null;
    private GroundChecker m_groundChecker = null;
    private Vector2 Movement = Vector2.zero;

    private float CounterForJumpStart = 0f;
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
    }
    void Update()
    {
        UpdateState();
    }
    void FixedUpdate()
    {
        m_rigidbody.drag = (m_groundChecker.isOnGround) ? normalDrag : fallDrag;
        switch (m_characterState)
        {
            case CharacterState.RUN:
                DoMovement();
                RotateToAxis();
                break;
            case CharacterState.JUMP:
                DoJump();
                break;
            case CharacterState.JUMPINAIR:
                WhileJump();
                break;
            case CharacterState.DASH:
                m_rigidbody.drag = normalDrag;
                break;
            case CharacterState.RESET:
                ResetEveryThing();
                break;
        }
        m_animator.SetBool("isOnGround", m_groundChecker.isOnGround);
    }
    public bool NormalAttack()
    {
        if (m_animator.GetBool("Controlable"))
        {
            m_animator.SetTrigger("Attack");
            m_animator.SetBool("Controlable", false);
            return true;
        }
        return false;
    }
    public bool HeavyAttack()
    {
        if (m_animator.GetBool("Controlable"))
        {
            m_animator.SetTrigger("AttackHeavy");
            m_animator.SetBool("Controlable", false);
            return true;
        }
        return false;
    }
    public bool Dash()
    {
        if (m_animator.GetBool("Controlable") && m_groundChecker.isOnGround)
        {
            RotateToAxis();
            m_animator.SetTrigger("Dash");
            m_animator.SetBool("Controlable", false);
            return true;
        }
        return false;
    }
    public void MovementSetter(Vector3 Axis)
    {
        if (Axis == Vector3.zero || !m_groundChecker.isOnGround)
        {
            m_animator.SetFloat("MovementMagnitude", 0);
            return;
        }
        var weight = new Vector2(Axis.x, Axis.y).magnitude;
        Movement = Axis * runSpeed;
        m_animator.SetFloat("MovementMagnitude", weight);
    }
    public void Jump()
    {
        if (m_animator.GetBool("Controlable") && m_groundChecker.isOnGround)
        {
            m_animator.SetTrigger("Jump");
            CounterForJumpStart = Time.time + jumpStartDelay;
        }
    }
    void RotateToAxis()
    {
        var temp = Movement.normalized;
        var Axis = new Vector3(Movement.x, Movement.y, 0);
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x, relativeVector.y);
        var degree = (radian * 180) / Mathf.PI;

        transform.eulerAngles = new Vector3(0, degree, 0);
    }
    void ResetEveryThing()
    {

    }
    void WhileJump()
    {
        m_rigidbody.AddForce(new Vector3(Movement.x * jumpMoveScale, 0, Movement.y * jumpMoveScale));
    }
    void DoJump()
    {
        if (CounterForJumpStart <= Time.time)
        {
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, jumpUpScale, m_rigidbody.velocity.z);
        }
    }
    void DoMovement()
    {
        m_rigidbody.velocity = new Vector3(Movement.x, m_rigidbody.velocity.y, Movement.y);
    }
    void UpdateState()
    {
        m_characterState = (CharacterState)m_animator.GetInteger("CharacterState");
    }
}
