using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirBallon : MonoBehaviour
{
    [SerializeField] int coinRequire = 0;
    [SerializeField] GameObject acornObj = null;
    [SerializeField] GameObject requireObj = null;
    [SerializeField] TextMesh coinTxtRequire = null;
    [SerializeField] BoxCollider blockNotFinish = null;

    bool isFinishRequire = false;
    bool finishTrigger = false;
    float esclapForInt = 0;
    float esclapForInstance = 0;


    private Animator m_animator = null;
    private BoxCollider checkerColider = null;

    private WaitForSeconds second = null;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        checkerColider = GetComponent<BoxCollider>();
        second = new WaitForSeconds(0.1f);
    }
    void Start()
    {
        coinTxtRequire.text = coinRequire.ToString();
    }
    void FixedUpdate()
    {
        var hitInfo = PhysicsExtensions.OverlapBox(checkerColider, LayerMask.GetMask("Character"));

        if (hitInfo.Length > 0)
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton3))
            {
                AddCoin();
            }
        }
        if (isFinishRequire && !finishTrigger)
        {
            finishTrigger = true;
            m_animator.SetTrigger("Open");
            blockNotFinish.enabled = false;
        }
        esclapForInt += Time.deltaTime;
        esclapForInstance += Time.deltaTime;
        coinTxtRequire.text = coinRequire.ToString();
    }
    void DisableRequireObj()
    {
        requireObj.SetActive(false);
    }
    void AddCoin()
    {
        if (coinRequire == 0 && !finishTrigger)
        {
            isFinishRequire = true;
            FindObjectOfType<Map1>().SetCurrentQuest(6);
            Invoke("DisableRequireObj", 1.5f);
            
        }
        if (!isFinishRequire)
        {
            if (esclapForInt >= 0.1f)
            {
                esclapForInt = 0;
                GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.RemoveCoin(1);
                if (GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin >= 0)
                {
                    coinRequire--;
                    if (esclapForInstance >= 0.5f)
                    {
                        esclapForInstance = 0;
                        var temp = Instantiate(acornObj, Vector3.zero, Quaternion.identity);
                        temp.transform.position = GameCore.m_GameContrller.ClientPlayerTarget.transform.position;
                        temp.transform.DOJump(acornObj.transform.position, 1.2f, 1, 0.5f, false);
                        Destroy(temp.gameObject, 1.1f);
                    }
                }
                else
                {
                    GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin = 0;
                }
            }
        }
    }
}
