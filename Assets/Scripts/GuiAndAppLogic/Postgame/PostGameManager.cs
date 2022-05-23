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

    [BoxGroup("wizard")][SerializeField] GameObject wizardLevelTogglesPanel;
    [BoxGroup("wizard")][SerializeField] Toggle fightToggle;
    [BoxGroup("wizard")][SerializeField] Text fightValueLabel;
    [BoxGroup("wizard")][SerializeField] Toggle shootToggle;
    [BoxGroup("wizard")][SerializeField] Text shootValueLabel;
    [BoxGroup("wizard")][SerializeField] Toggle willToggle;
    [BoxGroup("wizard")][SerializeField] Text willValueLabel;
    [BoxGroup("wizard")][SerializeField] Toggle healthToggle;
    [BoxGroup("wizard")][SerializeField] Text healthValueLabel;
    [BoxGroup("wizard")][SerializeField] ToggleGroup allToggles;
    // [BoxGroup("wizard")][SerializeField] GameObject wizardSpellLevelPanel;
    // [BoxGroup("wizard")][SerializeField] GameObject wizardSpellLevelContents;
    // [BoxGroup("wizard")][SerializeField] GameObject spellCheckButtonPrefab;
    [BoxGroup("wizard")][SerializeField] SpellSelectionHandler spellSelectionHandler;







    [BoxGroup("injury")][SerializeField] GameObject injuryPanel;
    [BoxGroup("injury")][SerializeField] GameObject injuryPanelContents;
    [BoxGroup("injury")][SerializeField] GameObject cureSelectionPopup;
    
    [BoxGroup("prefabs")][SerializeField] GameObject displayElementPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject basicButtonPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject postgameSoldierButtonPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject modNumberPanelPrefab;
    [BoxGroup("prefabs")][SerializeField] GameObject treasureSelectWindowPrefab;

    [SerializeField] TreasureGenerator treasureGenerator;
    [SerializeField] WarbandUIManager warbandUIManager;

    private PlayerWarband currentWarband;
    private RuntimeGameInfo lastGameInfo;

    private PostgameSoldierButton currentlySelectedSoldier;
    private int currentStep;
    private GameObject currentTreasureTrackerButton;

    [SerializeField] int testExperience = 0;

   
    public void Init(PlayerWarband _playerWarband, RuntimeGameInfo _runtimeGameInfo)
    {
        currentWarband = _playerWarband;
        lastGameInfo = _runtimeGameInfo;
        currentStep = 0;
        currentWarband.warbandWizard.playerWizardExperience += testExperience;
        HandleCurrentStep();
    }

    public void EnableSkipButton()
    {

    }

    public void DisableSkipButton()
    {

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
        else if(currentStep == 1)
        {
            FinalizeWarbandInjuries();
            mainScroll.SetActive(false);
            experiencePanel.SetActive(true);
            SetUpExperiencePanel();
        }
        else if(currentStep == 2)
        {
            FinalizeExperienceGained();
            experiencePanel.SetActive(false);

            treasureTrackerPanel.SetActive(true);
            FillTreasureTracker();
            FillTreasureSelectionPopup();
        }
        else if(currentStep == 3)
        {
            // treasureTrackerPanel.SetActive(false);

            //check for illegal treasure combinations (multiple central treasures for ex)
            treasureFinalizerPanel.SetActive(true);
            FillTreasureFinalizer();
            treasureTrackerPanel.SetActive(false);
        }
        else if(currentStep == 4)
        {
            FinalizeTreasure();
            treasureFinalizerPanel.SetActive(false);

            wizardLevelTogglesPanel.SetActive(true);
            SetUpWizardLevelToggles();
            
        }
        else if(currentStep == 5)
        {
            FinalizeLevelToggles();
            wizardLevelTogglesPanel.SetActive(false);

            mainScroll.SetActive(true);
            ClearMainContent();
            SetUpWizardSpellLeveler(); 
        }
        else if(currentStep == 6)
        {
           FinalizeWizardSpellLeveler();
           ClearMainContent();
           SetUpWizardSpellLearner(); 
        }
        else if(currentStep == 7)
        {
           FinalizeWizardSpellLearner();
        }
        else{
            warbandUIManager.SaveWarbandChanges();
            warbandUIManager.BackToWarbandMain();
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
        // Debug.Log("in fill treasure finalizer");

        foreach(Transform child in treasureTrackerContents.transform)
        {
            // Debug.Log("loop with child: " + child.name.ToString());
            string name = child.GetComponentInChildren<TextMeshProUGUI>().text;
            GameObject temp = Instantiate(treasureSelectWindowPrefab);
            RuntimeTreasure tempTreasure = treasureGenerator.GenerateTreasureCoreBook();
            TreasureSelectWindow selectWindow = temp.GetComponent<TreasureSelectWindow>();
            selectWindow.Init(name, this);

            if(name == "Normal")
            {
                selectWindow.AddItemGroup("Normal", tempTreasure, TreasureSelectGroupType.normal);
                
            }
            if(name == "Central")
            {
                selectWindow.AddItemGroup("Central", tempTreasure, TreasureSelectGroupType.central);
            }
            if(name == "Reveal Secret")
            {
                selectWindow.AddItemGroup("secret1", tempTreasure, TreasureSelectGroupType.secret);
                tempTreasure = treasureGenerator.GenerateTreasureCoreBook();
                selectWindow.AddItemGroup("secret2", tempTreasure, TreasureSelectGroupType.secret);
            }
        
            temp.transform.SetParent(treasureFinalizerPanelContents.transform);
        }
    }

    private void FinalizeTreasure()
    {

        foreach(Transform child in treasureFinalizerPanelContents.transform)
        {
            TreasureSelectWindow selectWindow = child.GetComponent<TreasureSelectWindow>();
            var tempTreasure = selectWindow.GetFinalTreasure();
            Debug.Log("adding treasure from treasure window");
            currentWarband.warbandVault.AddRange(tempTreasure.items);
            currentWarband.warbandGold += tempTreasure.goldAmount;
        }
    }

    //for deleting the other group in the window
    public void SelectTreasureGroup(TreasureSelectWindow tsw, string groupName)
    {

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

    public RuntimeTreasure RollTreasure()
    {
        return treasureGenerator.GenerateTreasureCoreBook();
    }



    #endregion


    #region Wizardleveling
    private void SetUpWizardLevelToggles()
    {
        //need to set each toggle's stat, and check if its at max and disable if it is

        fightValueLabel.text = currentWarband.warbandWizard.playerWizardProfile.fight.ToString();
        shootValueLabel.text = currentWarband.warbandWizard.playerWizardProfile.shoot.ToString();
        willValueLabel.text = currentWarband.warbandWizard.playerWizardProfile.will.ToString();
        healthValueLabel.text = currentWarband.warbandWizard.playerWizardProfile.health.ToString();

        if(currentWarband.warbandWizard.playerWizardExperience < 100)
        {
            fightToggle.interactable = false;
            shootToggle.interactable = false;
            willToggle.interactable = false;
            healthToggle.interactable = false;
        }
        else{
            if(currentWarband.warbandWizard.playerWizardProfile.fight >= 5)
            {
                fightToggle.interactable = false;
            }
            if(currentWarband.warbandWizard.playerWizardProfile.shoot >= 5)
            {
                shootToggle.interactable = false;
            }
            if(currentWarband.warbandWizard.playerWizardProfile.will >= 8)
            {
                willToggle.interactable = false;
            }
            if(currentWarband.warbandWizard.playerWizardProfile.health >= 20)
            {
                healthToggle.interactable = false;
            }
        }
        
    }

    private void FinalizeLevelToggles()
    {
        if(allToggles.AnyTogglesOn())
        {
            Toggle activeToggle = allToggles.GetFirstActiveToggle();
            string name = activeToggle.name;
            // string name = activeToggle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
            if(name == "FightToggle")
            {
                currentWarband.warbandWizard.playerWizardProfile.fight += 1;
            }
            else if(name == "ShootToggle")
            {
                currentWarband.warbandWizard.playerWizardProfile.shoot += 1;
            }
            else if(name == "WillToggle")
            {
                currentWarband.warbandWizard.playerWizardProfile.will += 1;
            }
            else if(name == "HealthToggle")
            {
                currentWarband.warbandWizard.playerWizardProfile.health += 1;
            }
            currentWarband.warbandWizard.playerWizardExperience -= 100;
        }
    }

    private void SetUpWizardSpellLeveler()
    {
        spellSelectionHandler.SetMaxSelectable(1);
        spellSelectionHandler.GenerateContainersForSpellsFromWizardSpellbook(currentWarband.warbandWizard.playerWizardSpellbook);
        if(currentWarband.warbandWizard.playerWizardExperience < 100)
        {
            spellSelectionHandler.DisableAllUntoggledToggles();
            // CreateBasicButtonAndAttach("Not Enough Experience", mainScrollContents, null);
        }
    }


    private void FinalizeWizardSpellLeveler()
    {
        List<SpellScriptable> selectedSpell = spellSelectionHandler.GetCurrentlySelectedSpells();
        if(selectedSpell.Count > 0)
        {
            foreach(var item in currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
            {
                if(item.referenceSpell == selectedSpell[0])
                {
                    item.currentWizardLevelMod--;
                    currentWarband.warbandWizard.playerWizardExperience -= 100;
                    break;
                }
            }
        }
        
    }

    private void SetUpWizardSpellLearner()
    {
        int maxPossibleCanLearn = (int)Mathf.Round((currentWarband.warbandWizard.playerWizardExperience / 100));
        if(maxPossibleCanLearn <= 0)
        {
            // CreateBasicButtonAndAttach("Not Enough Experience to Learn Spells", mainScrollContents, null);
        }
        else{
            spellSelectionHandler.SetMaxSelectable(maxPossibleCanLearn);
            spellSelectionHandler.GenerateContainersForGrimoiresInWizardVault(currentWarband.warbandVault, currentWarband.warbandWizard.playerWizardSpellbook);
        }
        

        
        
    }

    private void FinalizeWizardSpellLearner()
    {
        List<SpellScriptable> selectedSpells = spellSelectionHandler.GetCurrentlySelectedSpells();
        foreach(var spell in selectedSpells)
        {
            WizardRuntimeSpell wrs = new WizardRuntimeSpell();
            wrs.Init(spell);
            currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells.Add(wrs);
            currentWarband.warbandWizard.playerWizardExperience -= 100;
            RemoveGrimoireOfSpellFromVault(spell);
        }
    }

    private void RemoveGrimoireOfSpellFromVault(SpellScriptable spell)
    {
        int count = 0;
        bool found = false;
        foreach(var item in currentWarband.warbandVault)
        {
            if(item.itemType == MagicItemType.Grimoire)
            {
                if(item.itemName.Contains(spell.Name))
                {
                    found = true;
                    break;
                }
            }
            count++;
        }
        if(found)
        {
            currentWarband.warbandVault.RemoveAt(count);
        }
    }

    #endregion

    public void ClearMainContent()
    {
        foreach(Transform item in mainScrollContents.transform)
        {
            Destroy(item.gameObject);
        }
    }

    

}
