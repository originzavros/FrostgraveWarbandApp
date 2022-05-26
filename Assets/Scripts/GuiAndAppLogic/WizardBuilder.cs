using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System.Linq;

public class WizardBuilder : MonoBehaviour
{

    [SerializeField] GameObject wizardSchoolButton;
    [SerializeField] GameObject spellButton;
    [SerializeField] GameObject scrollContainer;
    [SerializeField] GameObject CheckButtonContainerPrefab;
    [SerializeField] GameObject EquipmentButtonPrefab;
    [SerializeField] GameObject NameGeneratorInputBox;
    [SerializeField] GameObject WarbandInput;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] SpellSelectionHandler spellSelectionHandler;
    [SerializeField] GameObject nextButtonNav;
    [SerializeField] NavBox navBox;
    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] WizardSchoolScriptable randomSchoolPrefab;
    [SerializeField] GameObject infoPopup;

    [SerializeField][ReadOnly]
    int currentStep = 0;

    [SerializeField] SoldierScriptable wizardProfileScriptable;

    private PlayerWizard playerWizard;
    private PlayerWarband playerWarband;
    private WizardSchoolScriptable selectedSchool;
    private List<SpellScriptable> selectedPrimarySpells;
    private List<SpellScriptable> selectedAlignedSpells;
    private List<SpellScriptable> selectedNeutralSpells;




    // void Start()
    // {
    //     Init();
    // }

    public void Init()
    {
        ClearContent();
        currentStep = 0;
        playerWizard = new PlayerWizard();
        playerWizard.Init();
        RuntimeSoldierData newWizardProfile = new RuntimeSoldierData();
        newWizardProfile.Init(wizardProfileScriptable);
        playerWizard.playerWizardProfile = newWizardProfile;
        HandleCurrentStepSetup();
    }

    public void PopulateBuilderWithSchools()
    {
        foreach(var item in LoadAssets.wizardSchoolObjects)
        {
            CreateAndAttachSchoolButton(item);
        }
        CreateAndAttachSchoolButton(randomSchoolPrefab);
    }

    public void CreateAndAttachSchoolButton(WizardSchoolScriptable _schoolScriptable)
    {
        GameObject temp = Instantiate(wizardSchoolButton);
        temp.GetComponent<Button>().onClick.AddListener(delegate {OnClickWizardSchoolButton(temp);});
        WizardSchoolButton wb = temp.GetComponent<WizardSchoolButton>();
        wb.SetWizardSchool(_schoolScriptable.primarySchool);
        wb.schoolScriptableReference = _schoolScriptable;
        // temp.transform.parent = scrollContainer.transform;
        temp.transform.SetParent(scrollContainer.transform, false);
    }

    public void OnClickWizardSchoolButton(GameObject go)
    {
        selectedSchool = go.GetComponent<WizardSchoolButton>().schoolScriptableReference;
        if(selectedSchool.primarySchool == WizardSchools.Random)
        {
            CreateRandomWizard();
        }
        else{
            BuilderToNextStep();
        } 
    }

    public void CreateRandomWizard()
    {
        //do all selection and naming
        //popup event that it was completed
        // Debug.Log(LoadAssets.wizardSchoolObjects[0].primarySchool.ToString());
        int selection = Random.Range(0, LoadAssets.wizardSchoolObjects.Length);
        // LoadAssets.wizardSchoolObjects.OrderBy(x => Random.Range(0f,1f));
        
        selectedSchool = LoadAssets.wizardSchoolObjects[selection];
        selectedPrimarySpells = new List<SpellScriptable>();
        selectedPrimarySpells.AddRange(GetRandomSpellsFromSchool(3, selectedSchool.primarySchool));
        selectedAlignedSpells = new List<SpellScriptable>();
        foreach(var item in selectedSchool.alignedSchools)
        {
            selectedAlignedSpells.AddRange(GetRandomSpellsFromSchool(1, item));
        }
        IEnumerable<int> chosenNeutralSchools = Enumerable.Range(0,selectedSchool.neutralSchools.Count).OrderBy(x => Random.Range(0f, 1f)).Take(2);
        int[] neutralChoices = chosenNeutralSchools.ToArray();
        Debug.Log("neutralChoices :" + neutralChoices[0].ToString() + " and " + neutralChoices[1].ToString());
        selectedNeutralSpells = new List<SpellScriptable>();
        selectedNeutralSpells.AddRange(GetRandomSpellsFromSchool(1, selectedSchool.neutralSchools[neutralChoices[0]]));
        selectedNeutralSpells.AddRange(GetRandomSpellsFromSchool(1, selectedSchool.neutralSchools[neutralChoices[1]]));

        
        // NameGeneratorInputBox.GetComponent<NameGenerator>().OnClickRandomOldeMaleName();
        NameGeneratorInputBox.GetComponent<NameGenerator>().OnClickRandomMaleName();
        playerWizard.playerWizardProfile.soldierName = NameGeneratorInputBox.GetComponent<NameGenerator>().GetName();
        WarbandInput.GetComponent<BasicInput>().nameEntry.text = playerWizard.playerWizardProfile.soldierName + "'s Warband";
        FinishWarband();
        infoPopup.SetActive(true);
        string info = "Completed Random Wizard: " + playerWizard.playerWizardProfile.soldierName;
        infoPopup.GetComponent<BasicPopup>().UpdatePopupText(info);
    }

    public List<SpellScriptable> GetRandomSpellsFromSchool(int amount, WizardSchools _school)
    {
        List<SpellScriptable> temp = new List<SpellScriptable>();
        foreach(var spell in LoadAssets.spellObjects)
        {
            if(spell.School == _school)
            {
                temp.Add(spell);
            }
        }
        if(temp.Count <= amount)
        {
            Debug.Log("got less than count in randomspellfromschool");
            return temp;
        }
        else{
            int target = temp.Count;
            while(target > amount)
            {
                int remove = Random.Range(0, target);
                temp.RemoveAt(remove);
                target--;
            }
            return temp;
        }
    }

    public void ClearContent()
    {
        foreach(Transform item in scrollContainer.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void NextStep()
    {
        if(currentStep == 1)
        {
            if(spellSelectionHandler.CheckIfAllSelected())
            {
                Debug.Log("finishing first step");
                selectedPrimarySpells = spellSelectionHandler.GetCurrentlySelectedSpells();
                BuilderToNextStep();
            }
            else{
                Debug.Log("didn't select enough");
            }
            
        }
        else if(currentStep == 2)
        {
            if(spellSelectionHandler.CheckIfAllSelected())
            {
                Debug.Log("finishing second step");
                selectedAlignedSpells = spellSelectionHandler.GetCurrentlySelectedSpells();
                BuilderToNextStep();
            }
            else{
                Debug.Log("didn't select enough");
            }
        }
        else if(currentStep == 3)
        {
            if(spellSelectionHandler.CheckIfAllSelected())
            {
                Debug.Log("finishing third step");
                selectedNeutralSpells = spellSelectionHandler.GetCurrentlySelectedSpells();
                BuilderToNextStep();
            }
            else{
                Debug.Log("didn't select enough");
            }
        }
        else if(currentStep == 4)
        {
            Debug.Log("finishing fourth step");
            playerWizard.playerWizardProfile.soldierName = NameGeneratorInputBox.GetComponent<NameGenerator>().GetName();
            Debug.Log("wizard name: " + playerWizard.playerWizardProfile.soldierName);
            BuilderToNextStep();
        }
        else if(currentStep == 5)
        {
            Debug.Log("finishing fifth step");
            playerWizard.playerWizardProfile.baseSoldierEquipment = spellSelectionHandler.GetCurrentlySelectedEquipment();
            BuilderToNextStep();
        }
        else{
            Debug.Log("no code for next step");
        }
    }

    public void BuilderToNextStep()
    {
        ClearContent();
        currentStep++;
        HandleCurrentStepSetup();
    }

    public void PopulateBuilderWithPrimarySpells()
    {
        spellSelectionHandler.selectionHandlerMode = SelectionHandlerMode.normal;
        spellSelectionHandler.ResetCurrentSelected();
        spellSelectionHandler.SetMaxSelectable(3);
        spellSelectionHandler.GenerateContainersForSpellsFromSchool(selectedSchool.primarySchool); 
    }

    public void PopulateBuilderWithAlignedSpells()
    {
        spellSelectionHandler.selectionHandlerMode = SelectionHandlerMode.single;
        spellSelectionHandler.ResetCurrentSelected();
        spellSelectionHandler.SetMaxSelectable(3);
        spellSelectionHandler.GenerateContainersForSpellsFromMultipleSchools(selectedSchool.alignedSchools);
    }

    public void PopulateBuilderWithNeutralSpells()
    {
        spellSelectionHandler.selectionHandlerMode = SelectionHandlerMode.single;
        spellSelectionHandler.ResetCurrentSelected();
        spellSelectionHandler.SetMaxSelectable(2);
        spellSelectionHandler.GenerateContainersForSpellsFromMultipleSchools(selectedSchool.neutralSchools);
    }

    public void PopulateBuilderWithStandardEquipment()
    {
        spellSelectionHandler.selectionHandlerMode = SelectionHandlerMode.normal;
        spellSelectionHandler.ResetCurrentSelected();
        spellSelectionHandler.SetMaxSelectable(3);
        spellSelectionHandler.GenerateContainersForStandardWizardEquipment();
    }

    public void BackButton()
    {
        ClearContent();
        if(currentStep > 0)
        {
            currentStep--;
            HandleCurrentStepSetup();
            }
        else{ 
            navBox.OnClickNavHome();
        }
        
    }

    public void FinishWarband()
    {
        //do stuff to save it
        playerWizard.playerWizardSpellbook = new WizardSpellbook();
        playerWizard.playerWizardSpellbook.Init(selectedSchool);
        List<SpellScriptable> tempspells = new List<SpellScriptable>();
        tempspells.AddRange(selectedPrimarySpells);
        tempspells.AddRange(selectedAlignedSpells);
        tempspells.AddRange(selectedNeutralSpells);
        foreach(var item in tempspells)
        {
            WizardRuntimeSpell newTempSpell = new WizardRuntimeSpell();
            newTempSpell.Init(item);
            newTempSpell.wizardSchoolMod = CheckSpellAlignmentMod(newTempSpell, playerWizard.playerWizardSpellbook.wizardSchool);
            playerWizard.playerWizardSpellbook.wizardSpellbookSpells.Add(newTempSpell);
        }
        // playerWizard.playerWizardSpellbook.wizardSpellbookSpells.AddRange(selectedPrimarySpells);
        // playerWizard.playerWizardSpellbook.wizardSpellbookSpells.AddRange(selectedAlignedSpells);
        // playerWizard.playerWizardSpellbook.wizardSpellbookSpells.AddRange(selectedNeutralSpells);
        playerWarband = new PlayerWarband();
        playerWarband.Init();
        playerWarband.warbandName = WarbandInput.GetComponent<BasicInput>().nameEntry.text;
        playerWarband.warbandWizard = playerWizard;

        warbandInfoManager.Init(playerWarband);
        warbandInfoManager.SaveCurrentWarband();

        navBox.OnClickNavHome();
    }

    public int CheckSpellAlignmentMod(WizardRuntimeSpell _spell, WizardSchoolScriptable _school)
    {
        int schoolMod = 0;
        bool foundSchool = false;
        if(_spell.referenceSpell.School == _school.primarySchool)
        {
            schoolMod = 0;
        }
        else{
            if(!foundSchool)
            {
                foreach(var item in _school.alignedSchools)
                {
                    if(_spell.referenceSpell.School == item)
                    {
                        schoolMod = 2;
                        foundSchool = true;
                        break;
                    }
                }
            }
            if(!foundSchool)
            {
                foreach(var item in _school.neutralSchools)
                {
                    if(_spell.referenceSpell.School == item)
                    {
                        schoolMod = 4;
                        foundSchool = true;
                        break;
                    }
                }
            }
            if(!foundSchool)
            {
                foreach(var item in _school.enemySchools)
                {
                    if(_spell.referenceSpell.School == item)
                    {
                        schoolMod = 6;
                        foundSchool = true;
                        break;
                    }
                }
            } 
        }
        return schoolMod;
    }

    public void HandleCurrentStepSetup()
    {
        // ClearContent();
        if(currentStep == 0)
        {
            PopulateBuilderWithSchools();
            titleText.text = "Choose a Wizard School";
            nextButtonNav.SetActive(false);
        }
        if(currentStep == 1)
        {
            PopulateBuilderWithPrimarySpells();
            // titleText.text = "Choose 3 Spells from " + selectedSchool.primarySchool.ToString()+ " School";
            titleText.text = "Choose 3 Spells from Wizard School";
            nextButtonNav.SetActive(true);
        }
        if(currentStep == 2)
        {
            PopulateBuilderWithAlignedSpells();
            titleText.text = "Choose one Spell from each Aligned School";
        }
        if(currentStep == 3)
        {
            PopulateBuilderWithNeutralSpells();
            titleText.text = "Choose two Spells from different Neutral Schools";
            NameGeneratorInputBox.SetActive(false);
        }
        if(currentStep == 4)
        {
            NameGeneratorInputBox.SetActive(true);
            titleText.text = "Name your Wizard";
        }
        if(currentStep == 5)
        {
            PopulateBuilderWithStandardEquipment();
            NameGeneratorInputBox.SetActive(false);
            titleText.text = "Select 3 Standard Equipment";
            nextButtonNav.SetActive(true);
        }
        if(currentStep == 6)
        {
            WarbandInput.SetActive(true);
            WarbandInput.GetComponent<BasicInput>().nameEntry.text = playerWizard.playerWizardProfile.soldierName + "'s Warband";
            titleText.text = "Name Warband";
            nextButtonNav.SetActive(false);
        }
    }
}
