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
    JUMPINAIR = 10,
    RESET = 11,
    NONE = 12,
};
public class StateHandler : MonoBehaviour
{
    [SerializeField] CharacterState m_characterState = CharacterState.IDLE;
    public CharacterState currentCharacterState{get {return m_characterState;}}
    public Vector2 Movement{get;private set;}
    public delegate void _Func();
    public event _Func OnStateChanged;

    private Animator m_animator = null;
    private CharacterState previousState = CharacterState.NONE;
    private GroundChecker m_groundChecker = null;
    
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
    }
    void Update()
    {
        UpdateState();
    }
    void StateChange()
    {
        OnStateChanged?.Invoke();
    }
    void UpdateState()
    {
        previousState = currentCharacterState;
        m_characterState = (CharacterState)m_animator.GetInteger("CharacterState");
        m_animator.SetBool("isOnGround", m_groundChecker.isOnGround);
        if(previousState != currentCharacterState)
        {
            StateChange();
        }
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
            m_animator.SetTrigger("Dash");
            m_animator.SetBool("Controlable", false);
            return true;
        }
        return false;
    }
    public bool Hurt()
    {
        m_animator.SetTrigger("Hurt");
        return true;
    }
    public bool UsePotion()
    {
        m_animator.SetTrigger("UsePotion");
        return true;
    }
    public bool GetControlable()
    {
        return m_animator.GetBool("Controlable");
    }
    public void MovementSetter(Vector3 Axis)
    {
        Movement = Axis;
        var weight = new Vector2(Axis.x, Axis.y).magnitude;
        m_animator.SetFloat("MovementMagnitude", weight);
    }
    public void Jump()
    {
        if (m_animator.GetBool("Controlable") && m_groundChecker.isOnGround)
        {
            m_animator.SetTrigger("Jump");
        }
    }
}
