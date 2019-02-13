using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] int m_characterMaxHP = 100;
    [SerializeField] LayerMask itemLayer = 0;
    [SerializeField] Transform WeaponHoldPos = null;
    [SerializeField] Transform ShieldHoldPos = null;

    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }

    private CapsuleCollider m_capsuleColider = null;
    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        CharacterStemina = GetComponent<Stemina>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        CharacterHP.ResetHP();
        CharacterStemina.ResetStemina();
    }
    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
    }

    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
    }
    public bool CollectionItem()
    {
        var hitItem = PhysicsExtensions.OverlapCapsule(m_capsuleColider, itemLayer);
        if (hitItem.Length > 0)
        {
            ICollectable itemToCollect = hitItem[0].GetComponent<ICollectable>();
            switch (itemToCollect.itemType)
            {
                case ItemType.EQUIPMENT:
                    Equipment equipment = itemToCollect.PickUp().GetComponent<Equipment>();
                    if(equipment.equipmentType  == Equipment.EquipmentType.WEAPON)
                    {
                        equipment.transform.parent = WeaponHoldPos;
                        equipment.transform.localRotation = Quaternion.identity;
                        equipment.transform.localScale = Vector3.one;
                        equipment.transform.localPosition = equipment.HoldingPos;
                    }
                    else
                    {
                        equipment.transform.parent = ShieldHoldPos;
                        equipment.transform.localRotation = Quaternion.identity;
                        equipment.transform.localScale = Vector3.one;
                        equipment.transform.localPosition = equipment.HoldingPos;
                    }
                    return true;
            }
        }
        return false;
    }
}