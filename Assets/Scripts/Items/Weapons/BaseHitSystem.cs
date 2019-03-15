using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHitSystem:MonoBehaviour
{
    [SerializeField]protected int damagePerHit = 0;
    [SerializeField]protected float activeDuration = 0;
    [SerializeField]protected LayerMask TargetLayer = 0;

    protected HitDataStorage m_hitDataStorage = null;
    protected bool isActive = false;
    protected float activeDurationCounter = 0;
    abstract public void ActiveHit();
    abstract public void CancelHit();
}
