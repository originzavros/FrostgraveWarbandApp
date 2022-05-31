using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlotSoldier : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] GameObject addItemButton;
    [SerializeField] GameObject itemDescriptionButton;
    [SerializeField] GameObject removeItemButton;
    [SerializeField] GameObject useItemButton;

    private MagicItemScriptable referenceItem;
    private bool isHoldingItem = false;

    public void Init()
    {
        
    }

    public void OnClickAddItemButton()
    {
        addItemButton.SetActive(false);
        itemDescriptionButton.SetActive(true);
        removeItemButton.SetActive(true);
    }

    public void SetItem(MagicItemScriptable item)
    {
        referenceItem = item;
        itemNameText.text = referenceItem.itemName;
        itemDescriptionButton.SetActive(true);
        // removeItemButton.SetActive(true);
        addItemButton.SetActive(false);
        isHoldingItem = true;
    }

    public void SetItemDescriptionButtonEvent(UnityEngine.Events.UnityAction call)
    {
        itemDescriptionButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetAddItemEvent(UnityEngine.Events.UnityAction call)
    {
        addItemButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetRemoveItemEvent(UnityEngine.Events.UnityAction call)
    {
        removeItemButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void SetUseItemEvent(UnityEngine.Events.UnityAction call)
    {
        useItemButton.GetComponent<Button>().onClick.AddListener(call);
    }

    public void OnClickRemoveItem()
    {
        itemNameText.text = "Empty Item Slot";
        removeItemButton.SetActive(false);
        itemDescriptionButton.SetActive(false);
        addItemButton.SetActive(true);
        isHoldingItem = false;
    }

    public void OnClickUseItem()
    {
        useItemButton.GetComponent<Button>().interactable = false;
    }

    public MagicItemScriptable GetStoredItem()
    {
        return referenceItem;
    }

    public bool IsHoldingItem()
    {
        return isHoldingItem;
    }

    public void SetItemToPlaymode()
    {
        removeItemButton.SetActive(false);
        useItemButton.SetActive(true);
        itemDescriptionButton.SetActive(true);
    }
    public void SetItemToVaultMode()
    {
        removeItemButton.SetActive(true);
        itemDescriptionButton.SetActive(true);
    }

    

}
