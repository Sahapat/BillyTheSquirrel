using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CustomNavMeshAgent : MonoBehaviour
{
    [SerializeField] float speed = 4.2f;
    [SerializeField] float stopDistance = 0.8f;
    [SerializeField] bool _isStopped = false;

    public bool isReachToTheDestinaton
    {
        get; private set;
    }
    public bool isStopped
    {
        get
        {
            return _isStopped;
        }
        set
        {
            _isStopped = value;
        }
    }
    private NavMeshPath m_NavMeshPath = null;
    private Rigidbody m_rigidbody = null;
    private Vector3 Destination = Vector3.zero;
    private Vector3 closedPointForUnreachable = Vector3.zero;
    private int navMeshPathCornerIndex = -1;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_NavMeshPath = new NavMeshPath();
    }
    void Start()
    {
        isReachToTheDestinaton = true;
        Destination = Vector3.zero;
    }
    void FixedUpdate()
    {
        if (isStopped) return;

        if (!isReachToTheDestinaton)
        {
            CalculatePath();
            FindPath();
            MoveToPos();
        }
    }
    public void SetDestination(Vector3 Destination)
    {
        this.Destination = Destination;
        isReachToTheDestinaton = false;
    }
    void CalculatePath()
    {
        NavMeshHit hit;

        if (!NavMesh.SamplePosition(Destination, out hit, 1f, NavMesh.AllAreas))
        {
            //When target isn't on the navigation mesh
            closedPointForUnreachable = hit.position;
            navMeshPathCornerIndex = -1;
        }
        else if (NavMesh.CalculatePath(transform.position, Destination, NavMesh.AllAreas, m_NavMeshPath))
        {
            navMeshPathCornerIndex = 1;
        }
    }
    void FindPath()
    {
        if (navMeshPathCornerIndex != -1)
        {
            for (int i = navMeshPathCornerIndex; i < m_NavMeshPath.corners.Length; i++)
            {
                var distance = Vector3.Distance(transform.position, m_NavMeshPath.corners[i]);
                if (distance < stopDistance)
                {
                    if (i == m_NavMeshPath.corners.Length - 1)
                    {
                        isReachToTheDestinaton = true;
                    }
                    else
                    {
                        navMeshPathCornerIndex++;
                        break;
                    }
                }
                else
                {
                    isReachToTheDestinaton = false;
                }
            }
        }
    }
    void MoveToPos()
    {
        try
        {
            LookAtPosition(m_NavMeshPath.corners[navMeshPathCornerIndex]);
            m_rigidbody.AddForce(transform.forward * speed * (m_rigidbody.drag * 15 + 100));
        }
        catch (System.IndexOutOfRangeException)
        {
            isReachToTheDestinaton = true;
        }
    }
    void LookAtPosition(Vector3 position)
    {
        var newPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.LookAt(position);
    }
}
