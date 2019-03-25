using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IAttackable
{
    public enum AIStateMachine
    {
        GUARD,
        CHASING,
        ROAR,
        STAYAROUND,
        ATTACK,
        BACK,
        NONE
    };
    [Header("Enemy Properties")]
    [SerializeField] int m_characterMaxHP = 100;
    [SerializeField, Range(0f, 1f)] float chanceForRoar = 0.5f;
    [SerializeField] float roarDuration = 2.5f;
    [SerializeField, Range(0f, 1f)] float chanceForNormalAttack = 0.5f;
    [SerializeField, Range(0f, 1f)] float chanceForCombo = 0.2f;
    [SerializeField] float attackWaitDuration = 2.2f;

    [Header("Enemy Ref")]
    [SerializeField] SightCheck enemySightCheck = null;
    [SerializeField] SightCheck attackSightCheck = null;
    [SerializeField] GameObject alertIconNormal = null;
    [SerializeField] GameObject alertIconDanger = null;
    [SerializeField] BaseSword swordInHand = null;

    public Health CharacterHP { get; private set; }
    public bool isDead {get;private set;}

    private AIStateMachine aIStateMachine = AIStateMachine.GUARD;
    private AIStateMachine previousAiState = AIStateMachine.NONE;
    private Rigidbody m_rigidbody = null;
    private StateHandler m_stateHandler = null;
    private CustomNavMeshAgent m_navMeshAgent = null;
    private RagdollController m_ragdoll = null;
    private Transform targetPlayer = null;
    private Vector3 previousTargetPosition = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    private int navMeshPathCornerIndex = -1;
    private bool canCancelAnimation = true;

    //Checker variable
    private float counterForRoar = 0f;
    private float counterForAttack = 0f;
    private float roarElapsed = 3.0f;

    private bool roarTrigger = false;
    private WaitForSeconds waitComboAttack1 = null;
    private WaitForSeconds waitComboAttack2 = null;
    private WaitForSeconds waitComboAttack3 = null;

    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);

        waitComboAttack1 = new WaitForSeconds(1.3f);
        waitComboAttack2 = new WaitForSeconds(0.65f);
        waitComboAttack3 = new WaitForSeconds(0.85f);

        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_navMeshAgent = GetComponent<CustomNavMeshAgent>();
        m_ragdoll = GetComponent<RagdollController>();
    }
    void Start()
    {
        if (!targetPlayer) { targetPlayer = GameCore.m_GameContrller.GetClientPlayerTarget().transform; }
        CharacterHP.OnHPChanged += CheckHealth;
        startPosition = transform.position;
        startRotation = transform.rotation;
        previousTargetPosition = targetPlayer.position;
        m_ragdoll.InActiveRagdoll();
        CheckStateChange();
    }
    void FixedUpdate()
    {
        if(isDead){return;}
        if (GameCore.m_GameContrller.GetClientPlayerTarget().isDead)
        {
            ResetState();
            aIStateMachine = AIStateMachine.BACK;
        }
        switch (aIStateMachine)
        {
            case AIStateMachine.GUARD:
                GuardState();
                break;
            case AIStateMachine.ROAR:
                RoreState();
                break;
            case AIStateMachine.CHASING:
                ChasingState();
                break;
            case AIStateMachine.ATTACK:
                AttackState();
                break;
            case AIStateMachine.BACK:
                BackState();
                break;
        }
    }
    void OnStateChange()
    {
    }
    void GuardState()
    {
        SightCheck();
    }
    void ChasingState()
    {
        if (enemySightCheck.targetInSight)
        {
            if (attackSightCheck.targetInSight)
            {
                m_stateHandler.MovementSetter(Vector2.zero);
                m_navMeshAgent.isStopped = true;
                aIStateMachine = AIStateMachine.ATTACK;
            }
            else
            {
                m_navMeshAgent.isStopped = false;
                m_stateHandler.MovementSetter(Vector2.one);
                m_navMeshAgent.SetDestination(targetPlayer.position);
            }
        }
        else
        {
            ResetState();
            aIStateMachine = AIStateMachine.BACK;
        }
    }
    void RoreState()
    {
        if (roarTrigger)
        {
            swordInHand.gameObject.SetActive(false);
            if (counterForRoar <= Time.time)
            {
                roarTrigger = false;
                swordInHand.gameObject.SetActive(true);
                aIStateMachine = AIStateMachine.CHASING;
            }
        }
    }
    void StayaroundState()
    {

    }
    void BackState()
    {
        m_navMeshAgent.isStopped = false;
        var distanceFromStart = Vector3.Distance(transform.position, startPosition);
        if (distanceFromStart <= 0.5f)
        {
            m_navMeshAgent.isStopped = true;
            m_stateHandler.MovementSetter(Vector2.zero);
            transform.rotation = startRotation;
            aIStateMachine = AIStateMachine.GUARD;
        }
        else
        {
            m_stateHandler.MovementSetter(Vector2.one);
            m_navMeshAgent.SetDestination(startPosition);
        }
    }
    void AttackState()
    {
        if (attackSightCheck.targetInSight)
        {
            if (counterForAttack <= Time.time)
            {
                if (Random.value <= chanceForCombo)
                {
                    m_stateHandler.SetBoolWithString("isStartAttack", false);
                    canCancelAnimation = false;
                    alertIconDanger.SetActive(true);
                    Invoke("ResetAlertIcon", 0.4f);
                    StartCoroutine(DoCombo());
                }
                else if (Random.value <= chanceForNormalAttack)
                {
                    counterForAttack = Time.time + attackWaitDuration;
                    canCancelAnimation = false;
                    m_stateHandler.SetBoolWithString("isStartAttack", false);
                    alertIconNormal.SetActive(true);
                    Invoke("ResetAlertIcon", 0.4f);
                    LookAtPosition(targetPlayer.position);
                    m_stateHandler.NormalAttack();
                }
            }
            else if (m_stateHandler.currentCharacterState == CharacterState.IDLE)
            {
                LookAtPosition(targetPlayer.position);
            }
        }
        else if (m_stateHandler.currentCharacterState == CharacterState.IDLE)
        {
            aIStateMachine = AIStateMachine.CHASING;
        }
    }
    void CheckStateChange()
    {
        if (previousAiState != aIStateMachine)
        {
            OnStateChange();
            previousAiState = aIStateMachine;
        }
    }
    void ResetState()
    {
        roarTrigger = false;
        counterForRoar = 0f;
        swordInHand.gameObject.SetActive(true);
    }
    void ResetAlertIcon()
    {
        alertIconNormal.SetActive(false);
        alertIconDanger.SetActive(false);
        m_stateHandler.SetBoolWithString("isStartAttack", true);
        canCancelAnimation = true;
    }
    void LookAtPosition(Vector3 position)
    {
        var newPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(position);
    }
    void SightCheck()
    {
        if (enemySightCheck.targetInSight)
        {
            if (roarElapsed >= 3f)
            {
                if (!roarTrigger && Random.value <= chanceForRoar)
                {
                    roarTrigger = true;
                    aIStateMachine = AIStateMachine.ROAR;
                    counterForRoar = Time.time + roarDuration;
                    m_stateHandler.UsePotion();
                }
                else
                {
                    aIStateMachine = AIStateMachine.CHASING;
                }
                roarElapsed -= 3f;
            }
            if (!roarTrigger)
            {
                roarElapsed += Time.deltaTime;
            }
        }
    }
    void CheckHealth(int value)
    {
        if(value <= 0)
        {
            m_ragdoll.ActiveRagdoll(m_rigidbody.velocity);
            Destroy(this.gameObject,3f);
        }
    }
    IEnumerator DoCombo()
    {
        counterForAttack = Time.time + attackWaitDuration + 3.5f;
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack1;
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack2;
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack3;
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        if (canCancelAnimation)
        {
            m_stateHandler.Hurt();
            ResetAlertIcon();
        }
    }
    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        if (canCancelAnimation)
        {
            m_rigidbody.velocity = forceToAdd;
            m_stateHandler.Hurt();
            ResetAlertIcon();
        }
    }
}
