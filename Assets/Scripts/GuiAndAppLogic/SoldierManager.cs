using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierManager : MonoBehaviour
{
    // [SerializeField] GameObject warbandPartyScroll;
    // [SerializeField] GameObject warbandPartyContent;

    // [SerializeField] GameObject warbandBenchScroll;
    // [SerializeField] GameObject warbandBenchContent;

    [SerializeField] GameObject warbandMainContentScroll;
    [SerializeField] GameObject warbandMainContentContainer;
    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] WarbandUIManager warbandUIManager;

    [SerializeField] GameObject popupContainer;
    [SerializeField] GameObject changeSoldierNamePopup;
    [SerializeField] InjuryKeywordPopup injuryKeywordPopup;

    [SerializeField] GameObject soldierContainerPrefab;
    [SerializeField] GameObject soldierPlaymodeWindowPrefab;
    [SerializeField] GameObject injuryKeywordButtonPrefab;
    [SerializeField] GameObject genericSoldierWindowButtonPrefab;
    [SerializeField] GameObject monsterKeywordButtonPrefab;

    [SerializeField] AddMonsterPopup addMonsterPopup;
    [SerializeField] EditTraitsPopup editTraitsPopup;
    [SerializeField] MonsterKeywordPopup monsterKeywordPopup;
    [SerializeField] NavBox navBox;


    private PlayerWarband currentWarband;

    private RuntimeSoldierData currentlySelectedSoldier;

    private bool isParty = false;

    [SerializeField] List<SoldierScriptable> legalSummons;

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        addMonsterPopup.Init(legalSummons);
        navBox.ChangeFragmentName(AppFragment.SoldierManager);
    }

    public void OnClickViewWarband()
    {
        // warbandPartyScroll.SetActive(true);
        // warbandBenchScroll.SetActive(false);
        ClearContents();
        isParty = true;
        FillViewWithParty();
    }
    public void OnClickViewBench()
    {
        // warbandPartyScroll.SetActive(false);
        // warbandBenchScroll.SetActive(true);
        ClearContents();
        isParty = false;
        FillViewWithBench();
    }

    public void FillViewWithParty()
    {
        CreateAndAttachPlaymodeSoldierContainer(currentWarband.warbandWizard.playerWizardProfile, warbandMainContentContainer);

        foreach(var soldier in currentWarband.warbandSoldiers)
        {
            CreateAndAttachPlaymodeSoldierContainer(soldier, warbandMainContentContainer);
        }
    }
    public void FillViewWithBench()
    {
        foreach(var soldier in currentWarband.warbandBonusSoldiers)
        {
            CreateAndAttachPlaymodeSoldierContainer(soldier, warbandMainContentContainer);
        }
    }

    private void CreateAndAttachPlaymodeSoldierContainer(RuntimeSoldierData incoming, GameObject attachedTo)
    {
        GameObject temp = Instantiate(soldierPlaymodeWindowPrefab);
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        csw.UpdatePanelInfo(incoming);

        csw.SetEditEvent(delegate{AddChangeSoldierNamePopup(csw);});
        csw.SetWindowToManageMode();

        foreach(var _keyword in incoming.soldierPermanentInjuries)
        {
            AddInjuryKeywordToSoldier(csw, _keyword);
        }

        foreach (var _keyword in incoming.monsterKeywordList)
        {
            AddMonsterKeywordToMonster(csw, _keyword);
        }

        AddEditTraitButtonToSoldier(csw, incoming);
        AddSoldierStatusButtonToSoldier(csw, incoming);

        if(incoming.soldierType != "Wizard") //can't swap wizard out
        {
            AddSwapButtonToSoldier(csw, incoming);
        }
        
        

        csw.SetBodyPermaActive();        
        
        temp.transform.SetParent(attachedTo.transform);
    }

    public void ClearContents()
    {
        foreach(Transform item in warbandMainContentContainer.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void OnClickSwapButton(RuntimeSoldierData incoming)
    {
        currentlySelectedSoldier = incoming;
        if(isParty)
        {
            currentWarband.warbandBonusSoldiers.Add(currentlySelectedSoldier);
            currentWarband.warbandSoldiers.Remove(incoming);
            OnClickViewWarband();
        }
        else
        {
            if(CheckIfCanAddToWarbandParty())
            {
                currentWarband.warbandSoldiers.Add(currentlySelectedSoldier);
                currentWarband.warbandBonusSoldiers.Remove(incoming);
                OnClickViewBench();
            } 
        }
    }

    public void ExitEditTraitsPopup()
    {
        if(isParty)
        {
            OnClickViewWarband();
        }
        else
        {
            OnClickViewBench();
        }
    }

    public void OnClickDeleteButton(RuntimeSoldierData incoming)
    {
        if(isParty)
        {
            currentWarband.warbandSoldiers.Remove(incoming);
            OnClickViewWarband();
        }
        else
        {
            currentWarband.warbandBonusSoldiers.Remove(incoming);
            OnClickViewBench();
        }
    }

    public void AddChangeSoldierNamePopup(PlaymodeWindow csw)
    {
        changeSoldierNamePopup.SetActive(true);
        changeSoldierNamePopup.GetComponent<ChangeSoldierNamePopup>().Init(csw);
    }

    public void AddInjuryKeywordToSoldier(PlaymodeWindow _playmodeWindow, InjuryScriptable _keyword)
    {
        GameObject temp = Instantiate(injuryKeywordButtonPrefab);
        temp.GetComponent<InjuryKeywordButton>().Init(_keyword);

        temp.GetComponent<InjuryKeywordButton>().SetPopupEvent(delegate {AddInjuryKeywordTextPopup(_keyword);});

        _playmodeWindow.AddItemToContents(temp);
    }

    public void AddSwapButtonToSoldier(PlaymodeWindow _playmodeWindow, RuntimeSoldierData incoming)
    {
        GameObject temp = Instantiate(genericSoldierWindowButtonPrefab);
        temp.GetComponent<GenericSoldierWindowButton>().Init("Swap");
        temp.GetComponent<GenericSoldierWindowButton>().SetPopupEvent(delegate{OnClickSwapButton( incoming);});
        _playmodeWindow.AddItemToContents(temp);
    }

    public void AddDeleteButtonToSoldier(PlaymodeWindow _playmodeWindow, RuntimeSoldierData incoming)
    {
        GameObject temp = Instantiate(genericSoldierWindowButtonPrefab);
        temp.GetComponent<GenericSoldierWindowButton>().Init("Delete");
        temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.red);
        temp.GetComponent<GenericSoldierWindowButton>().SetPopupEvent(delegate { OnClickDeleteButton(incoming); });
        _playmodeWindow.AddItemToContents(temp);
    }
    public void AddSoldierStatusButtonToSoldier(PlaymodeWindow _playmodeWindow, RuntimeSoldierData incoming)
    {
        GameObject temp = Instantiate(genericSoldierWindowButtonPrefab);
        if(incoming.status == SoldierStatus.ready)
        {
            temp.GetComponent<GenericSoldierWindowButton>().Init("Ready");
            temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.green);
        }
        else if(incoming.status == SoldierStatus.badlyWounded)
        {
            temp.GetComponent<GenericSoldierWindowButton>().Init("Badly Wounded");
            temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.red);
        }
        else if(incoming.status == SoldierStatus.preserved){
            temp.GetComponent<GenericSoldierWindowButton>().Init("preserved");
            temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.yellow);
        }
        else{
            temp.GetComponent<GenericSoldierWindowButton>().Init(incoming.status.ToString());
            temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.white);
        }
        
        // temp.GetComponent<GenericSoldierWindowButton>().SetPopupEvent(delegate{OnClickSwapButton( incoming);});
        _playmodeWindow.AddItemToContents(temp);
    }

    public void OnClickFinalizeWarband()
    {
        warbandUIManager.SaveWarbandChanges();
        warbandUIManager.BackToWarbandMain();
    }
    public void OnClickBackButton()
    {
        warbandUIManager.BackToWarbandMain();
    }


    public void ErrorPopup(string errorString)
    {
        popupContainer.SetActive(true);
        popupContainer.GetComponent<BasicPopup>().UpdatePopupText(errorString);
    }

    public bool CheckIfCanAddToWarbandParty()
    {
        if(currentWarband.warbandSoldiers.Count < 9)
        {
            return true;
        }
        else{
            ErrorPopup("Warband Party is Full!");
            return false;
        }
    }

    public void AddInjuryKeywordTextPopup(InjuryScriptable _keyword)
    {
        injuryKeywordPopup.gameObject.SetActive(true);
        injuryKeywordPopup.Init(_keyword);
    }

    public void OnClickAddSummon()
    {

        addMonsterPopup.gameObject.SetActive(true);
        addMonsterPopup.AssignMonsterEvent(delegate { monsterSummonPopup(); });
    }

    public void monsterSummonPopup()
    {
        SoldierScriptable selectedMonster = addMonsterPopup.GetMonster();
        RuntimeSoldierData newSummon = new RuntimeSoldierData();
        newSummon.Init(selectedMonster);
        if(selectedMonster.hiringName == "Collegium Porter")
        {
            newSummon.inventoryLimit = 3;
        }
        //else if(selectedMonster.hiringName == "Bear")
        //{
        //    newSummon.will += 3;
        //}
        //else if (selectedMonster.hiringName == "Bear")
        //{
        //    newSummon.will += 3;
        //}

        currentWarband.warbandBonusSoldiers.Add(newSummon);
        OnClickViewBench();
    }

    public void AddMonsterKeywordToMonster(PlaymodeWindow _playmodeWindow, RuntimeMonsterKeyword _keyword)
    {
        GameObject temp = Instantiate(monsterKeywordButtonPrefab);
        temp.GetComponent<MonsterKeywordButton>().Init(_keyword);

        temp.GetComponent<MonsterKeywordButton>().SetPopupEvent(delegate { AddMonsterKeywordTextPopup(_keyword); });

        _playmodeWindow.AddItemToContents(temp);
    }

    public void AddEditTraitButtonToSoldier(PlaymodeWindow _playmodeWindow, RuntimeSoldierData incoming)
    {
        GameObject temp = Instantiate(genericSoldierWindowButtonPrefab);

        temp.GetComponent<GenericSoldierWindowButton>().Init("Edit Traits");
        temp.GetComponent<GenericSoldierWindowButton>().SetPopupEvent(delegate { OnClickAddTraitButton(incoming); });
        temp.GetComponent<GenericSoldierWindowButton>().SetColor(Color.cyan);
        _playmodeWindow.AddItemToContents(temp);
    }

    public void OnClickAddTraitButton(RuntimeSoldierData incoming)
    {
        editTraitsPopup.gameObject.SetActive(true);
        editTraitsPopup.Init(incoming);
    }

    public void AddMonsterKeywordTextPopup(RuntimeMonsterKeyword _keyword)
    {
        monsterKeywordPopup.GetComponent<MonsterKeywordPopup>().Init(_keyword);
        monsterKeywordPopup.gameObject.SetActive(true);
    }


}
