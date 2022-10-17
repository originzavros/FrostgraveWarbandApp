using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTraitsPopup : MonoBehaviour
{
    [SerializeField] GameObject scrollContents;
    [SerializeField] GameObject monsterKeywordButtonContainerPrefab;
    [SerializeField] GameObject monsterKeywordPopup;

    [SerializeField] GameObject AddTraitPopup;
    [SerializeField] SoldierManager soldierManager;

    private RuntimeSoldierData currentSoldier;

    public void Init(RuntimeSoldierData incoming)
    {
        foreach(var item in incoming.monsterKeywordList)
        {
            AddContainerToScroll(item);
        }
        currentSoldier = incoming;
    }


    public void AddContainerToScroll(RuntimeMonsterKeyword newKeyword)
    {
        var newContainer = Instantiate(monsterKeywordButtonContainerPrefab);
        

        MonsterKeywordButtonContainer mkbc = newContainer.GetComponent<MonsterKeywordButtonContainer>();

        GameObject newKeywordButton = mkbc.GetMonsterKeywordButton();
        newKeywordButton.GetComponent<MonsterKeywordButton>().Init(newKeyword);
        newKeywordButton.GetComponent<MonsterKeywordButton>().SetPopupEvent(delegate { AddMonsterKeywordTextPopup(newKeyword); });

        GameObject deleteButtonObject = mkbc.GetDeleteButton();
        deleteButtonObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickDeleteButton(mkbc); });
        Debug.Log("finished setting up container to add to scroll");

        newContainer.transform.SetParent(scrollContents.transform);
    }
    public void AddMonsterKeywordTextPopup(RuntimeMonsterKeyword _keyword)
    {
        monsterKeywordPopup.GetComponent<MonsterKeywordPopup>().Init(_keyword);
        monsterKeywordPopup.gameObject.SetActive(true);
    }

    public void OnClickAddTraitButton()
    {
        AddTraitPopup.SetActive(true);
    }

    public void AddSoldierTrait(RuntimeMonsterKeyword rmk)
    {
        currentSoldier.monsterKeywordList.Add(rmk);
        AddContainerToScroll(rmk);
    }

    public void OnClickDeleteButton(MonsterKeywordButtonContainer mkbc)
    {
        Debug.Log("Delete button pressed");
        MonsterKeywordButton mkb = mkbc.GetMonsterKeywordButton().GetComponent<MonsterKeywordButton>(); ;
        RemoveTrait(mkb.referenceMonsterKeyword);
        //Destroy(mkbc);
        ClearContainer();
        Init(currentSoldier);
    }

    public void AddTrait(RuntimeMonsterKeyword runtimeMonsterKeyword)
    {
        currentSoldier.monsterKeywordList.Add(runtimeMonsterKeyword);
        Init(currentSoldier);
    }

    public void RemoveTrait(RuntimeMonsterKeyword runtimeMonsterKeyword)
    {
        foreach(var item in currentSoldier.monsterKeywordList)
        {
            if(runtimeMonsterKeyword == item)
            {
                currentSoldier.monsterKeywordList.Remove(item);
                break;
            }
        }
    }

    public void ExitEditTraitsPopup()
    {
        soldierManager.ExitEditTraitsPopup();
        this.gameObject.SetActive(false);
    }

    public void ClearContainer()
    {
        foreach(Transform item in scrollContents.transform)
        {
            Destroy(item.gameObject);
        }
    }
}
