using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireSoldiersManager : MonoBehaviour
{
    // [SerializeField] GameObject soldierHiringWindowPrefab;
    [SerializeField] GameObject currentSoldiersContent;
    [SerializeField] GameObject soldierHiringContent;
    [SerializeField] GameObject currentSoldiersScroll;
    [SerializeField] GameObject soldierHiringScroll;
    [SerializeField] NavBox navBox;
    [SerializeField] GameObject popupContainer;

    // [SerializeField] GameObject collapsableWindowPrefab;
    [SerializeField] GameObject soldierHiringContainerPrefab;
    [SerializeField] WarbandUIManager warbandUIManager;



    private PlayerWarband loadedWarband;
    private int currentGoldTotal;
    private int currentHiredSpecialistsSoldierCount;
    private int currentHiredSoldierCount;
    public void Init(PlayerWarband playerWarband)
    {
        currentHiredSpecialistsSoldierCount = 0;
        currentHiredSoldierCount = 0;
        loadedWarband = playerWarband;
        currentGoldTotal = playerWarband.warbandGold;
        // currentGoldTotal = 1000;
        UpdateGoldAmount(0);
        //init current warband
        if(playerWarband.warbandSoldiers.Count > 0)
        {
            foreach(var item in playerWarband.warbandSoldiers)
            {

                // CreateAndAttachCollapsableWindow(item, currentSoldiersContent);
                // item.isHired = true;
                Debug.Log(item);
                CreateAndAttachSoliderContainer(item, currentSoldiersContent, false);
                currentHiredSoldierCount++;
                if(item.soldierType == "Specialist")
                {
                    currentHiredSpecialistsSoldierCount++;
                }
            }
        }
        

        //init hiring soldiers
        foreach(var item in LoadAssets.allSoldierObjects)
        {
            CreateAndAttachSoliderContainer(item, soldierHiringContent);
            // CreateAndAttachCollapsableWindow(item, soldierHiringContent);
        }
    }

    //don't use, use container instead
    // private void CreateAndAttachSoldierWindow(SoldierScriptable incoming, GameObject attachedTo)
    // {
    //     GameObject temp = Instantiate(soldierHiringWindowPrefab);
    //     SoldierInfoWindow csw = temp.GetComponent<SoldierInfoWindow>();
    //     csw.UpdatePanelInfo(incoming);

    //     SoldierHireWindow shw = temp.GetComponent<SoldierHireWindow>();
    //     shw.SwitchToHireMode();
    //     shw.SetHireEvent(delegate {OnClickHireSoldier(csw);});
        
    //     temp.transform.SetParent(attachedTo.transform);
    // }

    //don't use, use container instead
    // private void CreateAndAttachCollapsableWindow(SoldierScriptable incoming, GameObject attachedTo)
    // {
    //     GameObject temp = Instantiate(collapsableWindowPrefab);
    //     SoldierInfoWindow csw = temp.GetComponent<SoldierInfoWindow>();
    //     csw.UpdatePanelInfo(incoming);

    //     SoldierHireWindow shw = temp.GetComponent<SoldierHireWindow>();
    //     shw.SwitchToHireMode();
    //     shw.SetHireEvent(delegate {OnClickHireSoldier(csw);});
        
    //     temp.transform.SetParent(attachedTo.transform);
    // }

    private void CreateAndAttachSoliderContainer(SoldierScriptable incoming, GameObject attachedTo, bool hireMode = true)
    {
        GameObject temp = Instantiate(soldierHiringContainerPrefab);
        SoldierInfoWindow csw = temp.GetComponentInChildren<SoldierInfoWindow>();
        csw.UpdatePanelInfo(incoming);

        SoldierHireWindow shw = temp.GetComponent<SoldierHireWindow>();
        if(hireMode)
        {
            shw.SwitchToHireMode();
            shw.SetHireEvent(delegate {OnClickHireSoldier(csw);});
        }
        else{
            shw.SwitchToFireMode();
            shw.SetFireEvent(delegate {OnClickFireSoldier(csw);});
        }
        
        temp.transform.SetParent(attachedTo.transform);
    }

    public void OnClickViewWarband()
    {
        soldierHiringScroll.SetActive(false);
    }
    public void OnClickHireWarband()
    {
        soldierHiringScroll.SetActive(true);
    }

    public void OnClickHireSoldier(SoldierInfoWindow siw)
    {
        Debug.Log("hired soldier by name: " + siw.GetStoredSoldier().hiringName);

        SoldierScriptable hiredSoldier = siw.GetStoredSoldier();
        if(hiredSoldier.cost > currentGoldTotal)
        {
            ErrorPopup("Not enough gold to hire soldier!");
        }
        else if(currentHiredSoldierCount >= loadedWarband.warbandMaxSoldiers)
        {
            ErrorPopup("Warband is full, fire soldiers to hire more");
        }
        else if(hiredSoldier.soldierType == "Specialist" && currentHiredSpecialistsSoldierCount >= 4)
        {
            ErrorPopup("Cannot hire more than 4 specialists");
        }
        else{
            CreateAndAttachSoliderContainer(hiredSoldier, currentSoldiersContent, false);
            currentHiredSoldierCount++;
            UpdateGoldAmount(-1 * hiredSoldier.cost);
            if(hiredSoldier.soldierType == "Specialist")
            {
                currentHiredSpecialistsSoldierCount++;
            }
        }

        
        
    }

    public void ErrorPopup(string error)
    {
        popupContainer.SetActive(true);
        popupContainer.GetComponent<BasicPopup>().UpdatePopupText(error);
    }
    
    public void OnClickFireSoldier(SoldierInfoWindow siw)
    {
        SoldierScriptable hiredSoldier = siw.GetStoredSoldier();
        if(hiredSoldier.soldierType == "Specialist")
        {
            currentHiredSpecialistsSoldierCount--;
        }
        currentHiredSoldierCount--;
        Destroy(siw.transform.parent.gameObject);
    }

    public void UpdateGoldAmount(int gold)
    {
        currentGoldTotal += gold;
        navBox.ChangeScreenName("Current Gold: " + currentGoldTotal);
    }

    public void OnClickBackButton()
    {
        ClearContents();
        warbandUIManager.BackToWarbandMain();
    }
    public void OnClickFinalizeWarband()
    {
        AddAllHiredToPlayerWarband();
        loadedWarband.warbandGold = currentGoldTotal;
        warbandUIManager.SaveWarbandChanges();
        ClearContents();
        warbandUIManager.BackToWarbandMain();
    }
    public void ClearContents()
    {
        
        foreach(Transform item in currentSoldiersContent.transform)
        {
            Destroy(item.gameObject);
        }
        foreach(Transform item in soldierHiringContent.transform)
        {
            Destroy(item.gameObject);
        }
    }
    public void AddAllHiredToPlayerWarband()
    {
        loadedWarband.warbandSoldiers.Clear();
        foreach(Transform item in currentSoldiersContent.transform)
        {
            SoldierScriptable savingSoldier = item.GetComponentInChildren<SoldierInfoWindow>().GetStoredSoldier();
            loadedWarband.warbandSoldiers.Add(savingSoldier);
        }
    }
    public void HandleDropdownSorting()
    {

    }








}
