using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterKeywordButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monsterText;
    [SerializeField] Button monsterKeywordButton;

    public MonsterKeywordScriptable referenceMonsterKeyword;

    public void Init(MonsterKeywordScriptable _keyword)
    {
        referenceMonsterKeyword = _keyword;
        monsterText.text = _keyword.keywordName;
    }

    public void SetPopupEvent(UnityEngine.Events.UnityAction call)
    {
        monsterKeywordButton.onClick.AddListener(call);
    }
}
