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
        ROAR,
        STAYAROUND,
        ATTACK
    };
    [Header("Character Properties")]
    [SerializeField] int m_characterMaxHP = 100;
    [SerializeField] float moveSpeed = 4.2f;
    [Header("Enemy AI Ref")]
    [SerializeField] float stopDistance = 0.5f;
    [Header("Sight Ref")]
    [SerializeField] SightCheck enemySightCheck = null;
    [SerializeField] SightCheck attackSightCheck = null;
    [SerializeField] GameObject temp;

    public Health CharacterHP { get; private set; }

    private AIStateMachine aIStateMachine = AIStateMachine.GUARD;
    private Rigidbody m_rigidbody = null;
    private NavMeshPath m_NavMeshPath = null;
    private StateHandler m_stateHandler = null;

    private Transform targetPlayer = null;
    private Vector3 previousPlayerPosition = Vector3.zero;
    private int navMeshPathCornerIndex = -1;
    private bool isStop = false;

    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        m_NavMeshPath = new NavMeshPath();
        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        if (!targetPlayer) { targetPlayer = GameCore.m_GameContrller.GetClientPlayerTarget().transform; }
        previousPlayerPosition = targetPlayer.position;
    }
    void FixedUpdate()
    {
        if (enemySightCheck.targetInSight)
        {
            CalculatePath();
            FindPath();
            MoveToPos();
        }
        /* switch (aIStateMachine)
        {
            case AIStateMachine.GUARD:
                GuardState();
                break;
            case AIStateMachine.ROAR:
                RoreState();
                break;
            case AIStateMachine.CHASING:
                ChasingState();
                CalculatePath();
                FindPath();
                break;
            case AIStateMachine.STAYAROUND:
                StayaroundState();
                CalculatePath();
                FindPath();
                break;
            case AIStateMachine.ATTACK:
                AttackState();
                break;
        } */
    }
    void GuardState()
    {

    }
    void ChasingState()
    {

    }
    void RoreState()
    {

    }
    void StayaroundState()
    {

    }
    void AttackState()
    {

    }
    void CalculatePath()
    {
        if (targetPlayer.position != previousPlayerPosition)
        {
            if (NavMesh.CalculatePath(transform.position, targetPlayer.position, NavMesh.AllAreas, m_NavMeshPath))
            {
                navMeshPathCornerIndex = 1;
                #region test
                foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Respawn"))
                {
                    Destroy(temp);
                }
                for (int i = 0; i < m_NavMeshPath.corners.Length; i++)
                {
                    var obj = Instantiate(temp, m_NavMeshPath.corners[i], Quaternion.identity);
                    obj.tag = "Respawn";
                }
                #endregion
            }
            previousPlayerPosition = targetPlayer.position;
        }
    }
    void FindPath()
    {
        if (navMeshPathCornerIndex != -1)
        {
            for (int i = navMeshPathCornerIndex; i < m_NavMeshPath.corners.Length; i++)
            {
                var distance = Vector3.Distance(transform.position, m_NavMeshPath.corners[i]);
                if(distance < stopDistance)
                {
                    if (i == m_NavMeshPath.corners.Length - 1)
                    {
                        isStop = true;
                    }
                    else
                    {
                        navMeshPathCornerIndex++;
                        break;
                    }
                }
                else
                {
                    isStop = false;
                }
            }
        }
    }
    void MoveToPos()
    {
        if (isStop || navMeshPathCornerIndex == -1) return;

        LookAtPosition(m_NavMeshPath.corners[navMeshPathCornerIndex]);
        m_rigidbody.AddForce(transform.forward * moveSpeed *(m_rigidbody.drag*10+100));
        m_stateHandler.MovementSetter(Vector3.one);
    }
    void LookAtPosition(Vector3 position)
    {
        var newPosition = new Vector3(position.x, 0, position.z);
        transform.LookAt(position);
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_stateHandler.Hurt();
    }

    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
    }
}
