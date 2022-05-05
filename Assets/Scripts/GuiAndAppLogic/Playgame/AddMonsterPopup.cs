using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMonsterPopup : MonoBehaviour
{
    [SerializeField] GameObject scrollContents;
    [SerializeField] GameObject monsterButtonPrefab;
    [SerializeField] PlayModeManager playModeManager;

    public void Init()
    {
        foreach(MonsterScriptable item in LoadAssets.allMonsterObjects)
        {
            AddMonsterButton(item);
        }
    }

    public void AddMonsterButton(MonsterScriptable _monster)
    {
        GameObject temp = Instantiate(monsterButtonPrefab);
        temp.GetComponent<MonsterButton>().Init(_monster);

        temp.GetComponent<Button>().onClick.AddListener(delegate { OnClickMonster(_monster);});

        temp.transform.SetParent(scrollContents.transform);
    }

    public void OnClickMonster(MonsterScriptable _monster)
    {
        playModeManager.AddMonsterToMonsterScroll(_monster);
        ClosePopup();
    }

    //simple, we'll add encounter table later
    public void OnClickRollMonstersButton()
    {
       
        int temp = Random.Range(0, LoadAssets.allMonsterObjects.Length);
        // Debug.Log("Rolled monster at : " + temp);
        OnClickMonster(LoadAssets.allMonsterObjects[temp]);
        
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
