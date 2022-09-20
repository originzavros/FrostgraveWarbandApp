using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TMP_Dropdown soldierTypeDropdown;
    [SerializeField] TextMeshProUGUI goldTrackerText;
    [SerializeField] GameObject CoinAnimation;
    [SerializeField] CampaignSettingsManager campaignSettingsManager;



    private PlayerWarband loadedWarband;
    private int currentGoldTotal;
    private int currentHiredSpecialistsSoldierCount;
    private int currentHiredSoldierCount;
    private bool apprenticeHired;
    private bool hasCarrierPigeons = false;
    private List<FrostgraveBook> enabledCampaignSoldiers = new List<FrostgraveBook>();
    public void Init(PlayerWarband playerWarband)
    {
        navBox.ChangeFragmentName(AppFragment.HireSoldiers);
        currentHiredSpecialistsSoldierCount = 0;
        currentHiredSoldierCount = 0;
        loadedWarband = playerWarband;
        currentGoldTotal = playerWarband.warbandGold;
        apprenticeHired = false;
        // currentGoldTotal = 1000;
        UpdateGoldAmount(0);

        enabledCampaignSoldiers = campaignSettingsManager.GetEnabledCampaigns();

        //init current warband
        if(playerWarband.warbandSoldiers.Count > 0)
        {
            foreach(var item in playerWarband.warbandSoldiers)
            {

                // CreateAndAttachCollapsableWindow(item, currentSoldiersContent);
                // item.isHired = true;
                // Debug.Log(item);
                // SoldierScriptable tempsoldier =   SoldierScriptable.CreateInstance<SoldierScriptable>();
                // tempsoldier.Init(item);
                CreateAndAttachSoliderContainer(item, currentSoldiersContent, false);
                currentHiredSoldierCount++;
                if(item.soldierType == "Specialist")
                {
                    currentHiredSpecialistsSoldierCount++;
                }
                if(item.soldierType == "Apprentice")
                {
                    apprenticeHired = true;
                }
            }
        }

        hasCarrierPigeons = false;
        foreach(var item in playerWarband.warbandVault)
        {
            if(item.itemName == "Carrier Pigeons"){ hasCarrierPigeons = true;}
        }
        

        //init hiring soldiers
        foreach(var item in LoadAssets.allSoldierObjects)
        {
            if(item.soldierType == "Apprentice")
            {
                SoldierScriptable newApprenticeData = ScriptableObject.CreateInstance<SoldierScriptable>();
                newApprenticeData.Init(playerWarband.warbandWizard.playerWizardProfile);
                newApprenticeData.fight -= 2;
                newApprenticeData.will -= 2;
                newApprenticeData.health -= 2;
                newApprenticeData.cost = (playerWarband.warbandWizard.playerWizardLevel -6) * 10 + 160;
                newApprenticeData.soldierType = "Apprentice";
                newApprenticeData.soldierName = "Apprentice";
                newApprenticeData.hiringName = "Apprentice";
                newApprenticeData.description = "Can cast spells with a -2 to the Casting Roll";
                newApprenticeData.baseSoldierEquipment.Clear();

                RuntimeSoldierData convertAgain = new RuntimeSoldierData();
                convertAgain.Init(newApprenticeData);
                CreateAndAttachSoliderContainer(convertAgain, soldierHiringContent);

            }
            else{

                if(campaignSettingsManager.GetAllSoldiersState())
                {
                    RuntimeSoldierData newSoldier = new RuntimeSoldierData();
                    newSoldier.Init(item);
                    CreateAndAttachSoliderContainer(newSoldier, soldierHiringContent, hiringCostMod: (hasCarrierPigeons ? -10:0));
                }
                else{
                    if(item.bookEdition == FrostgraveBook.Core)
                    {
                        RuntimeSoldierData newSoldier = new RuntimeSoldierData();
                        newSoldier.Init(item);
                        CreateAndAttachSoliderContainer(newSoldier, soldierHiringContent, hiringCostMod: (hasCarrierPigeons ? -10:0));
                    }
                    else{
                        foreach(var book in enabledCampaignSoldiers)
                        {
                            if(item.bookEdition == book)
                            {
                                RuntimeSoldierData newSoldier = new RuntimeSoldierData();
                                newSoldier.Init(item);
                                CreateAndAttachSoliderContainer(newSoldier, soldierHiringContent, hiringCostMod: (hasCarrierPigeons ? -10:0));
                            }
                        }

                    }
                }
                // RuntimeSoldierData newSoldier = new RuntimeSoldierData();
                // newSoldier.Init(item);
                // CreateAndAttachSoliderContainer(newSoldier, soldierHiringContent, hiringCostMod: (hasCarrierPigeons ? -10:0));
            }
            
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

    private void CreateAndAttachSoliderContainer(RuntimeSoldierData incoming, GameObject attachedTo, bool hireMode = true, int hiringCostMod = 0)
    {
        // RuntimeSoldierData newsoldier = new RuntimeSoldierData();
        // newsoldier.Init(incoming);
        GameObject temp = Instantiate(soldierHiringContainerPrefab);
        SoldierInfoWindow csw = temp.GetComponentInChildren<SoldierInfoWindow>();
        csw.UpdatePanelInfo(incoming,hiringCostMod);

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

        RuntimeSoldierData hiredSoldier = new RuntimeSoldierData();
        hiredSoldier.Init(siw.GetStoredSoldier());
        hiredSoldier.soldierName = hiredSoldier.hiringName; //give them a default name
        int realSoldierCost = hiredSoldier.cost + (hasCarrierPigeons ? -10:0);
        if(realSoldierCost > currentGoldTotal)
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
        else if(hiredSoldier.soldierType == "Apprentice" && apprenticeHired)
        {
            ErrorPopup("Already Hired Apprentice");
        }
        else{
            PlayCoinAnimation();
            CreateAndAttachSoliderContainer(hiredSoldier, currentSoldiersContent, false);
            currentHiredSoldierCount++;
            if(realSoldierCost < 0){ realSoldierCost = 0;}
            UpdateGoldAmount(-1 * realSoldierCost);
            if(hiredSoldier.soldierType == "Specialist")
            {
                currentHiredSpecialistsSoldierCount++;
            }
            if(hiredSoldier.soldierType == "Apprentice")
            {
                apprenticeHired = true;
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
        RuntimeSoldierData hiredSoldier = siw.GetStoredSoldier();
        if(hiredSoldier.soldierType == "Specialist")
        {
            currentHiredSpecialistsSoldierCount--;
        }
        if(hiredSoldier.soldierType == "Apprentice")
        {
            apprenticeHired = false;
        }
        currentHiredSoldierCount--;
        Destroy(siw.transform.parent.gameObject);
    }

    public void UpdateGoldAmount(int gold)
    {
        currentGoldTotal += gold;
        goldTrackerText.text = "Current Gold: " + currentGoldTotal.ToString();
        // navBox.ChangeScreenName("Current Gold: " + currentGoldTotal);
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
            RuntimeSoldierData savingSoldier = item.GetComponentInChildren<SoldierInfoWindow>().GetStoredSoldier();
            // RuntimeSoldierData temp = new RuntimeSoldierData();
            // temp.Init(savingSoldier);
            loadedWarband.warbandSoldiers.Add(savingSoldier);
        }
    }
    public void HandleDropdownSorting()
    {
        string temp = soldierTypeDropdown.options[soldierTypeDropdown.value].text;
        SortSoldierHiringContent(temp);
    }

    public void SortSoldierHiringContent(string sorttype)
    {
        if(sorttype == "Standard")
        {
            foreach(Transform item in soldierHiringContent.transform)
            {
                if(item.GetComponentInChildren<SoldierInfoWindow>().GetStoredSoldier().soldierType != "Standard")
                {
                    item.gameObject.SetActive(false);
                }
                else{
                    item.gameObject.SetActive(true);
                }
            }
        }
        else if(sorttype == "Specialist")
        {
            foreach(Transform item in soldierHiringContent.transform)
            {
                if(item.GetComponentInChildren<SoldierInfoWindow>().GetStoredSoldier().soldierType != "Specialist")
                {
                    item.gameObject.SetActive(false);
                }
                else{
                    item.gameObject.SetActive(true);
                }
            }
        }
        else{
            foreach(Transform item in soldierHiringContent.transform)
            {
                
                item.gameObject.SetActive(true);
                
            }
        }
    }
    // public void OnSchoolDropdownChanged()
    // {

    //     string temp = spellDropDown.options[spellDropDown.value].text;
    //     // Debug.Log("Dropdown text: " + temp);
    //     //string temp = spellDropDown.GetComponent<drop
    //     // ShowOnlyButtonsOfSchool(temp);
    //     CheckRestrictions(temp);
    // }

    // public void OnRangeTypeDropdownChanged()
    // {
    //     string temp = rangeTypeDropDown.options[rangeTypeDropDown.value].text;
    //     CheckRestrictions(rangeType: temp);
    // }

    public void PlayCoinAnimation()
    {
        // CoinAnimation.GetComponent<Animation>().Play();
        CoinAnimation.GetComponent<Animator>().SetTrigger("playOnce");
    }






}
