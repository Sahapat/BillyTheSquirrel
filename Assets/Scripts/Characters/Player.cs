using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAttackable
{
    [Header("CharacterProperties")]
    [SerializeField] int m_characterMaxHP = 100;
    [SerializeField] LayerMask targetLayer = 0;

    [Header("Action Stamina Depletion")]
    [SerializeField] int Jump = 20;
    [SerializeField] int Dash = 20;
    [SerializeField] int holdingShield = 10;
    [Header("Other")]
    [SerializeField] Transform SwordHoldPosition = null;
    [SerializeField] BaseWeapon weaponInHand = null;
    [SerializeField] Transform ShieldHoldPosition = null;
    [SerializeField] BaseShield shieldInHand = null;
    [SerializeField] Transform PotionPosition = null;

    public bool isDead { get; private set; }
    public bool isBlocking { get; private set; }
    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }
    public BaseWeapon WeaponInventory { get { return weaponInHand; } }
    public BaseShield ShieldInvetory { get { return shieldInHand; } }
    public Inventory ItemInventory { get; private set; }
    public Transform CurrentGround { get; private set; }
    public Coin CharacterCoin { get; private set; }

    private CapsuleCollider m_capsuleColider = null;
    private Rigidbody m_rigidbody = null;
    private StateHandler m_stateHandler = null;
    private ActionHandler m_actionHandler = null;
    private RagdollController m_ragdollController = null;

    private Vector3 movement = Vector3.zero;

    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        CharacterCoin = new Coin();
        ItemInventory = new Inventory(8);
        CharacterStemina = GetComponent<Stemina>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
        m_stateHandler = GetComponent<StateHandler>();
        m_actionHandler = GetComponent<ActionHandler>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_ragdollController = GetComponent<RagdollController>();
    }
    void Start()
    {
        CharacterHP.OnHPChanged += CheckHealth;
        weaponInHand.SetTargetLayer(targetLayer);
        m_ragdollController.InActiveRagdoll();
        if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
        {
            shieldInHand.gameObject.SetActive(false);
        }
        else
        {
            shieldInHand.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        isBlocking = m_stateHandler.GetBool("isHoldingShield");
    }
    void FixedUpdate()
    {
        if (isDead || !GameCore.m_GameContrller.Controlable) return;
        bool InventoryStatus = GameCore.m_uiHandler.GetInventoryStatus();

        ItemCollectChecker();
        CheckForUpdateNewLastPosition();

        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            if (GameCore.m_uiHandler.currentItemIndex != -1)
            {
                m_stateHandler.UsePotion();
                SwordHoldPosition.gameObject.SetActive(false);
                ShieldHoldPosition.gameObject.SetActive(false);
                var itemIndex = GameCore.m_uiHandler.currentItemIndex - 2;
                if (itemIndex >= 0)
                {
                    var itemUse = ItemInventory.itemInEndPoint[itemIndex];
                    itemUse.transform.parent = PotionPosition;
                    itemUse.transform.localPosition = Vector3.zero;
                    itemUse.transform.localRotation = Quaternion.identity;
                    itemUse.transform.localScale = Vector3.one;
                    Invoke("UseItem", 1.2f);
                    Destroy(itemUse, 1.2f);
                }
            }
        }

        //Check Input for Open&Close InventoryHub
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            if (InventoryStatus)
            {
                GameCore.m_uiHandler.CloseInventory();
            }
            else
            {
                GameCore.m_uiHandler.OpenInventory();
            }
        }

        //Condition to do action due with the inventoryHub status
        if (InventoryStatus)
        {
            m_stateHandler.MovementSetter(SerializeInputByCameraTranform(Vector2.zero));
        }
        else
        {
            MovementInputGetter();
            m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));
            if (NormalAttackGetter() && CheckNormalAttackSP())
            {
                if (GameCore.m_GameContrller.TargetToLockOn)
                {
                    transform.LookAt(GameCore.m_GameContrller.TargetToLockOn.transform);
                }
                if (m_stateHandler.NormalAttack())
                {
                    var indexToUse = 0;
                    if (m_stateHandler.GetBool("CanAttack2"))
                    {
                        indexToUse = 1;
                    }
                    else if (m_stateHandler.GetBool("CanAttack3"))
                    {
                        indexToUse = 2;
                    }
                    CharacterStemina.RemoveSP(weaponInHand.GetNormalSteminaDeplete(indexToUse));
                }
            }
            if (HeavyAttackGetter() && CheckHeavyAttackSP())
            {
                if (m_stateHandler.HeavyAttack())
                {
                    CharacterStemina.RemoveSP(weaponInHand.GetHeavySteminaDeplete());
                    switch (weaponInHand.weaponType)
                    {
                        case WeaponType.SHIELD_AND_SWORD:
                            GetComponentInChildren<HeavyHitForSword>()?.ActiveTrail();
                            break;
                        case WeaponType.SPEAR:
                            GetComponentInChildren<HeavyHitForSword>()?.ActiveTrail();
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                m_stateHandler.SetBool("isHoldingShieldStart", true);
            }
            if (HoldingShieldGetter() && CheckHoldingShieldSP())
            {
                m_stateHandler.SetHoldingShield();
            }
            else
            {
                m_stateHandler.SetUnHoldingShield();
                m_stateHandler.SetBool("isHoldingShieldStart", false);
            }
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton1)) && CheckDashSP())
            {
                if (m_stateHandler.Dash())
                {
                    CharacterStemina.RemoveSP(Dash);
                }
            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && CheckJumpSP())
            {
                m_stateHandler.Jump();
                CharacterStemina.RemoveSP(Jump);
            }
        }
    }
    bool CheckNormalAttackSP()
    {
        var indexToUse = 0;
        if (m_stateHandler.GetBool("CanAttack2"))
        {
            indexToUse = 1;
        }
        else if (m_stateHandler.GetBool("CanAttack3"))
        {
            indexToUse = 2;
        }
        return CharacterStemina.SP >= weaponInHand.GetNormalSteminaDeplete(indexToUse);
    }
    bool CheckHeavyAttackSP()
    {
        return CharacterStemina.SP >= weaponInHand.GetHeavySteminaDeplete();
    }
    bool CheckDashSP()
    {
        return CharacterStemina.SP >= Dash;
    }
    bool CheckJumpSP()
    {
        return CharacterStemina.SP >= Jump;
    }
    bool CheckHoldingShieldSP()
    {
        return CharacterStemina.SP >= holdingShield;
    }
    bool NormalAttackGetter()
    {
        return Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetMouseButtonDown(0);
    }
    bool HeavyAttackGetter()
    {
        var TriggerAxis = Input.GetAxis("JoystickTrigger");
        return TriggerAxis > 0 || Input.GetMouseButtonDown(1);
    }
    bool HoldingShieldGetter()
    {
        return Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.LeftControl);
    }
    void MovementInputGetter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    void CheckHealth(int value)
    {
        if (value <= 0)
        {
            isDead = true;
            m_stateHandler.SetBool("isDead", true);
            m_ragdollController.ActiveRagdoll(m_rigidbody.velocity);
        }
    }
    void UseItem()
    {
        var itemIndex = GameCore.m_uiHandler.currentItemIndex - 2;
        if (itemIndex >= 0)
        {
            var itemUse = ItemInventory.itemInEndPoint[itemIndex].GetComponent<Item>();
            itemUse.Use(this);
            ItemInventory.RemoveItem(itemIndex);
            GameCore.m_uiHandler.RemoveCurrentItem();
            SwordHoldPosition.gameObject.SetActive(true);
            ShieldHoldPosition.gameObject.SetActive(true);
        }
    }
    void CheckForUpdateNewLastPosition()
    {
        var hitInfo = PhysicsExtensions.OverlapCapsule(m_capsuleColider, LayerMask.GetMask("Ground"));
        if (hitInfo.Length > 0)
        {
            foreach (var temp in hitInfo)
            {
                if (CurrentGround)
                {
                    if (temp.GetInstanceID() != CurrentGround.GetInstanceID())
                    {
                        CurrentGround = temp.transform;
                    }
                }
                else
                {
                    CurrentGround = temp.transform;
                }
            }
        }
    }
    void ItemCollectChecker()
    {
        var hitInfo = PhysicsExtensions.OverlapCapsule(m_capsuleColider, LayerMask.GetMask("PickUp"));

        if (hitInfo.Length > 0)
        {
            print("in");
            if (hitInfo[0].CompareTag("Chest"))
            {
                var chestScript = hitInfo[0].GetComponent<Chest>();
                chestScript.ShowToolTip();
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
                {
                    chestScript.OpenChest();
                }
            }
            /* var collectItem = hitInfo[0].GetComponent<ICollectable>();
            switch (collectItem.itemType)
            {
                case ItemType.EQUIPMENT:
                    var weapon = hitInfo[0].GetComponent<BaseWeapon>();
                    var shield = hitInfo[0].GetComponent<BaseShield>();

                    if (weapon != null)
                    {
                        weaponInHand.Discard();
                        var weaponObject = weapon.PickUp();
                        weaponInHand = weapon;
                        weaponObject.transform.parent = SwordHoldPosition;
                        weaponObject.transform.localPosition = weapon.HoldingPos;
                        weaponObject.transform.localRotation = Quaternion.identity;
                        GameCore.m_uiHandler.UpdateEquipmentSlot();
                        m_actionHandler.UpdateSword(weapon);
                    }
                    else
                    {
                        shieldInHand.Discard();
                        var shieldObject = shield.PickUp();
                        shieldInHand = shield;
                        shieldObject.transform.parent = ShieldHoldPosition;
                        shieldObject.transform.localPosition = shield.HoldingPos;
                        shieldObject.transform.localRotation = Quaternion.identity;
                        GameCore.m_uiHandler.UpdateEquipmentSlot();
                    }
                    break;
                case ItemType.ITEM:
                    if (!ItemInventory.isFull)
                    {
                        ItemInventory.AddItem(hitInfo[0].gameObject, GameCore.m_GameContrller.TemporaryTranform);
                    }
                    break;
            } */
        }
    }
    Vector3 SerializeInputByCameraTranform(Vector3 inputAxis)
    {
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized * inputAxis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized * inputAxis.x;
        Vector3 direction = newForward + newRight;
        return new Vector3(direction.x, direction.z);
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        if (damage != 0)
        {
            m_stateHandler.SetUnHoldingShield();
            m_stateHandler.Hurt();
        }
    }
    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        m_rigidbody.velocity = forceToAdd;
        if (damage != 0)
        {
            m_stateHandler.SetUnHoldingShield();
            m_stateHandler.Hurt();
        }
    }
    public void Stop()
    {
        m_stateHandler.MovementSetter(Vector3.zero);
    }
}