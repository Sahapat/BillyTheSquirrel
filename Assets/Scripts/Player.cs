using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]BoxCollider m_Boxcolider = null;
    [SerializeField]Transform grabPos = null;
    [SerializeField]Transform grabRef = null;
    void Update()
    {
        var pickUpCheckers = Physics.OverlapBox(m_Boxcolider.transform.position,m_Boxcolider.size,Quaternion.identity,LayerMask.GetMask("PickUp"));

        if(!GameCore.m_floatingJoystick)
        {
            if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                var temp = pickUpCheckers[0].GetComponent<PickUo>().PickObjUo();
                grabPos.transform.position = grabPos.position;
                temp.transform.parent = grabPos.transform;
                temp.transform.localPosition =grabRef.transform.localPosition;
                temp.transform.localRotation = grabRef.transform.localRotation;
            }
        }
    }
}
