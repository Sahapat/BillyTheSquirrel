using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ICharacter
{
    public enum AIStateMachine
    {
        GUARD,
        CHASING,
        ATTACK
    };

    [SerializeField] int maxHealth = 100;
    [SerializeField] Vector3 StartPosition = Vector3.zero;
    [Header("Enemy StateMachine")]
    [SerializeField] Enemy_SightCheck m_DetechPlayer = null;
    [SerializeField] Enemy_SightCheck m_AttackSight = null;
    [SerializeField, Range(0, 100)] int chanceForCombo = 50;
    [SerializeField, Range(0, 100)] int chanceForHeavyAttack = 30;
    [SerializeField] float stateAttackDelay = 1.2f;
    public Health CharacterHP { get; private set; }

    private AIStateMachine m_stateMachine = AIStateMachine.GUARD;
    private StateHandler m_stateHandler = null;
    private NavMeshAgent m_Agent = null;
    private bool attackTrigger = false;
    private bool isGoFirstAttack = false;
    private float CounterForStateAttackDelay = 0f;
    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_Agent = GetComponent<NavMeshAgent>();
        if (StartPosition == Vector3.zero) StartPosition = this.transform.position;
        CharacterHP = new Health(maxHealth);
    }
    void Start()
    {
        CharacterHP.OnHPChanged += CheckDeath;
    }
    void FixedUpdate()
    {
        StateConditionCheck();

        switch (m_stateMachine)
        {
            case AIStateMachine.GUARD:
                GuardState();
                break;
            case AIStateMachine.CHASING:
                ChasingState();
                break;
            case AIStateMachine.ATTACK:
                AttackState();
                AttackSpeculate();
                break;
        }
    }
    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
        m_stateHandler.UsePotion();
    }

    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_stateHandler.Hurt();
    }
    void GuardState()
    {
        isGoFirstAttack = false;
        m_Agent.SetDestination(StartPosition);
        m_stateHandler.MovementSetter(new Vector2(transform.forward.x, transform.forward.z));

        if (m_Agent.remainingDistance <= 0)
        {
            m_Agent.isStopped = true;
            m_stateHandler.MovementSetter(Vector2.zero);
        }
    }
    void AttackState()
    {
        m_Agent.isStopped = true;
        if (m_AttackSight.EnemyInSight && m_stateHandler.GetControlable())
        {
            transform.LookAt(m_AttackSight.EnemyInSight.transform.position);
            m_stateHandler.MovementSetter(Vector2.zero);
        }
    }
    void AttackSpeculate()
    {
        if (!isGoFirstAttack && !attackTrigger)
        {
            if (Random.value <= chanceForHeavyAttack*0.01f)
            {
                m_stateHandler.HeavyAttack();
            }
            else
            {
                m_stateHandler.NormalAttack();
            }
            CounterForStateAttackDelay = Time.time + stateAttackDelay;
            isGoFirstAttack = true;
            attackTrigger = true;
        }
        if(!attackTrigger)
        {
            if(Random.value <= chanceForCombo*0.01f)
            {
                m_stateHandler.NormalAttack();
            }
            else if(Random.value <= chanceForHeavyAttack*0.01f)
            {
                m_stateHandler.HeavyAttack();
            }
            CounterForStateAttackDelay = Time.time + stateAttackDelay;
            attackTrigger = true;
        }
        attackTrigger = (CounterForStateAttackDelay>= Time.time);
    }
    void ChasingState()
    {
        isGoFirstAttack = false;
        m_Agent.isStopped = false;
        m_Agent.SetDestination(m_DetechPlayer.EnemyInSight.transform.position);
        m_stateHandler.MovementSetter(new Vector2(transform.forward.x, transform.forward.z));
    }
    void StateConditionCheck()
    {
        if (m_stateHandler.GetControlable())
        {
            if (m_DetechPlayer.EnemyInSight != null)
            {
                m_stateMachine = AIStateMachine.CHASING;
            }
            else
            {
                m_stateMachine = AIStateMachine.GUARD;
            }

            if (m_AttackSight.EnemyInSight != null)
            {
                m_stateMachine = AIStateMachine.ATTACK;
            }
        }
    }
    void CheckDeath(int hpValue)
    {
        if (hpValue <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
