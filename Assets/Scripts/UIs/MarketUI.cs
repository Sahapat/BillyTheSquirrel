using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketUI : MonoBehaviour
{
    [System.Serializable]
    public class MarketItem
    {
        public string headerName = string.Empty;
        public string description = string.Empty;
        public int coinRequire = 0;
        public Sprite iconItem = null;
        public GameObject reference = null;
    }
    [SerializeField] SlotUIController[] slotUiController = null;
    [SerializeField] MarketItem[] marketItems = null;
    [SerializeField] TextMeshProUGUI descriptionHeader = null;
    [SerializeField] TextMeshProUGUI description = null;
    [SerializeField] TextMeshProUGUI coinRequire = null;
    [SerializeField]GameObject descriptionObj = null;
    [SerializeField]AudioSource shareAudio = null;
    [SerializeField]AudioClip buy =null;

    private int selectedSlotIndex = 0;
    private bool inputTrigger = false;
    void Awake()
    {
        for (int i = 0; i < marketItems.Length; i++)
        {
            slotUiController[i].AddItemToSlot(marketItems[i].reference.GetComponent<IItem>());
        }
    }
    void Update()
    {
        if (!inputTrigger)
        {
            var directionX = Input.GetAxis("Horizontal");
            var directionY = Input.GetAxis("Vertical");
            bool isHorizontalDirection = Mathf.Abs(directionX) > Mathf.Abs(directionY);
            if (isHorizontalDirection && Mathf.Abs(directionX) > 0.5f)
            {
                inputTrigger = true;
                selectedSlotIndex = (directionX > 0) ? selectedSlotIndex + 1 : selectedSlotIndex - 1;
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex, 0, slotUiController.Length - 1);
                Invoke("SetCanInputAgain", 0.15f);
                UpdateSelectedSlot();
            }
        }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if(selectedSlotIndex < marketItems.Length)
            {
                if(GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin._Coin >= marketItems[selectedSlotIndex].coinRequire)
                {
                    var parentToStore = GameCore.m_GameContrller.TemporaryTranform;
                    var temp = Instantiate(marketItems[selectedSlotIndex].reference,Vector3.zero,Quaternion.identity);
                    GameCore.m_GameContrller.ClientPlayerTarget.CharacterCoin.RemoveCoin(marketItems[selectedSlotIndex].coinRequire);
                    GameCore.m_GameContrller.ClientPlayerTarget.ItemInventory.AddItem(temp,parentToStore);
                    shareAudio.PlayOneShot(buy);
                }
            }
        }
    }
    void UpdateSelectedSlot()
    {
        foreach(var i in slotUiController)
        {
            i.SetUnSelectedSlot();
        }
        slotUiController[selectedSlotIndex].SetSelectedSlot();
        if(selectedSlotIndex < marketItems.Length)
        {
            descriptionObj.SetActive(true);
            descriptionHeader.text = marketItems[selectedSlotIndex].headerName;
            description.text = marketItems[selectedSlotIndex].description;
            coinRequire.text = marketItems[selectedSlotIndex].coinRequire.ToString();
        }
        else
        {
            descriptionObj.SetActive(false);
        }
    }
    void SetCanInputAgain()
    {
        inputTrigger = false;
    }
    void OnEnable()
    {
        selectedSlotIndex = 0;
        UpdateSelectedSlot();
    }
}
