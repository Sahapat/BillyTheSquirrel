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
            case CharacterState.ATTACK1:
                queueMotionIndex = 0;
                CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
                break;
            case CharacterState.ATTACK2:
                queueMotionIndex = 1;
                CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
                break;
            case CharacterState.ATTACK3:
                queueMotionIndex = 2;
                CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
                break;
            case CharacterState.ATTACKHEAVY:
                queueMotionIndex = 3;
                CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
                break;
            case CharacterState.RESET:
                m_baseSword.hitSystemManager.CancelAllHit();
                break;
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
