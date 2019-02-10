using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitSystem
{
    float delayForActive{get;}
    float delayForInActive{get;}
    void ActiveHit();
    void CancelHit();
}
