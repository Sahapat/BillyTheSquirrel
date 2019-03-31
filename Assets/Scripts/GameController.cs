using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Player ClientPlayerTarget = null;
    [SerializeField] Transform temporaryTranform = null;
    [SerializeField] float ClampPlayerByYPosition = 0f;
    
    void FixedUpdate()
    {
        //Check death
        if(ClientPlayerTarget.isDead)
        {
            GameCore.m_uiHandler.ShowGameOver();
        }
        else
        {
            GameCore.m_uiHandler.CloseGameOver();
        }
        //Clamp player when fall
        if(ClientPlayerTarget.transform.position.y < ClampPlayerByYPosition)
        {
            var spawnPos = ClientPlayerTarget.CurrentGround.position;
            spawnPos = new Vector3(spawnPos.x,spawnPos.y + 10,spawnPos.z);
            ClientPlayerTarget.transform.position = spawnPos;
            ClientPlayerTarget.GetComponent<Rigidbody>().velocity= Vector3.zero;
        }
    }
    public Player GetClientPlayerTarget()
    {
        return ClientPlayerTarget;
    }
    public Transform GetTemporaryTranform()
    {
        return temporaryTranform;
    }
}
