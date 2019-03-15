using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item,IPopable
{
    [SerializeField]int HpHeal = 0;
    [SerializeField]int HPMax = 0;
    [SerializeField]int SPHeal = 0;
    [SerializeField]int SpMax = 0;
    Rigidbody m_rigidBody = null;
    BoxCollider m_boxcolider = null;
    bool isPopOut = false;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_boxcolider = GetComponent<BoxCollider>();
        m_rigidBody.isKinematic = true;
    }
    void Update()
    {
        if (isPopOut)
        {
            var hitGround = PhysicsExtensions.OverlapBox(m_boxcolider, LayerMask.GetMask("Ground"));
            if (hitGround.Length > 0)
            {
                m_rigidBody.isKinematic = true;
                isPopOut = false;
            }
        }
    }
    public void PopOut(float xForce, float zForce, float forceToAdd)
    {
        m_rigidBody.isKinematic = false;
        isPopOut = true;
        m_rigidBody.AddForce(new Vector3(xForce,Vector3.up.y,zForce) * forceToAdd, ForceMode.Impulse);
    }

    public override void Use(Player player)
    {
        player.Heal(HpHeal);
        player.CharacterHP.SetMaxHP(player.CharacterHP.HP+HPMax);
        player.CharacterStemina.AddSP(SPHeal);
        player.CharacterStemina.SetMaxSP(player.CharacterStemina.SP+SpMax);
    }
}
