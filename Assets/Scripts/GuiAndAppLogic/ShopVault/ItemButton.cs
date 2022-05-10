using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemCostText;

    private MagicItemScriptable itemReference;

    public MagicItemScriptable GetItemReference()
    {
        return itemReference;
    }

    public void Init(MagicItemScriptable itemScriptable)
    {
        itemReference = itemScriptable;
        itemNameText.text = itemReference.itemName;
        itemCostText.text = itemReference.itemPurchasePrice.ToString();
    }

    public void SetButtonEvent(UnityEngine.Events.UnityAction call)
    {
        this.GetComponent<Button>().onClick.AddListener(call);
    }


    public void SwitchCostToSell()
    {
        itemCostText.text = itemReference.itemSalePrice.ToString();
    }


}
