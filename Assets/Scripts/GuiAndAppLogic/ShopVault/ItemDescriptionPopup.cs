using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescriptionPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;

    public void Init(MagicItemScriptable item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
