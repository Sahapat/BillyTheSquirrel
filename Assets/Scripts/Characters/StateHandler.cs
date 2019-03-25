using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CharacterState
{
    IDLE,
    RUN,
    WEAPON0_ATTACK1,
    WEAPON0_ATTACK2,
    WEAPON0_ATTACK3,
    WEAPON0_ATTACKHEAVY,
    WEAPON1_ATTACK1,
    WEAPON1_ATTACK2,
    WEAPON1_ATTACK3,
    WEAPON1_ATTACKHEAVY,
    GUARD,
    JUMP,
    DASH,
    INAIR,
    JUMPINAIR,
    RESET,
    NONE,
};
public class StateHandler : MonoBehaviour
{
    [SerializeField] CharacterState m_characterState = CharacterState.IDLE;
    [SerializeField] int weaponType = 1;

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
    void OnDestroy()
    {
        OnStateChanged = null;
    }
    void StateChange()
    {
        OnStateChanged?.Invoke();
    }
    void UpdateState()
    {
        previousState = currentCharacterState;
        m_animator.SetInteger("WeaponHolding",weaponType);
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
    public void SetBoolWithString(string name,bool value)
    {
        m_animator.SetBool(name,value);
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
