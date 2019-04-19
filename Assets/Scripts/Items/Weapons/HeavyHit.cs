using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHit : BaseHitSystem
{
    [SerializeField]protected float forceToAdd = 5f;
    [SerializeField]protected float shakeLenght=0.32f;
    [SerializeField]int _steminaDeplete = 0;
    [SerializeField]protected Vector3 coliderPositionLocalToRoot = Vector3.zero;
    [SerializeField]protected Vector3 coliderRotateLocalToRoot = Vector3.zero;

    public int steminaDeplete {get {return _steminaDeplete;}}
    
    void Awake()
    {
        m_hitDataStorage = new HitDataStorage(10);
    }
    void FixedUpdate()
    {
        if(isActive)
        {
            OnActive();
            CheckHit();
            isActive = (activeDurationCounter >= Time.time);
            if(!isActive)
            {
                ResetHit();
            }
        }
        else
        {
            OnInActive();
        }
    }
    public override void ActiveHit()
    {
        isActive = true;
        activeDurationCounter = Time.time + activeDuration;
    }

    public override void CancelHit()
    {
        ResetHit();
    }
    protected virtual void CheckHit()
    {
        return;
    }
    protected virtual void OnActive()
    {
        return;
    }
    protected virtual void OnInActive()
    {
        return;
    }
    protected virtual void OnResetHit()
    {
        return;
    }
    void ResetHit()
    {
        isActive = false;
        m_hitDataStorage.ResetHit();
        OnResetHit();
    }
    public void SetTargetLayer(LayerMask mask)
    {
        TargetLayer = mask;
    }
}
