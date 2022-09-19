using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMonsterPopup : MonoBehaviour
{
    [SerializeField] GameObject scrollContents;
    [SerializeField] GameObject monsterButtonPrefab;
    [SerializeField] PlayModeManager playModeManager;

    [SerializeField] CampaignSettingsManager campaignSettingsManager;

    private UnityEngine.Events.UnityAction OnClickMonsterEvent;

    private SoldierScriptable chosenMonster = null;

    private bool active = false;
    public void Init(List<SoldierScriptable> fill)
    {
        ClearConent();
        // if(!active)
        // {
            foreach(SoldierScriptable item in fill)
            {
                if(item.bookEdition == FrostgraveBook.Core)
                {
                    AddMonsterButton(item);
                }
                else{
                    foreach(var cpSetting in campaignSettingsManager.GetEnabledCampaigns())
                    {
                        if(cpSetting == item.bookEdition)
                        {
                            AddMonsterButton(item);
                        }
                    }
                }
                // AddMonsterButton(item);
            }
            // active = true;
        // }
    }

    public void AddMonsterButton(SoldierScriptable _monster)
    {
        GameObject temp = Instantiate(monsterButtonPrefab);
        temp.GetComponent<MonsterButton>().Init(_monster);

        temp.GetComponent<Button>().onClick.AddListener(delegate { OnClickMonster(_monster);});

        temp.transform.SetParent(scrollContents.transform);
    }

    public void OnClickMonster(SoldierScriptable _monster)
    {
        chosenMonster = _monster;
        OnClickMonsterEvent.Invoke();
        //playmodemanager.addmonstertomonsterscroll(_monster);
        ClosePopup();
    }

    public void AssignMonsterEvent(UnityEngine.Events.UnityAction call )
    {
        OnClickMonsterEvent = call;
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

    public void ClearConent()
    {
        foreach(Transform item in scrollContents.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public SoldierScriptable GetMonster()
    {
        return chosenMonster;
    }
}
