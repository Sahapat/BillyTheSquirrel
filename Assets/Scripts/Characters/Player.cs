using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] int m_characterMaxHP = 100;

    [Header("Action Stamina Depletion")]
    [SerializeField] int NormalAttack = 20;
    [SerializeField] int HeavyAttack = 40;
    [SerializeField] int Dash = 20;
    [Header("Inventory Properties")]
    [SerializeField]int WeaponSlot =2;
    [SerializeField]int ItemSlot = 8;

    public Health CharacterHP { get; private set; }
    public Stemina CharacterStemina { get; private set; }
    public Inventory WeaponInventory {get;private set;}
    public Inventory ItemInventory{get;private set;}

    private CapsuleCollider m_capsuleColider = null;
    private StateHandler m_stateHandler = null;
    private Vector3 movement = Vector3.zero;
    void Awake()
    {
        CharacterHP = new Health(m_characterMaxHP);
        CharacterStemina = GetComponent<Stemina>();
        m_capsuleColider = GetComponent<CapsuleCollider>();
        m_stateHandler = GetComponent<StateHandler>();
        WeaponInventory = new Inventory(WeaponSlot);
        ItemInventory = new Inventory(ItemSlot);
    }
    void Start()
    {
        CharacterHP.OnHPChanged += CheckHealth;
    }
    void Update()
    {
        MovementInputGetter();
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            GameCore.m_GameContrller.SwitchAvtiveInventory();
        }

        if(GameCore.m_uiHandler.GetInventoryStatus())
        {
            m_stateHandler.MovementSetter(SerializeInputByCameraTranform(Vector3.zero));
            return;
        }

        m_stateHandler.MovementSetter(SerializeInputByCameraTranform(movement));

        if (NormalAttackGetter() && CheckNormalAttackSP())
        {
            if(m_stateHandler.NormalAttack())
            {
                CharacterStemina.RemoveSP(NormalAttack);
            }
        }
        if (HeavyAttackGetter() && CheckHeavyAttackSP())
        {
            if(m_stateHandler.HeavyAttack())
            {
               CharacterStemina.RemoveSP(HeavyAttack);
            }
        }
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button1))&&CheckDashSP())
        {
           if( m_stateHandler.Dash())
           {
               CharacterStemina.RemoveSP(Dash);
           }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            m_stateHandler.Jump();
        }
    }
    public void TakeDamage(int damage)
    {
        CharacterHP.RemoveHP(damage);
        m_stateHandler.Hurt();
    }

    public void Heal(int healValue)
    {
        CharacterHP.AddHP(healValue);
        m_stateHandler.UsePotion();
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
        return CharacterStemina.SP >=Dash;
    }
    bool NormalAttackGetter()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetMouseButtonDown(0);
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
        if(value <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    Vector3 SerializeInputByCameraTranform(Vector3 inputAxis)
    {
        Vector3 newForward = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized * inputAxis.y;
        Vector3 newRight = -Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized * inputAxis.x;
        Vector3 direction = newForward + newRight;
        return new Vector3(direction.x, direction.z);
    }
}