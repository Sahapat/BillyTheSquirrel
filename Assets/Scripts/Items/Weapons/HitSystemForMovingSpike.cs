using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystemForMovingSpike : BaseHitSystem
{
    [SerializeField] float shakeLenght = 0.35f;
    private BoxCollider m_boxcolider = null;

    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
        m_hitDataStorage = new HitDataStorage(5);
    }
    void FixedUpdate()
    {
        CheckHit();
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
    void ResetHit()
    {
        m_hitDataStorage.ResetHit();
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider, TargetLayer);
        if (hitInfo.Length == 0) return;
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                if (activeDurationCounter <= Time.time)
                {
                    var attackableObj = hitInfo[i].GetComponent<IAttackable>();
                    attackableObj?.TakeDamage(damagePerHit);
                    GameCore.m_cameraController.ShakeCamera(0.22f, shakeLenght);
                    activeDurationCounter = Time.time + activeDuration;
                }
            }
        }
    }
}
