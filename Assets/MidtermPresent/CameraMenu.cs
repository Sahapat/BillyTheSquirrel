using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    [System.Serializable]
    public struct CameraPos
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    [SerializeField]CameraPos MenuPos;
    [SerializeField]GameObject MenuHUB;
    [SerializeField]CameraPos OatPos;
    [SerializeField]GameObject OatHUB;
    [SerializeField]CameraPos SunPos;
    [SerializeField]GameObject SunHUB;
    [SerializeField]AnimationShow OatShow;
    [SerializeField]EnviroumentShow SunShow;
    [SerializeField]float speed = 3f;
    bool isSet = false;
    CameraPos Destination;
    void FixedUpdate()
    {
        if(!isSet)return;
        transform.position = Vector3.MoveTowards(transform.position,Destination.Position,Time.deltaTime*speed);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(Destination.Rotation),Time.deltaTime*speed*19.2f);

        if(transform.position == Destination.Position && transform.rotation == Quaternion.Euler(Destination.Rotation))
        {
            isSet = false;
        }
    }
    public void SetToOat()
    {
        isSet = true;
        Destination = OatPos;
        SunHUB.SetActive(false);
        MenuHUB.SetActive(false);
        OatHUB.SetActive(true);
        OatShow.OpenAll();
        SunShow.CloseAll();
    }
    public void SetToSun()
    {
        isSet = true;
        Destination = SunPos;
        OatHUB.SetActive(false);
        MenuHUB.SetActive(false);
        SunHUB.SetActive(true);
        OatShow.CloseAll();
        SunShow.OpenAll();
    }
    public void SetToMenu()
    {
        isSet = true;
        Destination = MenuPos;
        SunHUB.SetActive(false);
        OatHUB.SetActive(false);
        MenuHUB.SetActive(true);
        OatShow.CloseAll();
        SunShow.CloseAll();
    }
}
