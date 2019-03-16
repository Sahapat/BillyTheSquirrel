using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stemina : MonoBehaviour
{
    [SerializeField]int _SP = 100;
    [SerializeField]int _MaxSP = 100;
    [SerializeField]float regenerateFrequency = 0.8f;
    [SerializeField]int regeneratePerFrequen = 10;

    public int SP {get{return _SP;}}
    public int MaxSP{get{return _MaxSP;}}
    public delegate void _Func();
    public delegate void _FuncVar(int value);
    public event _FuncVar OnSteminachange;
    public event _Func OnSteminaReset;

    private float regenerateTimeCount = 0f;

    void FixedUpdate()
    {   
        if(regenerateTimeCount <= Time.time)
        {
            AddSP(regeneratePerFrequen);
            regenerateTimeCount = Time.time + regenerateFrequency;
        }
    }   
    void OnDestroy()
    {
        OnSteminachange = null;
        OnSteminaReset = null;
    }
    public void ResetStemina()
    {
        _SP = MaxSP;
        _FireEvent_OnSteminaReset();
    }
    public void RemoveSP(int value)
    {
        _SP-=value;
        _SP = Mathf.Clamp(_SP,0,MaxSP);
        _FireEvent_OnSteminaChange();
    }
    public void AddSP(int value)
    {
        _SP+=value;
        _SP = Mathf.Clamp(_SP,0,MaxSP);
        _FireEvent_OnSteminaChange();
    }
    public void SetMaxSP(int value)
    {
        _MaxSP = value;
    }
    void _FireEvent_OnSteminaChange()
    {
        if(OnSteminachange != null)
        {
            OnSteminachange(SP);
        }
    }
    void _FireEvent_OnSteminaReset()
    {
        if(OnSteminaReset !=null)
        {
            OnSteminaReset();
        }
    }
}
