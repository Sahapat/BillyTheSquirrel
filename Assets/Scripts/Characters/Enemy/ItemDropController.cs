using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField]PopOutProperty[] itemsPopOut = null;

    public void DoDrop()
    {
        for (int i = 0; i < itemsPopOut.Length; i++)
        {
            if (itemsPopOut[i].GetPopOutChance())
            {
                var numToPopOut = itemsPopOut[i].GetNumPopOut();
                for (int j = 0; j < numToPopOut; j++)
                {
                    var PopOutObject = Instantiate(GameCore.m_GameContrller.PopOutPrefab, Vector3.zero, Quaternion.identity);

                    var PopOutController = PopOutObject.GetComponent<PopOutController>();

                    var circleRandom = Random.insideUnitCircle;

                    var endPosition = new Vector3(transform.position.x+circleRandom.x,transform.position.y+2,transform.position.z+circleRandom.y);

                    var rayCastHit = Physics.Raycast(endPosition,Vector3.down,Mathf.Infinity,LayerMask.GetMask("Ground"));

                    if(rayCastHit)
                    {
                        endPosition.y = transform.position.y+0.25f;
                    }
                    else
                    {
                        endPosition = GetComponent<Enemy>().CurrentGround.position;
                    }

                    if (itemsPopOut[i].objectToPopOut.CompareTag("Coin"))
                    {

                        PopOutController.PopOut(itemsPopOut[i].objectToPopOut, transform.position, endPosition, true, true);
                    }
                    else
                    {
                        PopOutController.PopOut(itemsPopOut[i].objectToPopOut, transform.position, endPosition, true, false);
                    }
                }
            }
        }
    }
}
