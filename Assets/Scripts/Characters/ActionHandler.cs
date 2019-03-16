using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateHandler))]
public class ActionHandler : MonoBehaviour
{
    [System.Serializable]
    private struct ActionMotion
    {
        public float delayForAddForce;
        public float forceToAdd;
    }

    [Header("Action Motion Setter")]
    [SerializeField] ActionMotion[] actionMotions;

    StateHandler m_stateHandler = null;
    Rigidbody m_rigidbody = null;
    BaseSword m_baseSword = null;

    int queueMotionIndex = -1;
    float CounterForMotionAdded = 0;

    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_baseSword = GetComponentInChildren<BaseSword>();
    }
    void Start()
    {
        m_stateHandler.OnStateChanged += OnActionState;
    }
    void FixedUpdate()
    {
        if (queueMotionIndex == -1) return;
        if (CounterForMotionAdded <= Time.time)
        {
            m_rigidbody.AddForce(transform.forward * actionMotions[queueMotionIndex].forceToAdd, ForceMode.Impulse);
            queueMotionIndex = -1;
        }
    }
    void OnActionState()
    {
        switch (m_stateHandler.currentCharacterState)
        {
            case CharacterState.WEAPON0_ATTACK1:
                queueMotionIndex = 0;
                break;
            case CharacterState.WEAPON0_ATTACK2:
                queueMotionIndex = 1;
                break;
            case CharacterState.WEAPON0_ATTACK3:
                queueMotionIndex = 2;
                break;
            case CharacterState.WEAPON0_ATTACKHEAVY:
                queueMotionIndex = 3;
                break;
            case CharacterState.WEAPON1_ATTACK1:
                queueMotionIndex = 4;
                break;
            case CharacterState.WEAPON1_ATTACK2:
                queueMotionIndex = 5;
                break;
            case CharacterState.WEAPON1_ATTACK3:
                queueMotionIndex = 6;
                break;
            case CharacterState.WEAPON1_ATTACKHEAVY:
                queueMotionIndex = 7;
                break;
            case CharacterState.RESET:
                m_baseSword.hitSystemManager.CancelAllHit();
                break;
        }
        if(queueMotionIndex != -1)
        {
            CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
        }
    }
    public void ActiveNormalHit1(int index)
    {
        m_baseSword.hitSystemManager.ActiveNormalHit(index);
    }
    public void ActiveHeavyHit()
    {
        m_baseSword.hitSystemManager.ActiveHeavyHit();
    }
    public void UpdateSword(BaseSword baseSword)
    {
        m_baseSword = baseSword;
    }
}
