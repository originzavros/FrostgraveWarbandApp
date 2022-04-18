using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellTextPopup : MonoBehaviour
{
    public TextMeshProUGUI spellNameText;
    public TextMeshProUGUI castingNumberText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI schoolValueText;

    

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
