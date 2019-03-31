using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUIController : MonoBehaviour
{
    [SerializeField] Sprite unSelectSprite = null;
    [SerializeField] Sprite SelectSprite = null;
    [SerializeField] Image targetIconImage = null;
    [SerializeField] Sprite NoneImage = null;
    [SerializeField] TextMeshProUGUI targetDescriptionTxt = null;
    [SerializeField] TextMeshProUGUI targetHeaderDescription = null;

    ItemType itemType = ItemType.NONE;
    Sprite icon = null;
    string description = string.Empty;
    string headerDescription = string.Empty;
    Image selectImage = null;

    void Awake()
    {
        selectImage = GetComponent<Image>();
    }
    public void AddItemToSlot(ICollectable itemIn)
    {
        if (itemIn == null)
        {
            itemType = ItemType.NONE;
            icon = NoneImage;
            description = string.Empty;
            headerDescription = string.Empty;
        }
        else
        {
            itemType = itemIn.itemType;
            icon = itemIn.Icon;
            description = itemIn.description;
            headerDescription = itemIn.headerName;
        }
        UpdateItemInSlot();
    }
    public void SetSelectedSlot()
    {
        selectImage.sprite = SelectSprite;
    }
    public void SetUnSelectedSlot()
    {
        selectImage.sprite = unSelectSprite;
    }
    void UpdateItemInSlot()
    {
        var showIcon = (icon == null) ? NoneImage : icon;
        targetIconImage.sprite = showIcon;
        targetDescriptionTxt.text = description;
        targetHeaderDescription.text = headerDescription;
    }
}
