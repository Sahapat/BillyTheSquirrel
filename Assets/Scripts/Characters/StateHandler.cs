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
    WEAPON2_ATTACK1,
    WEAPON2_ATTACK2,
    WEAPON2_ATTACK3,
    WEAPON2_ATTACKHEAVY,
    WEAPON3_ATTACK1,
    WEAPON3_ATTACK2,
    WEAPON3_ATTACK3,
    WEAPON3_ATTACKHEAVY,
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
    [SerializeField] int weaponType = 0;

    public CharacterState currentCharacterState { get { return m_characterState; } }
    public Vector2 Movement { get; private set; }
    public delegate void _Func();
    public event _Func OnStateChanged;

    private Animator m_animator = null;
    private CharacterState previousState = CharacterState.NONE;
    private GroundChecker m_groundChecker = null;

    private bool canHoldShield = false;
    private BaseWeapon baseWeapon = null;
    void Awake()
    {
        m_animator = GetComponent<Animator>();
        baseWeapon = GetComponentInChildren<BaseWeapon>();
        m_groundChecker = GetComponentInChildren<GroundChecker>();
        canHoldShield = true;
        m_animator.SetBool("isSpecial",false);
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
        if (currentCharacterState == CharacterState.RUN || currentCharacterState == CharacterState.IDLE)
        {
            canHoldShield = true;
            m_animator.SetBool("isSpecial",false);
        }
    }
    void UpdateState()
    {
        previousState = currentCharacterState;
        m_animator.SetInteger("WeaponHolding", weaponType);
        m_characterState = (CharacterState)m_animator.GetInteger("CharacterState");
        m_animator.SetBool("isOnGround", m_groundChecker.isOnGround);
        if(baseWeapon.transform.root.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            baseWeapon = GetComponentInChildren<BaseWeapon>();
        }
        else
        {
            weaponType = (int)baseWeapon.weaponType;
        }
        if (previousState != currentCharacterState)
        {
            StateChange();
        }
    }
    public bool NormalAttack()
    {
        if (m_animator.GetBool("Controlable") && !m_animator.GetBool("isHoldingShield"))
        {
            m_animator.SetTrigger("Attack");
            m_animator.SetBool("Controlable", false);
            canHoldShield = false;
            return true;
        }
        return false;
    }
    public bool HeavyAttack()
    {
        if (m_animator.GetBool("Controlable") && !m_animator.GetBool("isHoldingShield"))
        {
            m_animator.SetTrigger("AttackHeavy");
            m_animator.SetBool("Controlable", false);
            canHoldShield = false;
            return true;
        }
        return false;
    }
    public bool SetHoldingShield()
    {
        if (m_animator.GetBool("Controlable") && canHoldShield)
        {
            m_animator.SetBool("isHoldingShield", true);
            return true;
        }
        return false;
    }
    public bool SetUnHoldingShield()
    {
        if (m_animator.GetBool("Controlable"))
        {
            m_animator.SetBool("isHoldingShield", false);
            return true;
        }
        return false;
    }
    public bool Dash()
    {
        if (m_animator.GetBool("Controlable") && m_groundChecker.isOnGround && !m_animator.GetBool("isHoldingShield"))
        {
            m_animator.SetTrigger("Dash");
            m_animator.SetBool("Controlable", false);
            canHoldShield = false;
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
    public void SetBool(string name, bool value)
    {
        m_animator.SetBool(name, value);
    }
    public bool GetBool(string name)
    {
        return m_animator.GetBool(name);
    }
}
