using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHit : BaseHitSystem
{
    [SerializeField]float forceToAdd = 5f;
    [SerializeField]float shakeLenght=0.32f;
    [SerializeField]SphereCollider hitColiderChecker = null;
    [SerializeField]Vector3 coliderPositionLocalToRoot = Vector3.zero;
    [SerializeField]Vector3 coliderRotateLocalToRoot = Vector3.zero;
    void Awake()
    {
        m_hitDataStorage = new HitDataStorage(10);
        hitColiderChecker.GetComponent<MeshRenderer>().enabled = false;
    }
    void Update()
    {
        if(hitColiderChecker)
        {
            var root = this.transform.root.gameObject;
            if(root.CompareTag("Player") || root.CompareTag("Enemy") || root.CompareTag("Boss"))
            {
                hitColiderChecker.transform.parent = root.transform;
                hitColiderChecker.transform.localPosition = coliderPositionLocalToRoot;
                hitColiderChecker.transform.localRotation = Quaternion.Euler(coliderRotateLocalToRoot);
            }
        }
    }
    void FixedUpdate()
    {
        if(isActive)
        {
            hitColiderChecker.GetComponent<MeshRenderer>().enabled = true;
            CheckHit();
            isActive = (activeDurationCounter >= Time.time);
            if(!isActive)
            {
                ResetHit();
            }
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
    void ResetHit()
    {
        hitColiderChecker.GetComponent<MeshRenderer>().enabled = false;
        isActive = false;
        m_hitDataStorage.ResetHit();
    }
    void CheckHit()
    {
        var hitInfo = PhysicsExtensions.OverlapSphere(hitColiderChecker, TargetLayer);
        if (hitInfo.Length == 0) return;
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (m_hitDataStorage.CheckHit(hitInfo[i].GetInstanceID()))
            {
                var attackableObj = hitInfo[i].GetComponent<IAttackable>();
                if (hitInfo[i].CompareTag("Player") || hitInfo[i].CompareTag("Enemy"))
                {
                    var addForceDirection = -hitInfo[i].transform.forward;
                    addForceDirection.y = 0;
                    addForceDirection *= forceToAdd;
                    attackableObj?.TakeDamage(damagePerHit,addForceDirection);
                }
                else
                {
                    attackableObj?.TakeDamage(damagePerHit);
                }
                if(shakeLenght != 0)
                {
                    GameCore.m_cameraController.ShakeCamera(0.22f,shakeLenght);
                }
            }
        }
    }
}
