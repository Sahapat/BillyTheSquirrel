using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform weaponHoldPos = null;
    [SerializeField] int health = 100;
    [SerializeField] int stemina = 100;

    private CapsuleCollider m_capsuleColider = null;

    void Awake()
    {
        m_capsuleColider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        #region PickUpChecker
        var hitInfo = PhysicsExtensions.OverlapCapsule(m_capsuleColider,LayerMask.GetMask("PickUp"));
        if(hitInfo.Length>0 && (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Joystick1Button3)))
        {
            var m_pickup = hitInfo[0].GetComponent<PickUp>();
            if(hitInfo[0].CompareTag("Item"))
            {
                var weaponPick = m_pickup.PickObjUp();
                Destroy(weaponPick);
            }
            else if(hitInfo[0].CompareTag("Weapon"))
            {
                var weaponPick = m_pickup.PickObjUp();
                weaponPick.transform.rotation = Quaternion.identity;
                weaponPick.transform.parent = weaponHoldPos;
                weaponPick.transform.localPosition = Vector3.zero;
                weaponPick.transform.localRotation = Quaternion.identity;
            }
        }
        #endregion

        if(Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.F))
        {
            GameCore.m_inventoryController.InventorySwitch();
        }
        GameCore.m_uiHandler.UpdateTxtHealth(health);
        GameCore.m_uiHandler.UpdateTxtStemina(stemina);
    }
}
