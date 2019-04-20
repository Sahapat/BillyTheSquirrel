using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWithNoReset : BaseHitSystem
{
    [SerializeField] float shakeLenght = 0.32f;

    private SphereCollider m_sphereColider = null;
    void Awake()
    {
        m_hitDataStorage = new HitDataStorage(5);
        m_sphereColider = GetComponent<SphereCollider>();
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
        isActive = false;
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapSphere(m_sphereColider, TargetLayer);
        if (hitInfo.Length == 0) return;
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var attackableObj = hitInfo[i].GetComponent<IAttackable>();
                attackableObj.TakeDamage(damagePerHit);
                if (shakeLenght != 0)
                {
                    GameCore.m_cameraController.ShakeCamera(0.22f, shakeLenght);
                }
            }
        }
    }
}
