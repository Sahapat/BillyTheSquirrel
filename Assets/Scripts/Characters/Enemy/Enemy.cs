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
    [SerializeField] BaseSword swordInHand = null;

    public Health CharacterHP { get; private set; }
    public bool isDead { get; private set; }

    private AIStateMachine aIStateMachine = AIStateMachine.GUARD;
    private AIStateMachine previousAiState = AIStateMachine.NONE;
    //Component require
    private Rigidbody m_rigidbody = null;
    private StateHandler m_stateHandler = null;
    private CustomNavMeshAgent m_navMeshAgent = null;
    private DamageMaterial m_damageMaterial = null;
    private RagdollController m_ragdoll = null;

    //General Var
    private Transform targetPlayer = null;
    private Vector3 previousTargetPosition = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;
    private bool canCancelAnimation = true;

    //Checker variable
    private float counterForRoar = 0f;
    private float counterForAttack = 0f;
    private float roarElapsed = 3.0f;
    private float attackElapsed = 0.0f;
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
        m_damageMaterial = GetComponent<DamageMaterial>();
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
        if (isDead) { return; }
        if (GameCore.m_GameContrller.GetClientPlayerTarget().isDead)
        {
            if (counterForAttack <= Time.time)
            {
                ResetState();
                aIStateMachine = AIStateMachine.BACK;
            }
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
                aIStateMachine = AIStateMachine.ATTACK;
                attackElapsed = 1f;
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
            if (counterForRoar <= Time.time)
            {
                roarTrigger = false;
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
        m_navMeshAgent.isStopped = true;
        if (attackSightCheck.targetInSight)
        {
            attackElapsed += Time.deltaTime;
            if (attackElapsed > 1f)
            {
                if (counterForAttack <= Time.time && Random.value <= chanceForCombo)
                {
                    counterForAttack = Time.time + 4f + attackWaitDuration;
                    canCancelAnimation = false;
                    attackElapsed = 0;
                    StartCoroutine(DoCombo());
                }
                else if (counterForAttack <= Time.time && Random.value <= chanceForNormalAttack)
                {
                    counterForAttack = Time.time + attackWaitDuration;
                    canCancelAnimation = true;
                    m_stateHandler.NormalAttack();
                    attackElapsed = 0;
                }
            }

        }
        else if (m_stateHandler.currentCharacterState == CharacterState.IDLE && counterForAttack <= Time.time)
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
        if (value <= 0)
        {
            isDead = true;
            m_stateHandler.SetBool("isDead",true);
            m_ragdoll.ActiveRagdoll(m_rigidbody.velocity);
            m_damageMaterial.FadeOut(1f);
            Destroy(this.gameObject, 5f);
        }
    }
    IEnumerator DoCombo()
    {
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack1;
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack2;
        LookAtPosition(targetPlayer.position);
        m_stateHandler.NormalAttack();
        yield return waitComboAttack3;
        canCancelAnimation = true;
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        if (canCancelAnimation)
        {
            m_stateHandler.Hurt();
        }
    }
    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        if (canCancelAnimation)
        {
            m_rigidbody.velocity = forceToAdd;
            m_stateHandler.Hurt();
        }
    }
}
