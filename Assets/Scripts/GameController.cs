using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Player ClientPlayerTarget = null;
    public Player GetClientPlayerTarget()
    {
        return ClientPlayerTarget;
    }
}
