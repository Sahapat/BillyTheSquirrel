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
    public int killEnemyCount = 0;
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
        m_ragdollController.InActiveRagdoll();
        if (weaponInHand)
        {
            weaponInHand.SetTargetLayer(targetLayer);
            if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
            {
                shieldInHand.gameObject.SetActive(false);
            }
            else
            {
                shieldInHand.gameObject.SetActive(true);
            }
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
            if (shieldInHand)
            {
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
                if (m_stateHandler.Jump())
                {
                    CharacterStemina.RemoveSP(Jump);
                }
            }
            MovementInputGetter();
            m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));
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
        return CharacterStemina.SP >= weaponInHand?.GetNormalSteminaDeplete(indexToUse);
    }
    bool CheckHeavyAttackSP()
    {
        return CharacterStemina.SP >= weaponInHand?.GetHeavySteminaDeplete();
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
    bool Interact()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3);
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
            GameCore.m_GameContrller.itemFocus = hitInfo[0].gameObject;
            switch (hitInfo[0].tag)
            {
                case "Chest":
                    if (Interact())
                    {
                        var chestScript = hitInfo[0].GetComponent<Chest>();
                        chestScript.OpenChest();
                    }
                    break;
                case "Equipment":
                    if (Interact())
                    {
                        var popoutControllerScript = hitInfo[0].GetComponent<PopOutController>();

                        var equipemnt = popoutControllerScript.PickUp();
                        var baseWeapon = equipemnt.GetComponent<BaseWeapon>();
                        var baseShield = equipemnt.GetComponent<BaseShield>();

                        if (baseWeapon)
                        {
                            if (weaponInHand)
                            {
                                var circleRandom = Random.insideUnitCircle;

                                var endPosition = new Vector3(transform.position.x + circleRandom.x + 0.5f, transform.position.y + 0.25f, transform.position.z + 0.5f + circleRandom.y);

                                var popOutObj = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                                var popOutController = popOutObj.GetComponent<PopOutController>();

                                popOutController.PopOut(weaponInHand.gameObject, transform.position, endPosition, true, false);

                                Destroy(weaponInHand.gameObject);
                            }
                            baseWeapon.transform.parent = SwordHoldPosition;
                            baseWeapon.transform.localPosition = baseWeapon.HoldingPos;
                            baseWeapon.transform.localRotation = Quaternion.identity;
                            weaponInHand = baseWeapon;

                            weaponInHand.SetTargetLayer(targetLayer);
                            if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
                            {
                                shieldInHand?.gameObject.SetActive(false);
                            }
                            else
                            {
                                shieldInHand?.gameObject?.SetActive(true);
                            }
                            m_stateHandler.UpdateWeapon(baseWeapon);
                            GameCore.m_uiHandler.UpdateEquipmentSlot();
                            Destroy(hitInfo[0].gameObject);
                        }
                        else if (baseShield)
                        {
                            if (shieldInHand)
                            {
                                var circleRandom = Random.insideUnitCircle;

                                var endPosition = new Vector3(transform.position.x + circleRandom.x + 0.5f, transform.position.y + 0.25f, transform.position.z + 0.5f + circleRandom.y);

                                var popOutObj = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                                var popOutController = popOutObj.GetComponent<PopOutController>();

                                popOutController.PopOut(shieldInHand.gameObject, transform.position, endPosition, true, false);

                                Destroy(shieldInHand.gameObject);
                            }
                            baseShield.transform.parent = ShieldHoldPosition;
                            baseShield.transform.localPosition = baseShield.HoldingPos;
                            baseShield.transform.localRotation = Quaternion.identity;

                            shieldInHand = baseShield;
                            if (weaponInHand)
                            {
                                if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
                                {
                                    shieldInHand?.gameObject.SetActive(false);
                                }
                                else
                                {
                                    shieldInHand?.gameObject?.SetActive(true);
                                }
                            }
                            GameCore.m_uiHandler.UpdateEquipmentSlot();
                            Destroy(hitInfo[0].gameObject);
                        }
                    }
                    break;
                case "Item":
                    if (Interact())
                    {
                        var popoutControllerScript = hitInfo[0].GetComponent<PopOutController>();

                        var item = popoutControllerScript.PickUp();

                        ItemInventory.AddItem(item, GameCore.m_GameContrller.TemporaryTranform);
                        Destroy(hitInfo[0].gameObject);
                    }
                    break;
            }
        }
        else
        {
            GameCore.m_GameContrller.itemFocus = null;
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
    public void AddEquipment(BaseWeapon weapon, BaseShield shield)
    {
        if (weapon)
        {
            if (weaponInHand)
            {
                var circleRandom = Random.insideUnitCircle;

                var endPosition = new Vector3(transform.position.x + circleRandom.x, transform.position.y + 0.25f, transform.position.z + circleRandom.y);

                var popOutObj = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                var popOutController = popOutObj.GetComponent<PopOutController>();

                popOutController.PopOut(weaponInHand.gameObject, transform.position, endPosition, true, false);

                Destroy(weaponInHand.gameObject);
            }
            weapon.transform.parent = SwordHoldPosition;
            weapon.transform.localPosition = weapon.HoldingPos;
            weapon.transform.localRotation = Quaternion.identity;
            weaponInHand = weapon;

            weaponInHand.SetTargetLayer(targetLayer);
            if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
            {
                shieldInHand?.gameObject.SetActive(false);
            }
            else
            {
                shieldInHand?.gameObject?.SetActive(true);
            }
            m_stateHandler.UpdateWeapon(weapon);
            GameCore.m_uiHandler.UpdateEquipmentSlot();
        }
        else if (shield)
        {
            if (shieldInHand)
            {
                var circleRandom = Random.insideUnitCircle;

                var endPosition = new Vector3(transform.position.x + circleRandom.x, transform.position.y + 0.25f, transform.position.z + circleRandom.y);

                var popOutObj = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                var popOutController = popOutObj.GetComponent<PopOutController>();

                popOutController.PopOut(shieldInHand.gameObject, transform.position, endPosition, true, false);

                Destroy(shieldInHand.gameObject);
            }
            shield.transform.parent = ShieldHoldPosition;
            shield.transform.localPosition = shield.HoldingPos;
            shield.transform.localRotation = Quaternion.identity;

            shieldInHand = shield;
            if (weaponInHand)
            {
                if (weaponInHand.weaponType == WeaponType.GREAT_SWORD)
                {
                    shieldInHand?.gameObject.SetActive(false);
                }
                else
                {
                    shieldInHand?.gameObject?.SetActive(true);
                }
            }
            GameCore.m_uiHandler.UpdateEquipmentSlot();
        }
    }
    public void SetDrink(int maxHP,int maxSP,GameObject Obj)
    {
        m_stateHandler.UsePotion();
        GameCore.m_GameContrller.Controlable = false;
        CharacterHP.SetMaxHP(CharacterHP.HP+maxHP);
        CharacterStemina.SetMaxSP(CharacterStemina.SP + maxSP);
        GameCore.m_GameContrller.Controlable = true;
        SwordHoldPosition?.gameObject.SetActive(false);
        shieldInHand?.gameObject.SetActive(false);
        Obj.transform.parent = PotionPosition;
        Obj.transform.localPosition = Vector3.zero;
        Obj.transform.localRotation = Quaternion.identity;
        Obj.transform.localScale = Vector3.one;

        if(maxHP != 0)
        {
            Invoke("UpdateMaxHP",0.8f);
        }
        if(maxSP != 0)
        {
            Invoke("UpdateMaxSP",0.8f);
        }
        Destroy(Obj,0.8f);
    }
    void UpdateMaxHP()
    {
        GameCore.m_uiHandler.UpdateHPMax();
        SwordHoldPosition?.gameObject.SetActive(true);
        shieldInHand?.gameObject.SetActive(true);
    }
    void UpdateMaxSP()
    {
        GameCore.m_uiHandler.UpdateSPMax();
        SwordHoldPosition?.gameObject.SetActive(true);
        shieldInHand?.gameObject.SetActive(true);
    }
}