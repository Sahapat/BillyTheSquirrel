using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSPPlus : MonoBehaviour
{
    [SerializeField]int Max_SPPlus = 20;

    private BoxCollider m_boxcolider = null;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider,LayerMask.GetMask("Character"));

        if(hitInfo.Length > 0)
        {
            GameCore.m_GameContrller.ClientPlayerTarget.SetDrink(0,Max_SPPlus,this.gameObject);
            Destroy(this);
        }
    }
}
