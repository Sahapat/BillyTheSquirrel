using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseHitSystem:MonoBehaviour
{
    [SerializeField]protected int damagePerHit = 0;
    [SerializeField]protected float delayForActive = 0f;
    [SerializeField]protected float delayForInActive = 0f;
    [SerializeField]protected LayerMask TargetLayer = 0;
    protected float ActiveDelayCounter = 0f;
    protected float InActionDelayCounter = 0f;
    protected HitDataStorage m_hitDataStorage = null;
    protected bool isSetActive = false;
    protected bool isActive = false;
    abstract public void ActiveHit();
    abstract public void CancelHit();
}
