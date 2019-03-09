using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]GameObject PopOutObject = null;
    [SerializeField]int numObjectPopout = 3;
    [SerializeField]Animator chestAnim = null;
    private BoxCollider m_boxcolider = null;
    bool isPopOut = false;
    void Awake()
    {
        m_boxcolider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        if(isPopOut)return;
        var hitInfo = PhysicsExtensions.OverlapBox(m_boxcolider,LayerMask.GetMask("Character"));

        if(hitInfo.Length > 0)
        {
            if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                isPopOut =true;
                chestAnim.SetTrigger("Open");
                for(int i =0;i<numObjectPopout;i++)
                {
                    var temp = Instantiate(PopOutObject,transform.position+Vector3.up,Quaternion.identity);
                    temp.GetComponent<Acorn>().PopOut(Random.value,Random.value,5f);
                }
                Destroy(this.gameObject,2f);
            }
        }
    }
}
