using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Health
{
    public int HP = 0;
    public int MaxHP = 100;

    public void ResetHP()
    {
        HP = MaxHP;
    }
}
[System.Serializable]
public class Stemina
{
    public int SP = 0;
    public int MaxSP = 100;
    public void ResetSP()
    {
        SP = MaxSP;
    }
}