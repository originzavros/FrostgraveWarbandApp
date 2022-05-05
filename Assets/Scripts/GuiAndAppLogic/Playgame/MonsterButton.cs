using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monsterText;
    [SerializeField] Button monsterKeywordButton;

    public MonsterScriptable referenceMonster;

    public void Init(MonsterScriptable _keyword)
    {
        referenceMonster = _keyword;
        monsterText.text = _keyword.hiringName;
    }

    public void SetOnClickEvent(UnityEngine.Events.UnityAction call)
    {
        monsterKeywordButton.onClick.AddListener(call);
    }
}
