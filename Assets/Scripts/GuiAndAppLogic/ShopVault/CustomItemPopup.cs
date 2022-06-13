using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomItemPopup : MonoBehaviour
{
    [SerializeField] TMP_InputField itemNameInput;
    [SerializeField] TMP_InputField itemDescriptionInput;
    [SerializeField] Button addItemButton;
    [SerializeField] ShopVaultManager shopVaultManager;

    public void OnClickAddItemButton()
    {
        if(itemNameInput.text == "")
        {
            itemNameInput.text = "Custom Item Name";
        }
        if(itemDescriptionInput.text == "")
        {
            itemDescriptionInput.text = "Custom Item Description";
        }

        shopVaultManager.AddCustomItem(itemNameInput.text, itemDescriptionInput.text);
        
    }

}
