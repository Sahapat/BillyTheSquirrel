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
    GUARD= 6,
    JUMP = 7,
    NONE = 10
};
public class StateHandler : MonoBehaviour
{
    [SerializeField] CharacterState m_characterState = CharacterState.IDLE;
    [Space]
    [Header("Movement")]
    [SerializeField] float runSpeed = 0f;
    [SerializeField] float shieldRunSpeed = 0f;
    [Header("Drag")]
    [SerializeField] float normalDrag = 10f;
    [SerializeField] float fallDrag = 0;
    private Animator m_animator = null;
    private Rigidbody m_rigidbody = null;
    private GroundChecker m_groundChecker = null;
    private Vector2 Movement = Vector2.zero;
    private EquipmentSword currentEquipment = null;
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
        currentEquipment = GetComponentInChildren<EquipmentSword>();
    }
    void Update()
    {
        UpdateState();
    }
    void FixedUpdate()
    {
        switch (m_characterState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.RUN:
                DoMovement();
                break;
        }
        m_rigidbody.drag = (m_groundChecker.isOnGround) ? normalDrag : fallDrag;
    }
    public bool NormalAttack()
    {
        if(m_animator.GetBool("Controlable"))
        {
            m_animator.SetTrigger("Attack");
            m_animator.SetBool("Controlable",false);
            return true;
        }
        return false;
    }
    public bool HeavyAttack()
    {
        if(m_animator.GetBool("Controlable"))
        {
            m_animator.SetTrigger("AttackHeavy");
            m_animator.SetBool("Controlable",false);
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
        Movement = Axis*runSpeed;
        m_animator.SetFloat("MovementMagnitude", weight);
        if (m_animator.GetBool("Controlable")) RotateToAxis(Axis);
    }
    void RotateToAxis(Vector3 Axis)
    {
        var target = transform.position + Axis;
        var relativeVector = (target - transform.position).normalized;
        var radian = Mathf.Atan2(relativeVector.x, relativeVector.y);
        var degree = (radian * 180) / Mathf.PI;

        transform.eulerAngles = new Vector3(0, degree, 0);
    }
    void DoMovement()
    {
        m_rigidbody.velocity = new Vector3(Movement.x, m_rigidbody.velocity.y, Movement.y);
    }
    void UpdateState()
    {
        m_characterState = (CharacterState)m_animator.GetInteger("CharacterState");
    }
    void SetAnimToBase()
    {
        m_animator.SetLayerWeight(1, 1);
        m_animator.SetLayerWeight(2, 0);
        m_animator.SetLayerWeight(3, 0);
        m_animator.Play("Idle", 1);
    }
    void SetAnimToExpand()
    {
        m_animator.SetLayerWeight(1, 0);
        m_animator.SetLayerWeight(2, 1);
        m_animator.SetLayerWeight(3, 1);
    }
}
