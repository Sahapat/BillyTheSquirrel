﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateHandler))]
public class ActionHandler : MonoBehaviour
{
    [System.Serializable]
    private class ActionMotion
    {
        public float delayForAddForce = 0;
        public float forceToAdd = 0;
    }

    [Header("Action Motion Setter")]
    [SerializeField] ActionMotion[] actionMotions = null;

    StateHandler m_stateHandler = null;
    Rigidbody m_rigidbody = null;
    BaseWeapon m_baseWeapon = null;

    int queueMotionIndex = -1;
    float CounterForMotionAdded = 0;

    void Awake()
    {
        m_stateHandler = GetComponent<StateHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_baseWeapon = GetComponentInChildren<BaseWeapon>();
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
            m_rigidbody.velocity = transform.forward * actionMotions[queueMotionIndex].forceToAdd;
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
            case CharacterState.WEAPON2_ATTACK1:
                queueMotionIndex = 8;
                break;
            case CharacterState.WEAPON2_ATTACK2:
                queueMotionIndex = 9;
                break;
            case CharacterState.WEAPON2_ATTACK3:
                queueMotionIndex = 10;
                break;
            case CharacterState.WEAPON2_ATTACKHEAVY:
                queueMotionIndex = 11;
                break;
            case CharacterState.WEAPON3_ATTACK1:
                queueMotionIndex = 12;
                break;
            case CharacterState.WEAPON3_ATTACK2:
                queueMotionIndex = 13;
                break;
            case CharacterState.WEAPON3_ATTACK3:
                queueMotionIndex = 14;
                break;
            case CharacterState.WEAPON3_ATTACKHEAVY:
                queueMotionIndex = 15;
                break;
            case CharacterState.RESET:
                if (m_baseWeapon)
                {
                    m_baseWeapon.hitSystemManager.CancelAllHit();
                    queueMotionIndex = -1;
                }
                break;
        }
        if (queueMotionIndex != -1)
        {
            CounterForMotionAdded = Time.time + actionMotions[queueMotionIndex].delayForAddForce;
        }
    }
    public void ActiveNormalHit(int index)
    {
        m_baseWeapon.hitSystemManager.ActiveNormalHit(index);
    }
    public void ActiveHeavyHit()
    {
        m_baseWeapon.hitSystemManager.ActiveHeavyHit();
    }
    public void UpdateSword(BaseWeapon baseWeapon)
    {
        m_baseWeapon = baseWeapon;
    }
}
