﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : BaseHitSystem
{
    private BoxCollider m_boxcolider = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_hitDataStorage = new HitDataStorage(8);
    }
    void FixedUpdate()
    {
        if(!isSetActive)
        {
            return;
        }
        Counting();
        if(isActive)
        {
            CheckHit();
        }
    }
    public override void ActiveHit()
    {
        isSetActive = true;
        ActiveDelayCounter = Time.time + delayForActive;
        InActionDelayCounter = ActiveDelayCounter + delayForInActive;
    }
    public override void CancelHit()
    {
        ResetHit();
    }
    void ResetHit()
    {
        isActive = false;
        isSetActive = false;
        m_hitDataStorage.ResetHit();
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, TargetLayer);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var character = hitInfo[i].GetComponent<ICharacter>();
                character.TakeDamage(damagePerHit);
            }
        }
    }
    void Counting()
    {
        if (ActiveDelayCounter <= Time.time)
        {
            isActive = true;
        }
        if (InActionDelayCounter <= Time.time)
        {
            ResetHit();
        }
    }
}