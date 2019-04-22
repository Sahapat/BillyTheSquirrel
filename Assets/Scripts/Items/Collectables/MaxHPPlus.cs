using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPPlus : MonoBehaviour
{
    [SerializeField]int Max_HPPlus = 20;

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
            GameCore.m_GameContrller.ClientPlayerTarget.SetDrink(Max_HPPlus,0,this.gameObject);
            Destroy(this);
        }
    }
}
