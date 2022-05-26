using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InjuryKeywordPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI injuryKeywordTitle;
    [SerializeField] TextMeshProUGUI injuryKeywordDescritption;

    public void Init(InjuryScriptable injuryKeyword)
    {
        injuryKeywordTitle.text = injuryKeyword.injuryName;
        injuryKeywordDescritption.text = injuryKeyword.injuryDescription;
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
