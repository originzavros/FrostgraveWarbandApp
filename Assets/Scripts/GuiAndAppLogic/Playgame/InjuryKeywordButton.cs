using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InjuryKeywordButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI injuryText;
    [SerializeField] Button injuryKeywordButton;

    public InjuryScriptable injuryReference;

    public void Init(InjuryScriptable _keyword)
    {
        injuryReference = _keyword;
        injuryText.text = _keyword.injuryName;
    }

    public void SetPopupEvent(UnityEngine.Events.UnityAction call)
    {
        injuryKeywordButton.onClick.AddListener(call);
    }
}
