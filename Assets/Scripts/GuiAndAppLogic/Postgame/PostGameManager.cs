using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class PostGameManager : MonoBehaviour
{
    [BoxGroup("main Scroll")][SerializeField] GameObject mainScroll;
    [BoxGroup("main Scroll")][SerializeField] GameObject mainScrollContents;

    [BoxGroup("experience")][SerializeField] GameObject experiencePanel;
    [BoxGroup("experience")][SerializeField] GameObject experiencePanelContents;
    [BoxGroup("treasure")][SerializeField] GameObject treasureTrackerPanel;
    [BoxGroup("treasure")][SerializeField] GameObject treasureTrackerContents;
    [BoxGroup("treasure")][SerializeField] GameObject treasureSelectionPopup;
    [BoxGroup("treasure")][SerializeField] GameObject treasureSelectionPopupContents;
    [BoxGroup("treasure")][SerializeField] GameObject itemDescriptionPopup;
    [BoxGroup("treasure")][SerializeField] GameObject treasureFinalizerPanel;
    [BoxGroup("treasure")][SerializeField] GameObject treasureFinalizerPanelContents;



    [BoxGroup("injury")][SerializeField] GameObject injuryPanel;
    [BoxGroup("injury")][SerializeField] GameObject injuryPanelContents;
    [BoxGroup("injury")][SerializeField] GameObject cureSelectionPopup;
    
    [BoxGroup("prefabs")][SerializeField] GameObject displayElementPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject basicButtonPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject postgameSoldierButtonPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject modNumberPanelPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject treasureSelectWindowPrefab;

    [SerializeField] TreasureGenerator treasureGenerator;

    private PlayerWarband currentWarband;
    private RuntimeGameInfo lastGameInfo;

    private PostgameSoldierButton currentlySelectedSoldier;
    private int currentStep;
    private GameObject currentTreasureTrackerButton;

   
    public void Init(PlayerWarband _playerWarband, RuntimeGameInfo _runtimeGameInfo)
    {
        currentWarband = _playerWarband;
        lastGameInfo = _runtimeGameInfo;
        currentStep = 0;
        HandleCurrentStep();
    }

    #region CreateAndAttach

    private void CreateDisplayElementAndAttach(string display, GameObject parent)
    {

    }

    private void CreateBasicButtonAndAttach(string buttonText, GameObject parent, UnityEngine.Events.UnityAction call)
    {
        GameObject temp = Instantiate(basicButtonPrefab);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        temp.GetComponent<Button>().onClick.AddListener(call);
        temp.name = buttonText;
        temp.transform.SetParent(parent.transform);
    }
    private void CreateTreasureTypeButtonAndAttach(string buttonText, GameObject parent)
    {
        GameObject temp = Instantiate(basicButtonPrefab);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        temp.GetComponent<Button>().onClick.AddListener(delegate {SelectTreasureType(buttonText);});
        temp.name = buttonText;
        temp.transform.SetParent(parent.transform);
    }
    private void CreateTreasureSelectButtonAndAttach(string buttonText, GameObject parent)
    {
        GameObject temp = Instantiate(basicButtonPrefab);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        temp.GetComponent<Button>().onClick.AddListener(delegate {TreasureTrackerEvent(temp);});
        temp.name = buttonText;
        temp.transform.SetParent(parent.transform);
    }

    private void CreateSoldierButtonAndAttach(string buttonText, GameObject parent, RuntimeSoldierData rsd)
    {
        GameObject temp = Instantiate(postgameSoldierButtonPrefab);
        PostgameSoldierButton psb = temp.GetComponent<PostgameSoldierButton>();
        psb.Init(rsd);
        psb.UpdateText(buttonText);
        temp.GetComponent<Button>().onClick.AddListener(delegate {SelectSoldierEvent(psb);});
        temp.transform.SetParent(parent.transform);
    }

    private void CreateExperienceNumberPanelAndAttach(string panelName, GameObject parent, int panelStartingMod = 0)
    {
        GameObject temp = Instantiate(modNumberPanelPrefab);
        ModNumberPanel mod = temp.GetComponent<ModNumberPanel>();
        mod.Init(panelName,panelStartingMod);
        temp.transform.SetParent(parent.transform);
    }
    #endregion

    #region Injuries

    private void FillMainWithSoldierInjuries()
    {
        //reset any badly wounded in bonus members
        foreach(var soldier in currentWarband.warbandBonusSoldiers)
        {
            if(soldier.status == SoldierStatus.BadlyWounded)
            {
                soldier.status = SoldierStatus.ready;
            }
        }

        foreach(var soldier in currentWarband.warbandSoldiers)
        {
            //reset all soldiers that missed last game
            if(soldier.status == SoldierStatus.BadlyWounded)
            {
                soldier.status = SoldierStatus.ready;
            }
            
            //decide soldier fate
            if(soldier.status == SoldierStatus.knockout && soldier.soldierType != "Apprentice")
            {
                int currentRoll = Random.Range(1, 20);
                string statusDisplay;
                if(currentRoll < 5)
                {
                    soldier.status = SoldierStatus.Dead;
                    statusDisplay = "<color=red>Dead</color>";
                }
                else if(currentRoll > 5 && currentRoll < 9)
                {
                    soldier.status = SoldierStatus.BadlyWounded;
                    statusDisplay = "<color=orange>Badly Wounded</color>";
                }
                else{
                    soldier.status = SoldierStatus.ready;
                    statusDisplay = "<color=green>Full Recovery</color>";
                }
                string fullDisplay = soldier.soldierName + " | " + statusDisplay;
                CreateSoldierButtonAndAttach(fullDisplay, mainScrollContents, soldier);
            }
        }

        //we'll add in bonus soldiers here too for attempted healing if they are dead
        foreach(var soldier in currentWarband.warbandBonusSoldiers)
        {
            if(soldier.status == SoldierStatus.Dead)
            {
                string statusDisplay = "<color=red>Dead</color>";
                string fullDisplay = soldier.soldierName + " | " + statusDisplay;
                CreateSoldierButtonAndAttach(fullDisplay, mainScrollContents, soldier);
            }
        }
    }

    private void FinalizeWarbandInjuries()
    {
        

        //convenient linq query
        currentWarband.warbandSoldiers.RemoveAll(item => item.status == SoldierStatus.Dead);

        // for(int i = currentWarband.warbandSoldiers.Count; i >=0; i--)
        // {

        // }

        //whoops, hehe, bad programming
        // foreach(var soldier in currentWarband.warbandSoldiers)
        // {
        //     if(soldier.status == SoldierStatus.Dead)
        //     {
        //         currentWarband.warbandSoldiers.Remove(soldier);
        //     }
        // }
    }


    private void FillCureSelectionPopup()
    {
        foreach(var spell in currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            if(spell.referenceSpell.Name == "Miraculous Cure")
            {
                CreateBasicButtonAndAttach("Miraculous Cure", cureSelectionPopup, delegate {MiraculousCureEvent(spell);});
                break;
            }
        }

        foreach(var item in currentWarband.warbandVault)
        {
            if(item.itemName == "Potion of Preservation")
            {
                CreateBasicButtonAndAttach("Potion of Preservation", cureSelectionPopup, delegate {PreservationPotionEvent();});
            }
            if(item.itemName == "Elixir of Life")
            {
                CreateBasicButtonAndAttach("Elixir of Life", cureSelectionPopup, delegate {ElixirOfLifeEvent();});
            }
        }
    }

    public void MiraculousCureEvent(WizardRuntimeSpell spell)
    {
        //choose type of cure (injury or dead)
        //make spell roll and show result
    }
    public void PreservationPotionEvent()
    {
        //move to extra slot and keep dead status
    }
    public void ElixirOfLifeEvent()
    {
        //set status to living
    }

    public void CureSelectionPopupEvent()
    {
        cureSelectionPopup.SetActive(true);
    }

    //we will come back to this, will take lots of stuff to do
    public void SelectSoldierEvent(PostgameSoldierButton psb)
    {
        // currentlySelectedSoldier = psb;
        // CureSelectionPopupEvent();
    }
    #endregion

    public void FindButtonInWindowAndDestroy(string buttonName, GameObject window)
    {
        foreach(Transform child in window.transform)
        {
            if(child.gameObject.name == buttonName)
            {
                Destroy(child);
                break;
            }
        }
    }

    public void OnClickNextButton()
    {
        currentStep++;
        HandleCurrentStep();
    }

    private void HandleCurrentStep()
    {
        if(currentStep == 0)
        {
            mainScroll.SetActive(true);
            FillMainWithSoldierInjuries();
        }
        if(currentStep == 1)
        {
            FinalizeWarbandInjuries();
            mainScroll.SetActive(false);
            experiencePanel.SetActive(true);
            SetUpExperiencePanel();
        }
        if(currentStep == 2)
        {
            FinalizeExperienceGained();
            experiencePanel.SetActive(false);

            treasureTrackerPanel.SetActive(true);
            FillTreasureTracker();
            FillTreasureSelectionPopup();
        }
        if(currentStep == 3)
        {
            // treasureTrackerPanel.SetActive(false);

            //check for illegal treasure combinations (multiple central treasures for ex)

            treasureFinalizerPanel.SetActive(true);
            FillTreasureFinalizer();
        }
    }

    public void ResetCurrentStep()
    {

    }

    #region Experience
    public void AddCustomExperienceButton()
    {
        CreateExperienceNumberPanelAndAttach("Custom XP", experiencePanelContents);
    }

    private void SetUpExperiencePanel()
    {
        CreateExperienceNumberPanelAndAttach("Creatures Killed", experiencePanelContents, lastGameInfo.GetCreaturesKilled());
        CreateExperienceNumberPanelAndAttach("Spells Passed", experiencePanelContents, lastGameInfo.GetSpellsPassed());
        CreateExperienceNumberPanelAndAttach("Spells Failed", experiencePanelContents, lastGameInfo.GetSpellsFailed());
        CreateExperienceNumberPanelAndAttach("Treasure Captured", experiencePanelContents, lastGameInfo.GetTreasuresCaptured());
    }

    private void FinalizeExperienceGained()
    {
        int totalXpGained = 40; //always 40 per game
        foreach(Transform item in experiencePanelContents.transform)
        {
            ModNumberPanel temp = item.GetComponent<ModNumberPanel>();
            totalXpGained += GenerateExperienceBasedOnName(temp.GetPanelName(), temp.GetModNumberValue());
        }
        if(totalXpGained > 300)
        {
            totalXpGained = 300;
        }
        currentWarband.warbandWizard.playerWizardExperience += totalXpGained;
    }

    private int GenerateExperienceBasedOnName(string name, int amount)
    {
        int total = 0;
        if(name == "Creatures Killed")
        {
            total = amount * 5;
            if(total > 50)
            {
                total = 50;
            }
        }
        else if(name == "Spells Passed")
        {
            total = amount * 10;
        }
        else if(name == "Spells Failed")
        {
            total = amount * 5;
        }
        else if(name == "Treasure Captured")
        {
            total = amount * 40;
        }
        else if(name == "Custom XP")
        {
            total = amount;
        }
        
        return total;
    }
    #endregion

    #region Treasure
    private void FillTreasureTracker()
    {
        for(int i =0;i < lastGameInfo.GetTreasuresCaptured();i++)
        {
            CreateTreasureSelectButtonAndAttach("Normal",treasureTrackerContents);
        }
    }

    private void FillTreasureSelectionPopup()
    {
        CreateTreasureTypeButtonAndAttach("Normal",treasureSelectionPopupContents);
        CreateTreasureTypeButtonAndAttach("Central",treasureSelectionPopupContents);
        CreateTreasureTypeButtonAndAttach("Reveal Secret",treasureSelectionPopupContents);
        //later for each campaign enabled, add option
    }

    public void SelectTreasureType(string treasureType)
    {
        currentTreasureTrackerButton.GetComponentInChildren<TextMeshProUGUI>().text = treasureType;
        // FindButtonInWindowAndDestroy(treasureType, treasureSelectionPopupContents);
        treasureSelectionPopup.SetActive(false);
    }

    public void TreasureTrackerEvent(GameObject go)
    {
        treasureSelectionPopup.SetActive(true);
        currentTreasureTrackerButton = go;
    }
    public void OnClickAddTreasure()
    {
        CreateTreasureSelectButtonAndAttach("Normal",treasureTrackerContents);
    }

    private void FillTreasureFinalizer()
    {
        /*
        for each treasure in previous window
            add treasure select window
            create group
            fill group with items from randomized rolls on treasure table
            add gold amount basic button with no function, track gold with group name

            basic treasure no extra
            central, add reroll button that triggers removing the group and readding it without reroll
            secret, add extra group, add select button which deletes the other group

        on next
            then on each window, get all items from first group of each window and add to vault

        groups need to be tracked here locally, including gold amounts
            
        */
        Debug.Log("in fill treasure finalizer");

        foreach(Transform child in treasureTrackerContents.transform)
        {
            Debug.Log("loop with child: " + child.name.ToString());
            string name = child.gameObject.name;
            GameObject temp = Instantiate(treasureSelectWindowPrefab);
            RuntimeTreasure tempTreasure = treasureGenerator.GenerateTreasureCoreBook();
            TreasureSelectWindow selectWindow = temp.GetComponent<TreasureSelectWindow>();
            selectWindow.AddItemGroup("basic", tempTreasure);

            //add gold amount if theres any
            if(tempTreasure.goldAmount > 0)
            {
                GameObject goldObject = Instantiate(basicButtonPrefab);
                basicButtonPrefab.GetComponentInChildren<TextMeshProUGUI>().text = tempTreasure.goldAmount.ToString();
                selectWindow.AddObjectToGroup(goldObject, "basic");
            }
            
            //add each item to the window
            foreach(var item in tempTreasure.items)
            {
                selectWindow.AddItem(item, "basic", delegate {ItemDescriptionPopupEvent(item);});
            }

            temp.transform.SetParent(treasureFinalizerPanelContents.transform);
        }
    }

    private void CreateAndAddTreasureSelectWindowToWindow(string treasureType, GameObject parent)
    {
        GameObject temp = Instantiate(treasureSelectWindowPrefab);
        
    }

    public void ItemDescriptionPopupEvent(MagicItemScriptable item)
    {
        itemDescriptionPopup.SetActive(true);
        itemDescriptionPopup.GetComponent<ItemDescriptionPopup>().Init(item);
    }



    #endregion

    

}
