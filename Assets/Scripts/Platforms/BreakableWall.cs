using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IAttackable
{
    [SerializeField] int maxHealth = 120;
    public Health CharacterHP {get; private set;}
    private DamageMaterial m_damageMaterial = null;
    void Awake()
    {
        CharacterHP = new Health(maxHealth);
        m_damageMaterial = GetComponent<DamageMaterial>();
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_damageMaterial.TakeDamageMaterialActive(CharacterHP.HP,CharacterHP.MaxHP);
        if(CharacterHP.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        m_damageMaterial.TakeDamageMaterialActive(CharacterHP.HP,CharacterHP.MaxHP);
        if(CharacterHP.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
