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


    private PlayerWarband currentWarband;

    private RuntimeSoldierData currentlySelectedSoldier;

    private bool isParty = false;

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
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
        // GameObject temp = Instantiate(soldierContainerPrefab);
        GameObject temp = Instantiate(soldierPlaymodeWindowPrefab);
        // PlaymodeWindow csw = temp.GetComponent<CollapsableWindowContainer>()._collapsableWindow;
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        csw.UpdatePanelInfo(incoming);

        // csw.SetRollDiceEvent(delegate{RollDicePopup(csw);});
        // csw.SetStatusEvent(delegate{AddConditionPop(csw);});
        // csw.SetDeathEscapeEvent(delegate{SoldierEscapePopup(csw);});
        csw.SetEditEvent(delegate{AddChangeSoldierNamePopup(csw);});
        // temp.GetComponent<CollapsableWindowContainer>().GetSwapButton().onClick.AddListener(delegate {OnClickSwapButton( incoming);});
        AddSoldierStatusButtonToSoldier(csw, incoming);
        AddSwapButtonToSoldier(csw, incoming);
        csw.SetWindowToManageMode();
        // if(incoming.soldierInventory.Count > 0)
        // {
        //     foreach(var item in incoming.soldierInventory)
        //     {
        //         Debug.Log(item.itemName);
        //         GameObject newItemSlot = Instantiate(itemSlotPrefab);
        //         ItemSlotSoldier iss = newItemSlot.GetComponent<ItemSlotSoldier>();
        //         iss.SetItemDescriptionButtonEvent(delegate { AddItemInfoToItemPopup(item);});
        //         iss.SetItem(item);
        //         iss.SetUseItemEvent(delegate {UseItemEvent(item, iss, incoming);});
        //         iss.SetItemToPlaymode();
        //         csw.AddItemToContents(newItemSlot);
        //     }
        // }

        foreach(var _keyword in incoming.soldierPermanentInjuries)
        {
            AddInjuryKeywordToSoldier(csw, _keyword);
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
            currentWarband.warbandSoldiers.Remove(incoming);
            currentWarband.warbandBonusSoldiers.Add(incoming);
            OnClickViewWarband();
        }
        else
        {
            if(CheckIfCanAddToWarbandParty())
            {
                currentWarband.warbandSoldiers.Add(incoming);
                currentWarband.warbandBonusSoldiers.Remove(incoming);
                OnClickViewBench();
            } 
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
    }

    

}
