using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterKeywordPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI monsterKeywordTitle;
    [SerializeField] TextMeshProUGUI monsterKeywordDescritption;

    public void Init(MonsterKeywordScriptable monsterKeyword)
    {
        monsterKeywordTitle.text = monsterKeyword.keywordName;
        monsterKeywordDescritption.text = monsterKeyword.keywordDescription;
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
