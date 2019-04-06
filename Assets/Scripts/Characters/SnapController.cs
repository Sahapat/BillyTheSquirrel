using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapController : MonoBehaviour
{
    [SerializeField] BoxCollider snap_checker = null;
    [SerializeField] LayerMask CheckLayer = 0;

    private Transform HitEnemy = null;
    private StateHandler m_stateHandler = null;
    void Awake()
    {
        m_stateHandler = GetComponentInParent<StateHandler>();
        m_stateHandler.OnStateChanged += OnStateAction;
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(snap_checker, CheckLayer);
        if (hitInfo.Length > 0)
        {
            HitEnemy = hitInfo[0].transform;
        }
        else
        {
            HitEnemy = null;
        }
    }
    void LookAtTarget(Vector3 position)
    {
        var lookAtPos = new Vector3(position.x, transform.position.y, position.z);
        transform.root.transform.LookAt(lookAtPos);
    }
    void OnStateAction()
    {
        if(HitEnemy)
        {
            switch(m_stateHandler.currentCharacterState)
            {
                case CharacterState.WEAPON0_ATTACK1:
                    LookAtTarget(HitEnemy.position);
                break;
                case CharacterState.WEAPON0_ATTACK2:
                    LookAtTarget(HitEnemy.position);
                break;
                case CharacterState.WEAPON0_ATTACK3:
                    LookAtTarget(HitEnemy.position);
                break;
            }
        }
    }
}
