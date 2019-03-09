using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    [SerializeField]float cooldown = 1.5f;
    private BoxCollider m_boxcolider = null;

    private bool isAttack = false;
    private float CounterCooldown = 0;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void FixedUpdate()
    {
        if(CounterCooldown <= Time.time)
        {
            isAttack = false;
        }
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider,LayerMask.GetMask("Character"));
        if(hitInfo.Length > 0 && !isAttack)
        {
            hitInfo[0].GetComponent<ICharacter>().TakeDamage(30);
            isAttack = true;
            CounterCooldown = Time.time + cooldown;
        }
    }
}
