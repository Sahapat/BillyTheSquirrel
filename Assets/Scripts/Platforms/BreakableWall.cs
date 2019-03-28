using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IAttackable
{
    [SerializeField] int maxHealth = 120;
    public Health CharacterHP {get; private set;}

    void Awake()
    {
        CharacterHP = new Health(maxHealth);
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        if(CharacterHP.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        if(CharacterHP.HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
