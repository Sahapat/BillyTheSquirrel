using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] PopOutProperty[] PopOutObjectsProperty = null;
    [SerializeField] Transform[] rangesToPopOut = null;
    [SerializeField] GameObject toolTip = null;
    private BoxCollider m_boxcolider = null;
    private Animator m_animator = null;

    void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_boxcolider = GetComponent<BoxCollider>();
    }
    void Update()
    {
        if (m_boxcolider.enabled)
        {
            if (this.gameObject?.GetInstanceID() == GameCore.m_GameContrller.itemFocus?.GetInstanceID())
            {
                toolTip.SetActive(true);
            }
            else
            {
                toolTip.SetActive(false);
            }
        }
    }
    public void OpenChest()
    {
        m_animator.SetTrigger("Open");
        Invoke("PopOutObject", 0.35f);
    }
    void PopOutObject()
    {
        m_boxcolider.enabled =false;
        for (int i = 0; i < PopOutObjectsProperty.Length; i++)
        {
            if (PopOutObjectsProperty[i].GetPopOutChance())
            {
                var numToPopOut = PopOutObjectsProperty[i].GetNumPopOut();
                for (int j = 0; j < numToPopOut; j++)
                {
                    var PopOutObject = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                    var PopOutController = PopOutObject.GetComponent<PopOutController>();

                    Vector3 endPosition = new Vector3(Random.Range(rangesToPopOut[0].position.x, rangesToPopOut[1].position.x), transform.position.y + 0.25f, Random.Range(rangesToPopOut[0].position.z, rangesToPopOut[1].position.z));

                    if (PopOutObjectsProperty[i].objectToPopOut.CompareTag("Coin"))
                    {

                        PopOutController.PopOut(PopOutObjectsProperty[i].objectToPopOut, transform.position, endPosition, true, true);
                    }
                    else
                    {
                        PopOutController.PopOut(PopOutObjectsProperty[i].objectToPopOut, transform.position, endPosition, true, false);
                    }
                }
            }
        }
    }
}
