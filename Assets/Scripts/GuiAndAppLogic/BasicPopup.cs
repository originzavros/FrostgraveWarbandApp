using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BasicPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popupText;

    public void UpdatePopupText(string text)
    {
        popupText.text = text;
    }

    public void OnClickClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}