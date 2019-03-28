using System.Collections;
using System.Collections.Generic;

public class HitDataStorage
{
    private int[] HitStorage;
    private int addIndex = 0;
    public HitDataStorage(int storageSize)
    {
        HitStorage = new int[storageSize];
    }
    public bool CheckHit(int instanceID)
    {
        bool checkStatus = true;
        for (int i = 0; i < HitStorage.Length; i++)
        {
            if (instanceID == HitStorage[i])
            {
                checkStatus = false;
                break;
            }
        }
        if (checkStatus)
        {
            try
            {
                HitStorage[addIndex] = instanceID;
                addIndex++;
            }
            catch (System.IndexOutOfRangeException)
            {
                ResetHit();
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetHit()
    {
        addIndex = 0;
        for (int i = 0; i < HitStorage.Length; i++)
        {
            HitStorage[i] = 0;
        }
    }
}
