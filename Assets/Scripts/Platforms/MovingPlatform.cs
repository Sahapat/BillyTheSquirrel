using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActivable
{
    private enum JorneyType
    {
        NORMAL,
        BACK,
        RANDOM
    };

    [SerializeField] Transform[] anchorToMove = null;
    [SerializeField] float[] achorSpeed = null;
    [SerializeField] JorneyType jorneyType = MovingPlatform.JorneyType.NORMAL;


    private int currentIndex = 0;
    private int increaser = 1;
    [SerializeField]private bool isActive = false;

    void FixedUpdate()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, anchorToMove[currentIndex].position, Time.deltaTime * achorSpeed[currentIndex]);
            if (transform.position == anchorToMove[currentIndex].position)
            {
                ChangePoint();
            }
        }
    }
    public void Active()
    {
        isActive = true;
    }

    public void InActive()
    {
        isActive = false;
    }
    void ChangePoint()
    {
        switch (jorneyType)
        {
            case MovingPlatform.JorneyType.NORMAL:
                increaser = 1;
                currentIndex += increaser;
                if (currentIndex >= anchorToMove.Length)
                {
                    currentIndex = 0;
                }
                break;
            case MovingPlatform.JorneyType.BACK:
                if (currentIndex + increaser >= anchorToMove.Length)
                {
                    increaser = -1;
                }
                else if (currentIndex + increaser < 0)
                {
                    increaser = 1;
                }
                currentIndex += increaser;
                break;
            case MovingPlatform.JorneyType.RANDOM:
                currentIndex = Random.Range(0, anchorToMove.Length);
                break;
        }
    }
}
