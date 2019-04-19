using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IAttackable
{
    [SerializeField] int maxHealth = 120;
    [SerializeField]GameObject[] destructObject =null;
    [SerializeField]float destructTime = 6f;

    public bool isBlocking{get;private set;}
    public Health CharacterHP {get; private set;}
    
    private DamageMaterial m_damageMaterial = null;
    private BoxCollider m_boxcolider = null;
    private Rigidbody[] destructRigid = null;
    private MeshCollider[] destructColider = null;


    void Awake()
    {
        CharacterHP = new Health(maxHealth);
        destructRigid = new Rigidbody[destructObject.Length];
        destructColider = new MeshCollider[destructObject.Length];
        m_boxcolider = GetComponent<BoxCollider>();
        m_damageMaterial = GetComponent<DamageMaterial>();

        for(int i=0;i<destructObject.Length;i++)
        {
            destructRigid[i] = destructObject[i].GetComponent<Rigidbody>();
            destructColider[i] = destructObject[i].GetComponent<MeshCollider>();
            destructColider[i].enabled = false;
            destructRigid[i].useGravity = false;
            destructRigid[i].isKinematic = true;
        }
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_damageMaterial.TakeDamageMaterialActive(CharacterHP.HP,CharacterHP.MaxHP);
        if(CharacterHP.HP <= 0)
        {
            foreach(var i in destructRigid)
            {
                i.useGravity = true;
                i.isKinematic = false;
            }
            foreach(var i in destructColider)
            {
                i.enabled = true;
            }
            m_boxcolider.enabled = false;
            m_damageMaterial.FadeOut(destructTime);
        }
    }

    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        m_damageMaterial.TakeDamageMaterialActive(CharacterHP.HP,CharacterHP.MaxHP);
        if(CharacterHP.HP <= 0)
        {
            foreach(var i in destructRigid)
            {
                i.useGravity = true;
                i.isKinematic = false;
            }
            foreach(var i in destructColider)
            {
                i.enabled = true;
            }
            m_boxcolider.enabled = false;
            m_damageMaterial.FadeOut(destructTime);
        }
    }
}
