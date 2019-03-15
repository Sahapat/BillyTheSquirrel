using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventHandler : MonoBehaviour
{
    [Header("Action Properties")]
    [SerializeField]float attack1ScaleToAdd = 250f;
    [SerializeField]float attack2ScaleToAdd = 250f;
    [SerializeField]float attack3ScaleToAdd = 265f;
    [SerializeField]float HeavyAttackScaleToAdd = 300f;
    bool isSet = false;
    Vector3 destination = Vector3.zero;
    void FixedUpdate()
    {
        if(!isSet)return;
        transform.position = Vector3.MoveTowards(transform.position,destination,0.36f);
        if(transform.position == destination)
        {
            isSet = false;
        }
    }
    public void Attack1()
    {
        isSet = true;
        var forwardPos = transform.forward;
        destination = new Vector3(transform.position.x+(forwardPos.x*Time.deltaTime*attack1ScaleToAdd),transform.position.y,transform.position.z+(forwardPos.z*Time.deltaTime*attack1ScaleToAdd));
    }
    public void Attack2()
    {
        isSet = true;
        var forwardPos = transform.forward;
        destination = new Vector3(transform.position.x+(forwardPos.x*Time.deltaTime*attack2ScaleToAdd),transform.position.y,transform.position.z+(forwardPos.z*Time.deltaTime*attack2ScaleToAdd));
    }
    public void Attack3()
    {
        isSet = true;
        var forwardPos = transform.forward;
        destination = new Vector3(transform.position.x+(forwardPos.x*Time.deltaTime*attack3ScaleToAdd),transform.position.y,transform.position.z+(forwardPos.z*Time.deltaTime*attack3ScaleToAdd));
    }
    public void HeavyAttack()
    {
        isSet = true;
        var forwardPos = transform.forward;
        destination = new Vector3(transform.position.x+(forwardPos.x*Time.deltaTime*HeavyAttackScaleToAdd),transform.position.y,transform.position.z+(forwardPos.z*Time.deltaTime*HeavyAttackScaleToAdd));
    }
}
