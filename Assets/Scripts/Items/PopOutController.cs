using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopOutController : MonoBehaviour
{
    [SerializeField]GameObject target = null;
    [SerializeField]Transform placeTranform = null;
    [SerializeField]Vector3 rotateScale = Vector3.zero;
    [SerializeField]GameObject toolTip = null;


    private Vector3 initPosition = Vector3.zero;
    private Vector3 finalPosition = Vector3.zero;

    private bool isPopOut = false;
    private bool isFinishPopOut = false;
    void FixedUpdate()
    {
        toolTip.SetActive(false);
        isFinishPopOut = (isPopOut && transform.position == finalPosition);

        if(isFinishPopOut)
        {
            placeTranform.Rotate(rotateScale,Space.World);
        }
    }

    public void ShowToolTip()
    {
        toolTip.SetActive(true);
    }
    public void PopOut(GameObject targetObject,Vector3 startPosition,Vector3 endPosition,bool isInstantiate,bool isAutoCollect)
    {   
        GameObject itemTarget = null;
        if(isInstantiate)
        {
            itemTarget = Instantiate(targetObject,Vector3.zero,Quaternion.identity);
        }
        else
        {
            itemTarget = targetObject;
        }
        target = itemTarget;
        target.transform.parent = placeTranform;
        target.transform.localPosition = Vector3.zero;
        target.transform.localRotation = Quaternion.Euler(0,0,30);
        target.transform.localScale = Vector3.one;

        isPopOut = true;
        initPosition = startPosition;
        finalPosition = endPosition;

        if(isAutoCollect)this.GetComponent<BoxCollider>().enabled = false;

        transform.position = initPosition;
        transform.DOJump(finalPosition,1.2f,1,0.75f,false);
    }
}
