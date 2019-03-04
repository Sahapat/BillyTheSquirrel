using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    private Image[] inventoryImg;
    private Text[] inventoryTxt;
    private Button[] inventoryButton;
    public bool inventoryStatus {get; private set;}
    void Awake()
    {
        inventoryImg = GetComponentsInChildren<Image>();
        inventoryTxt = GetComponentsInChildren<Text>();
        inventoryButton= GetComponentsInChildren<Button>();
    }
    void Start()
    {

    }
    void UpdateAddedItem(ICollectable ItemAdded)
    {
        
    }
    public void CloseInventory()
    {
        foreach(Image img in inventoryImg)
        {
            img.enabled = false;
        }
        foreach(Text txt in inventoryTxt)
        {
            txt.enabled = false;
        }
        foreach(Button btn in inventoryButton)
        {
            btn.enabled = false;
        }
        inventoryStatus = false;
    }
    public void OpenInventory()
    {
        foreach(Image img in inventoryImg)
        {
            img.enabled = true;
        }
        foreach(Text txt in inventoryTxt)
        {
            txt.enabled = true;
        }
        foreach(Button btn in inventoryButton)
        {
            btn.enabled = true;
        }
        inventoryStatus = true;
    }
}
