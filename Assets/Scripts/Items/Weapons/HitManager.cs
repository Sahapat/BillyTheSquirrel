using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    [SerializeField]IHitSystem[] NormalHits;
    [SerializeField]IHitSystem HeavyHit;

    public void ActiveNormalHit(int index)
    {
        NormalHits[index].ActiveHit();
    }
    public void CancelNormalHit(int index)
    {
        NormalHits[index].CancelHit();
    }
    public void CancelHeavyHit()
    {
        HeavyHit.CancelHit();
    }
    public void CancelAllHit()
    {
        CancelHeavyHit();
        for(int i=0;i<NormalHits.Length;i++)
        {
            CancelNormalHit(i);
        }
    }
}
