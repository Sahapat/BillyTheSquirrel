using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour,IPopable
{
    [SerializeField] int coinValue = 1;

    public void PopOut(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        throw new System.NotImplementedException();
    }
}
