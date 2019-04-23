using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    [SerializeField] Player _ClientPlayerTarget = null;
    [SerializeField] Transform _temporaryTranform = null;
    [SerializeField] GameObject _targetToLockOn = null;
    public GameObject itemFocus = null;
    [SerializeField] GameObject _PopOutPrefab = null;
    [SerializeField] float _ClampPlayerByYPosition = 0f;
    [SerializeField] AudioSource shareAudioSource = null;
    [SerializeField] AudioClip[] sounds = null;

    public bool Controlable = true;
    public bool isGameStart{get;private set;}
    public Player ClientPlayerTarget
    {
        get
        {
            return _ClientPlayerTarget;
        }
    }
    public Transform TemporaryTranform
    {
        get
        {
            return _temporaryTranform;
        }
    }
    public GameObject TargetToLockOn
    {
        get
        {
            return _targetToLockOn;
        }
        private set
        {
            _targetToLockOn = value;
        }
    }
    public GameObject PopOutPrefab
    {
        get
        {
            return _PopOutPrefab;
        }
    }
    public float ClampPlayerByYPosition
    {
        get
        {
            return _ClampPlayerByYPosition;
        }
    }
    private GameObject[] enemyOnFOVCamera = null;
    void Awake()
    {
        enemyOnFOVCamera = new GameObject[10];
    }
    void Start()
    {
        GameCore.m_uiHandler.CloseInventory();
        GameCore.m_cameraController.enabled = false;
        Controlable = false;
    }
    void FixedUpdate()
    {
        //Check death
        if (ClientPlayerTarget.isDead)
        {
            GameCore.m_uiHandler.ShowGameOver();
        }
        else
        {
            GameCore.m_uiHandler.CloseGameOver();
        }
        //Clamp player when fall
        if (ClientPlayerTarget.transform.position.y < ClampPlayerByYPosition)
        {
            var spawnPos = ClientPlayerTarget.CurrentGround.position;
            spawnPos = new Vector3(spawnPos.x, spawnPos.y + 10, spawnPos.z);
            ClientPlayerTarget.transform.position = spawnPos;
            ClientPlayerTarget.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    public void AddEnemyOnFOVCamera(GameObject objIn)
    {
        for (int i = 0; i < enemyOnFOVCamera.Length; i++)
        {
            if (enemyOnFOVCamera[i])
            {
                if (enemyOnFOVCamera[i].GetInstanceID() == objIn.GetInstanceID())
                {
                    break;
                }
            }
            else
            {
                enemyOnFOVCamera[i] = objIn;
                break;
            }
        }
    }
    public void RemoveEnemyOnFOVCamera(GameObject objIn)
    {
        for (int i = 0; i < enemyOnFOVCamera.Length; i++)
        {
            if (enemyOnFOVCamera[i])
            {
                if (enemyOnFOVCamera[i].GetInstanceID() == objIn.GetInstanceID())
                {
                    enemyOnFOVCamera[i] = null;
                }
            }
        }
    }
    public void SetClosedEnemyInFOVCamera(bool isRandom)
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < enemyOnFOVCamera.Length; i++)
        {
            if (enemyOnFOVCamera[i])
            {
                var distance = Vector3.Distance(ClientPlayerTarget.transform.position, enemyOnFOVCamera[i].transform.position);
                if (distance < 20)
                {
                    temp.Add(enemyOnFOVCamera[i]);
                }
            }
        }
        if (isRandom)
        {
            int randomIndex = 0;
            if(temp.Count == 0)
            {
                randomIndex = -1;
            }
            else
            {
                if(TargetToLockOn)
                {
                    while(TargetToLockOn.GetInstanceID() == temp[randomIndex].GetInstanceID())
                    {
                        randomIndex = Random.Range(0,temp.Count);
                    }
                }
                else
                {
                    randomIndex = Random.Range(0,temp.Count);
                }
            }
            TargetToLockOn = (randomIndex != -1)?temp[randomIndex]:null;
        }
        else
        {
            float[] angles = new float[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                var point1 = new Vector2(temp[i].transform.position.x, temp[i].transform.position.z);
                var point2 = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
                angles[i] = Mathf.Abs(Vector2.Angle(point1, point2) - 13f);
            }



            float minAngle = Mathf.Infinity;
            int minIndex = -1;

            for (int i = 0; i < angles.Length; i++)
            {
                if (minAngle > angles[i])
                {
                    minAngle = angles[i];
                    minIndex = i;
                }
            }
            TargetToLockOn = (minIndex != -1) ? temp[minIndex] : null;
        }
    }
    void SetToControlable()
    {
        Controlable = true;
    }
    public void SetNotControlableByTime(float time)
    {
        Controlable = false;
        Invoke("SetToControlable", time);
    }
    public void ClearTargetLockOn()
    {
        TargetToLockOn = null;
    }
    public void GameStart()
    {
        GameCore.m_cameraController.enabled = true;
        isGameStart = true;
        Controlable = true;
    }
    public void PlayAcornCollect()
    {
        shareAudioSource.PlayOneShot(sounds[0]);
    }
}
