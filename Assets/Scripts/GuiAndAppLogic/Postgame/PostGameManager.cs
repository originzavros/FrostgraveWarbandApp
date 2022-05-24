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
    [BoxGroup("wizard")][SerializeField] Toggle invisibleToggle;//for the purpose of letting the player not select something here

    [BoxGroup("wizard")][SerializeField] ToggleGroup allToggles;
    // [BoxGroup("wizard")][SerializeField] GameObject wizardSpellLevelPanel;
    // [BoxGroup("wizard")][SerializeField] GameObject wizardSpellLevelContents;
    // [BoxGroup("wizard")][SerializeField] GameObject spellCheckButtonPrefab;
    [BoxGroup("wizard")][SerializeField] SpellSelectionHandler spellSelectionHandler;







    [BoxGroup("injury")][SerializeField] GameObject injuryPanel;
    [BoxGroup("injury")][SerializeField] GameObject injuryPanelContents;
    [BoxGroup("injury")][SerializeField] GameObject cureSelectionPopup;
    [BoxGroup("injury")][SerializeField] GameObject cureSelectionPopupContents;
    [BoxGroup("injury")][SerializeField] GameObject cureResultPopup;

    
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
    [SerializeField][ReadOnly] private int currentStep;
    private GameObject currentTreasureTrackerButton;

    [SerializeField] int testExperience = 0;
    [SerializeField] bool testGiveInjuryToWizard = false;
    [SerializeField] bool testGiveElixirOfLifeToWarband = false;
    [SerializeField] bool testGivePreservationPotionToWarband = false;

   
    public void Init(PlayerWarband _playerWarband, RuntimeGameInfo _runtimeGameInfo)
    {
        currentWarband = _playerWarband;
        lastGameInfo = _runtimeGameInfo;
        currentStep = 0;
        currentWarband.warbandWizard.playerWizardExperience += testExperience;
        if(testGiveElixirOfLifeToWarband){ giveItemWithNameToWarband("Elixir Of Life");}
        if(testGivePreservationPotionToWarband){ giveItemWithNameToWarband("Potion Of Preservation");}
        getTreasuresFromEscapedSoldiers();
        HandleCurrentStep();
    }

    private void giveItemWithNameToWarband(string name)
    {
        foreach(var item in LoadAssets.allMagicItemObjects)
        {
            if(item.itemName == name)
            {
                currentWarband.warbandVault.Add(item);
                break;
            }
        }
    }

    private void getTreasuresFromEscapedSoldiers()
    {
        foreach(var soldier in currentWarband.warbandSoldiers)
        {
            if(soldier.status == SoldierStatus.escapedWithTreasure)
            {
                lastGameInfo.CaptureTreasure();
                soldier.status = SoldierStatus.ready;
            }
            else if(soldier.status == SoldierStatus.escaped)
            {
                soldier.status = SoldierStatus.ready;
            }
        }

        if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.escapedWithTreasure)
        {
            currentWarband.warbandWizard.playerWizardProfile.status = SoldierStatus.ready;
            lastGameInfo.CaptureTreasure();
        }
        else if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.escaped)
        {
            currentWarband.warbandWizard.playerWizardProfile.status = SoldierStatus.ready;
        }
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
            if(soldier.status == SoldierStatus.badlyWounded)
            {
                soldier.status = SoldierStatus.ready;
            }
        }

        foreach(var soldier in currentWarband.warbandSoldiers)
        {
            //reset all soldiers that missed last game
            if(soldier.status == SoldierStatus.badlyWounded)
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
                    soldier.status = SoldierStatus.dead;
                    statusDisplay = "<color=red>Dead</color>";
                }
                else if(currentRoll > 5 && currentRoll < 9)
                {
                    soldier.status = SoldierStatus.badlyWounded;
                    statusDisplay = "<color=orange>Badly Wounded</color>";
                }
                else{
                    soldier.status = SoldierStatus.ready;
                    statusDisplay = "<color=green>Full Recovery</color>";
                }
                string fullDisplay = soldier.soldierName + " | " + statusDisplay;
                CreateSoldierButtonAndAttach(fullDisplay, mainScrollContents, soldier);
            }
            else if(soldier.status == SoldierStatus.knockout && soldier.soldierType == "Apprentice")
            {
                StatusRollForSpellcaster(soldier);
            }
        }

        if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.knockout)
        {
            StatusRollForSpellcaster(currentWarband.warbandWizard.playerWizardProfile);
        }

        //we'll add in bonus soldiers here too for attempted healing if they are dead
        foreach(var soldier in currentWarband.warbandBonusSoldiers)
        {
            if(soldier.status == SoldierStatus.preserved)
            {
                string statusDisplay = "<color=orange>Preserved</color>";
                string fullDisplay = soldier.soldierName + " | " + statusDisplay;
                CreateSoldierButtonAndAttach(fullDisplay, mainScrollContents, soldier);
            }
        }
    }

    private void StatusRollForSpellcaster(RuntimeSoldierData soldier)
    {
        int currentRoll = Random.Range(1, 20);
        string statusDisplay;
        if(currentRoll < 3)
        {
            if(soldier.soldierType == "Wizard") //wizards get +1 to their survival roll
            {
                if(currentRoll < 2)
                {
                    soldier.status = SoldierStatus.dead;
                    statusDisplay = "<color=red>Dead</color>";
                }
                else{
                    soldier.status = SoldierStatus.injured;
                    statusDisplay = "<color=red>Injured</color>";
                }
            }
            else{
                soldier.status = SoldierStatus.dead;
                statusDisplay = "<color=red>Dead</color>";
            } 
        }
        else if(currentRoll > 2 && currentRoll < 5)
        {
            soldier.status = SoldierStatus.injured;
            statusDisplay = "<color=red>Injured</color>";
        }
        else if(currentRoll > 4 && currentRoll < 7)
        {
            soldier.status = SoldierStatus.badlyWounded;
            statusDisplay = "<color=orange>Badly Wounded</color>";
        }
        else if(currentRoll > 6 && currentRoll < 9)
        {
            soldier.status = SoldierStatus.closeCall;
            statusDisplay = "<color=yellow>Close Call</color>";
        }
        else{
            soldier.status = SoldierStatus.ready;
            statusDisplay = "<color=green>Full Recovery</color>";
        }
        string fullDisplay = soldier.soldierName + " | " + statusDisplay;
        CreateSoldierButtonAndAttach(fullDisplay, mainScrollContents, soldier);
    }

    //Everyone keeps their status, for dead wizards, they simply stay dead, in case the player wants to continue playing with them
    private void FinalizeWarbandInjuries()
    {
        //convenient linq query
        currentWarband.warbandSoldiers.RemoveAll(item => item.status == SoldierStatus.dead);

        // for(int i = currentWarband.warbandSoldiers.Count; i > 0 ; i--)
        // {
        //     if(currentWarband.warbandBonusSoldiers[i].status == SoldierStatus.dead)
        //     {
        //         currentWarband.warbandBonusSoldiers.RemoveAt(i);
        //     }
        // }

        if(testGiveInjuryToWizard)
        {
            GiveInjuryToSoldier(currentWarband.warbandWizard.playerWizardProfile);
        }

        RuntimeSoldierData foundApprentice = null;
        int apothecaryCount = 0;

        foreach(var soldier in currentWarband.warbandSoldiers)
        {
            if(soldier.soldierType == "Apprentice")
            {
                foundApprentice = soldier;
            }
            else if(soldier.soldierType == "Apothecary")
            {
                apothecaryCount++;
            }
            
        }

        if(foundApprentice != null)
        {
            if(foundApprentice.status == SoldierStatus.injured)
            {
                GiveInjuryToSoldier(foundApprentice);
            }
            else if(foundApprentice.status == SoldierStatus.closeCall)
            {
                foundApprentice.soldierInventory.Clear();
            }
            else if(foundApprentice.status == SoldierStatus.badlyWounded)
            {
                if(apothecaryCount > 0)
                {
                    currentWarband.warbandGold -= 50;
                }
                else{
                    currentWarband.warbandGold -= 150;
                }
            }
        }

        if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.injured)
        {
            GiveInjuryToSoldier(currentWarband.warbandWizard.playerWizardProfile);
        }
        else if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.closeCall)
        {
            currentWarband.warbandWizard.playerWizardProfile.soldierInventory.Clear();
        }
        else if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.badlyWounded)
        {
            if(apothecaryCount > 0)
            {
                currentWarband.warbandGold -= 50;
            }
            else{
                currentWarband.warbandGold -= 150;
            }
        }
        else if(currentWarband.warbandWizard.playerWizardProfile.status == SoldierStatus.dead)
        {
            if(currentWarband.warbandWizard.playerWizardLevel < 6)
            {
                cureResultPopup.SetActive(true);
                cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Wizard died and was below lvl 6, recommend starting new warband");
            }
            else{
                if(foundApprentice != null)
                {
                    currentWarband.warbandWizard.playerWizardProfile = foundApprentice;
                    currentWarband.warbandWizard.playerWizardProfile.soldierType = "Wizard";
                    currentWarband.warbandWizard.playerWizardLevel -= 6;

                    currentWarband.warbandSoldiers.Remove(foundApprentice);
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Wizard died and was replaced by apprentice, wizard level reduced by 6");
                }
                else{
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Wizard died and no apprentice to replace him, recommend starting new warband");
                }
            }
        }
        //do injury rolls for spellcasters with injured status, check if they receive lost eye again and give dead status
        //spellcasters with close call lose all their equipped items
        //spellcaster with badly wounded have option to pay 150(50 with apothecary) to go to ready status (can go into negative gold);
        //if the wizard died and was above level 5 the apprentice becomes the wizard and remove the apprentice if they have one. reset new wizard to lvl 0.
        //if they don't have apprentice or wizard was below lvl 6, retire the warband
    }

    private void GiveInjuryToSoldier(RuntimeSoldierData soldier)
    {
        int currentRoll = Random.Range(1, 20);
        if(currentRoll < 19)
        {
            bool usable = false;
            while(!usable)
            {
                InjuryScriptable currentInjury = GenerateRandomInjury();
                if(currentInjury.injuryName != "Smashed Jaw" || currentInjury.injuryName != "Lost Eye")
                {
                    int total = 0;
                    foreach(var item in soldier.soldierPermanentInjuries)
                    {
                        if(item.injuryName == currentInjury.injuryName)
                        {
                            total++;
                        }
                    }
                    if(total < currentInjury.injuryMax)
                    {
                        soldier.soldierPermanentInjuries.Add(currentInjury);
                        usable = true;
                    }
                }
            }
            
        }
        else if(currentRoll == 19)
        {
            soldier.soldierPermanentInjuries.Add(GetInjuryScriptableByName("Smashed Jaw"));
        }
        else if(currentRoll == 20)
        {
            soldier.soldierPermanentInjuries.Add(GetInjuryScriptableByName("Lost Eye"));
        }
    }
    private InjuryScriptable GetInjuryScriptableByName(string name)
    {
        foreach(var item in LoadAssets.allInjuries)
        {
            if(item.injuryName == name)
            {
                return item;
            }
        }
        return null;
    }
    private InjuryScriptable GenerateRandomInjury()
    {
        int currentRoll = Random.Range(0, LoadAssets.allInjuries.Length);
        return LoadAssets.allInjuries[currentRoll];
    }

    private void FillCureSelectionPopup()
    {
        foreach(var spell in currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            if(spell.referenceSpell.Name == "Miraculous Cure")
            {
                CreateBasicButtonAndAttach("Miraculous Cure", cureSelectionPopupContents, delegate {MiraculousCureEvent(spell);});
                break;
            }
        }

        foreach(var item in currentWarband.warbandVault)
        {
            if(item.itemName == "Potion Of Preservation")
            {
                CreateBasicButtonAndAttach("Potion Of Preservation", cureSelectionPopupContents, delegate {PreservationPotionEvent();});
            }
            if(item.itemName == "Elixir Of Life")
            {
                CreateBasicButtonAndAttach("Elixir Of Life", cureSelectionPopupContents, delegate {ElixirOfLifeEvent();});
            }
        }
    }

    public void MiraculousCureEvent(WizardRuntimeSpell spell)
    {
        RuntimeSoldierData rsd = currentlySelectedSoldier.GetStoredSoldier();
        if(rsd.soldierType == "Wizard" || rsd.soldierType == "Apprentice")
        {
            if(rsd.status == SoldierStatus.dead || rsd.status == SoldierStatus.preserved)
            {
                if(SpellRoller.MakeRollForSpell(spell, -4))
                {
                    rsd.status = SoldierStatus.ready;
                    string output = rsd.soldierName + " has been brought back to life";
                    string fullDisplay = rsd.soldierName + " | " + "<color=green>Full Recovery</color>";
                    currentlySelectedSoldier.UpdateText(fullDisplay);
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
                }
                else{
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Miraculous Cure Failed to bring spellcaster back to life");
                }

            }
            else{
                if(rsd.status == SoldierStatus.injured)
                {
                    if(SpellRoller.MakeRollForSpell(spell))
                    {
                        rsd.status = SoldierStatus.ready;
                        string output = rsd.soldierName + " has recovered from their current injury";
                        string fullDisplay = rsd.soldierName + " | " + "<color=green>Full Recovery</color>";
                        currentlySelectedSoldier.UpdateText(fullDisplay);
                        cureResultPopup.SetActive(true);
                        cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
                    }
                    else{
                        cureResultPopup.SetActive(true);
                        cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Miraculous Cure failed to prevent current injury");
                    }
                }
                else{
                    if(SpellRoller.MakeRollForSpell(spell))
                    {
                        string output = rsd.soldierName + " has recovered from their last injury";
                        // string fullDisplay = rsd.soldierName + " | " + "<color=green>Full Recovery</color>";
                        // currentlySelectedSoldier.UpdateText(fullDisplay);
                        cureResultPopup.SetActive(true);
                        cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
                        int lastInjury = rsd.soldierPermanentInjuries.Count;
                        if(lastInjury > 0)
                        {
                            rsd.soldierPermanentInjuries.RemoveAt(lastInjury - 1);
                        }
                    }
                    else{
                        cureResultPopup.SetActive(true);
                        cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Miraculous Cure failed to remove last injury");
                    }
                } 
            }
            FindButtonInWindowAndDisable("Miraculous Cure", cureSelectionPopupContents);
        }
        else{
            //its a dead soldier
            if(rsd.status == SoldierStatus.dead || rsd.status == SoldierStatus.preserved)
            {
                if(SpellRoller.MakeRollForSpell(spell, -4))
                {
                    rsd.status = SoldierStatus.ready;
                    string output = rsd.soldierName + " has been brought back to life";
                    string fullDisplay = rsd.soldierName + " | " + "<color=green>Full Recovery</color>";
                    currentlySelectedSoldier.UpdateText(fullDisplay);
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
                }
                else{
                    cureResultPopup.SetActive(true);
                    cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Miraculous Cure Failed to bring soldier back to life");
                }
                FindButtonInWindowAndDisable("Miraculous Cure", cureSelectionPopupContents);
            }
            else{
                cureResultPopup.SetActive(true);
                cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText("Soldier is not dead or preserved, can't use miraculous cure");
            }
        }
        cureSelectionPopup.SetActive(false);
    }

    public void PreservationPotionEvent()
    {
        RuntimeSoldierData rsd = currentlySelectedSoldier.GetStoredSoldier();
        if(rsd.status == SoldierStatus.dead)
        {
            rsd.status = SoldierStatus.preserved;
            string output = rsd.soldierName + " has been preserved and added to bench";
            string fullDisplay = rsd.soldierName + " | " + "<color=orange>Preserved</color>";
            currentlySelectedSoldier.UpdateText(fullDisplay);
            cureResultPopup.SetActive(true);
            cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);

            currentWarband.warbandBonusSoldiers.Add(rsd);
            currentWarband.warbandSoldiers.Remove(rsd);
            FindButtonInWindowAndDisable("Potion Of Preservation", cureSelectionPopupContents);
            int foundLocation = -1;
            for(int i = currentWarband.warbandVault.Count; i > 0; i--)
            {
                if(currentWarband.warbandVault[i].itemName == "Potion Of Preservation")
                {
                    foundLocation = i;
                    break;
                }
            }
            if(foundLocation > -1)
            {
                currentWarband.warbandVault.RemoveAt(foundLocation);
            }
        }
        else{
            string output = "Soldier needs to be Dead to use this potion on them";
            cureResultPopup.SetActive(true);
            cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
        }
        cureSelectionPopup.SetActive(false);    
    }
    public void ElixirOfLifeEvent()
    {
        RuntimeSoldierData rsd = currentlySelectedSoldier.GetStoredSoldier();
        if(rsd.status == SoldierStatus.dead || rsd.status == SoldierStatus.preserved)
        {
            rsd.status = SoldierStatus.ready;
            string output = rsd.soldierName + " has been brought back to life";
            string fullDisplay = rsd.soldierName + " | " + "<color=green>Full Recovery</color>";
            currentlySelectedSoldier.UpdateText(fullDisplay);
            cureResultPopup.SetActive(true);
            cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
            FindButtonInWindowAndDisable("Elixir Of Life", cureSelectionPopupContents);
            int foundLocation = -1;
            for(int i = currentWarband.warbandVault.Count; i > 0; i--)
            {
                if(currentWarband.warbandVault[i].itemName == "Elixir Of Life")
                {
                    foundLocation = i;
                    break;
                }
            }
            if(foundLocation > -1)
            {
                currentWarband.warbandVault.RemoveAt(foundLocation);
            }
        }
        else{
            string output = "Soldier needs to be Dead or Preserved";
            cureResultPopup.SetActive(true);
            cureResultPopup.GetComponent<BasicPopup>().UpdatePopupText(output);
        }
        cureSelectionPopup.SetActive(false);    
    }

    public void CureSelectionPopupEvent()
    {
        cureSelectionPopup.SetActive(true);
    }

    //we will come back to this, will take lots of stuff to do
    public void SelectSoldierEvent(PostgameSoldierButton psb)
    {
        currentlySelectedSoldier = psb;
        CureSelectionPopupEvent();
    }
    #endregion

    // public void FindButtonInWindowAndDestroy(string buttonName, GameObject window)
    // {
    //     foreach(Transform child in window.transform)
    //     {
    //         if(child.gameObject.name == buttonName)
    //         {
    //             Destroy(child);
    //             break;
    //         }
    //     }
    // }

    public void FindButtonInWindowAndDisable(string buttonName, GameObject window)
    {
        foreach(Transform child in window.transform)
        {
            if(child.gameObject.name == buttonName)
            {
                if(child.TryGetComponent<Button>(out Button gameButton))
                {
                    gameButton.interactable = false;
                    break;
                }
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
            FillCureSelectionPopup();
        }
        else if(currentStep == 1)
        {
            FinalizeWarbandInjuries();
            ClearContent(mainScrollContents);
            ClearContent(cureSelectionPopupContents);
            mainScroll.SetActive(false);
            experiencePanel.SetActive(true);
            ClearContent(experiencePanelContents);
            SetUpExperiencePanel();
        }
        else if(currentStep == 2)
        {
            FinalizeExperienceGained();
            experiencePanel.SetActive(false);

            treasureTrackerPanel.SetActive(true);
            ClearContent(treasureTrackerContents);
            FillTreasureTracker();
            ClearContent(treasureSelectionPopupContents);
            FillTreasureSelectionPopup();
        }
        else if(currentStep == 3)
        {
            // treasureTrackerPanel.SetActive(false);

            //check for illegal treasure combinations (multiple central treasures for ex)
            treasureFinalizerPanel.SetActive(true);
            ClearContent(treasureFinalizerPanelContents);
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
        else{
            FinalizeWizardSpellLearner();
            ClearMainContent();
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
        Debug.Log("current wizard xp before adding xp: " + currentWarband.warbandWizard.playerWizardExperience);
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
        Debug.Log("current wizard xp after adding xp: " + currentWarband.warbandWizard.playerWizardExperience);
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
        // FindButtonInWindowAndDisable(treasureType, treasureSelectionPopupContents);
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
            Debug.Log("not enough experience to enable toggles");
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

        invisibleToggle.isOn = true;
        
    }

    private void FinalizeLevelToggles()
    {
        if(allToggles.AnyTogglesOn())
        {
            Toggle activeToggle = allToggles.GetFirstActiveToggle();
            string name = activeToggle.name;
            // string name = activeToggle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;

            RuntimeSoldierData apprentice = null;
            foreach(var soldier in currentWarband.warbandSoldiers)
            {
                if(soldier.soldierType == "Apprentice")
                {
                    apprentice = soldier;
                }
            }

            if(name == "InvisibleToggle")
            {
                //do nothing
            }
            else{
                if(name == "FightToggle")
                {
                    if(currentWarband.warbandWizard.playerWizardProfile.fight < 5)
                    {
                        currentWarband.warbandWizard.playerWizardProfile.fight += 1;
                        if(!CheckIfSoldierIsNull(apprentice))
                        {
                            apprentice.fight += 1;
                        }
                    }
                    
                }
                else if(name == "ShootToggle")
                {
                    if(currentWarband.warbandWizard.playerWizardProfile.shoot < 5)
                    {
                        currentWarband.warbandWizard.playerWizardProfile.shoot += 1;
                        if(!CheckIfSoldierIsNull(apprentice))
                        {
                            apprentice.shoot += 1;
                        }
                    }
                }
                else if(name == "WillToggle")
                {
                    if(currentWarband.warbandWizard.playerWizardProfile.will < 8)
                    {
                        currentWarband.warbandWizard.playerWizardProfile.will += 1;
                        if(!CheckIfSoldierIsNull(apprentice))
                        {
                            apprentice.will += 1;
                        }
                    }
                    
                }
                else if(name == "HealthToggle")
                {
                    if(currentWarband.warbandWizard.playerWizardProfile.health < 20)
                    {
                        currentWarband.warbandWizard.playerWizardProfile.health += 1;
                        if(!CheckIfSoldierIsNull(apprentice))
                        {
                            apprentice.health += 1;
                        }
                    }   
                }
                currentWarband.warbandWizard.playerWizardExperience -= 100;
            }
        }
    }

    private bool CheckIfSoldierIsNull(RuntimeSoldierData rsd)
    {
        if(rsd == null)
        {
            return true;
        }
        else{
            return false;
        }
    }

    private void SetUpWizardSpellLeveler()
    {
        spellSelectionHandler.SetMaxSelectable(1);
        spellSelectionHandler.GenerateContainersForSpellsFromWizardSpellbook(currentWarband.warbandWizard.playerWizardSpellbook);
        if(currentWarband.warbandWizard.playerWizardExperience < 100)
        {
            spellSelectionHandler.DisableAllUntoggledToggles();
            CreateBasicButtonAndAttach("Not Enough Experience", mainScrollContents, null);
        }
        else{
            CreateBasicButtonAndAttach("Select 1 spell to reduce it's Casting Number", mainScrollContents, delegate {DoNothingEvent();});
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
            CreateBasicButtonAndAttach("Not Enough Experience to Learn Spells", mainScrollContents, delegate {DoNothingEvent();});
        }
        else{
            int totalGrimoires = 0;
            foreach(var item in currentWarband.warbandVault)
            {
                if(item.itemType == MagicItemType.Grimoire)
                {
                    totalGrimoires++;
                }
            }

            Debug.Log("total grimoires detected by spell learner " + totalGrimoires);
            if(totalGrimoires > 0)
            {
                spellSelectionHandler.SetMaxSelectable(maxPossibleCanLearn);
                spellSelectionHandler.GenerateContainersForGrimoiresInWizardVault(currentWarband.warbandVault, currentWarband.warbandWizard.playerWizardSpellbook);
                CreateBasicButtonAndAttach("Choose Grimoires to Learn", mainScrollContents, delegate {DoNothingEvent();});
            }
            else{
                CreateBasicButtonAndAttach("No Grimoires in Inventory", mainScrollContents, delegate {DoNothingEvent();});
            }
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
    public void ClearContent(GameObject window)
    {
        foreach(Transform item in window.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void DoNothingEvent()
    {
        //can't pass null delegates so this instead
    }

    

}
