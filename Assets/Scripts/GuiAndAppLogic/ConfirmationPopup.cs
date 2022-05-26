using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmationPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    public static event Action<bool> OnConfirmChosen;
    
    public void Init(string title)
    {
        titleText.text = title;
    }

    public void YesEvent()
    {
        OnConfirmChosen.Invoke(true);
        this.gameObject.SetActive(false);
    }
    public void NoEvent()
    {
        OnConfirmChosen.Invoke(false);
        this.gameObject.SetActive(false);
    }

    


}
