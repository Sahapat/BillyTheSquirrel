using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAttackable
{
    [Header("CharacterProperties")]
    [SerializeField] int m_characterMaxHP = 100;

    [Header("Action Stamina Depletion")]
    [SerializeField] int NormalAttack = 20;
    [SerializeField] int HeavyAttack = 40;
    [SerializeField] int Jump = 20;
    [SerializeField] int Dash = 20;
    [Header("Other")]
    [SerializeField] Transform SwordHoldPosition = null;
    [SerializeField] BaseSword swordInHand = null;
    [SerializeField] Transform ShieldHoldPosition = null;
    [SerializeField] BaseShield shieldInHand = null;
    [SerializeField] Transform PotionPosition = null;

    public bool isDead { get; private set; }
    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }
    public BaseSword WeaponInventory { get { return swordInHand; } }
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
    private bool canCancelToHurt = true;

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
    }
    void Update()
    {
        MovementInputGetter();
    }
    void FixedUpdate()
    {
        if (isDead) return;
        ItemCollectChecker();
        CheckForUpdateNewLastPosition();
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            GameCore.m_GameContrller.SwitchActiveInventory();
        }

        if (GameCore.m_uiHandler.GetInventoryStatus())
        {
            m_stateHandler.MovementSetter(SerializeInputByCameraTranform(Vector3.zero));
            return;
        }

        m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));

        if (NormalAttackGetter() && CheckNormalAttackSP())
        {
            if (m_stateHandler.NormalAttack())
            {
                CharacterStemina.RemoveSP(NormalAttack);
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
            m_stateHandler.Jump();
            CharacterStemina.RemoveSP(Jump);
        }
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
    }
    bool CheckNormalAttackSP()
    {
        return CharacterStemina.SP >= NormalAttack;
    }
    bool CheckHeavyAttackSP()
    {
        return CharacterStemina.SP >= HeavyAttack;
    }
    bool CheckDashSP()
    {
        return CharacterStemina.SP >= Dash;
    }
    bool CheckJumpSP()
    {
        return CharacterStemina.SP >= Jump;
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
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                var collectItem = hitInfo[0].GetComponent<ICollectable>();
                switch (collectItem.itemType)
                {
                    case ItemType.EQUIPMENT:
                        var weapon = hitInfo[0].GetComponent<BaseSword>();
                        var shield = hitInfo[0].GetComponent<BaseShield>();

                        if (weapon != null)
                        {
                            swordInHand.Discard();
                            var weaponObject = weapon.PickUp();
                            swordInHand = weapon;
                            weaponObject.transform.parent = SwordHoldPosition;
                            weaponObject.transform.localPosition = weapon.HoldingPos;
                            weaponObject.transform.localRotation = Quaternion.identity;
                            m_actionHandler.UpdateSword(weapon);
                            GameCore.m_GameContrller.UpdateEquipmentSlot();
                        }
                        else
                        {
                            shieldInHand.Discard();
                            var shieldObject = shield.PickUp();
                            shieldInHand = shield;
                            shieldObject.transform.parent = ShieldHoldPosition;
                            shieldObject.transform.localPosition = shield.HoldingPos;
                            shieldObject.transform.localRotation = Quaternion.identity;
                            GameCore.m_GameContrller.UpdateEquipmentSlot();
                        }
                        break;
                    case ItemType.ITEM:
                        if (!ItemInventory.isFull)
                        {
                            ItemInventory.AddItem(hitInfo[0].gameObject, GameCore.m_GameContrller.GetTemporaryTranform());
                        }
                        break;
                }
            }
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
        if (canCancelToHurt)
        {
            m_stateHandler.Hurt();
        }
    }
    public void TakeDamage(int damage, Vector3 forceToAdd)
    {
        CharacterHP.RemoveHP(damage);
        if (canCancelToHurt)
        {
            m_rigidbody.velocity = forceToAdd;
            m_stateHandler.Hurt();
        }
    }
}