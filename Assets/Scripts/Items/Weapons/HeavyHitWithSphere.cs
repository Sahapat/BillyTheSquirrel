using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyHitWithSphere : HeavyHit
{
    [SerializeField]SphereCollider hitColiderChecker = null;

    private MeshRenderer hitMeshrenderer = null;

    void Start()
    {
        hitMeshrenderer = hitColiderChecker.GetComponent<MeshRenderer>();
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
    protected override void CheckHit()
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
    protected override void OnActive()
    {
        hitMeshrenderer.enabled = true;
    }
    protected override void OnInActive()
    {
        hitMeshrenderer.enabled = false;
    }
}
