using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Health
{
    public int HP{get;private set;}
    public int MaxHP{get; private set;}
    public delegate void _Func();
    public delegate void _FuncValue(int value);

    public event _FuncValue OnHPChanged;
    public event _Func OnResetHP;

    public Health(int maxHealth)
    {
        SetMaxHP(maxHealth);
        ResetHP();
    }
    ~Health()
    {
        OnHPChanged = null;
        OnResetHP = null;
    }
    public void RemoveHP(int value)
    {
        HP -= value;
        _FireEvent_OnHPChanged();
    }
    public void AddHP(int value)
    {
        HP += value;
        _FireEvent_OnHPChanged();
    }
    public void SetMaxHP(int value)
    {
        MaxHP = value;
        _FireEvent_OnHPChanged();
    }
    public void ResetHP()
    {
        HP = MaxHP;
        _FireEvent_OnResetHP();
    }
    void _FireEvent_OnResetHP()
    {
        if(OnResetHP != null)OnResetHP();
    }
    void _FireEvent_OnHPChanged()
    {
        if(OnHPChanged != null)OnHPChanged(HP);
    }
}