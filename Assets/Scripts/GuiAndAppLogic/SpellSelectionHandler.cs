using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class SpellSelectionHandler : MonoBehaviour
{

    /*
        Purpose is to handle any menu where spells need to be selected with a checkbox
        meant to attach at the top level with whatever needs to use it, doesn't have a prefab
        setup with the scroll container it needs to populate
    */
    private int maxSelectable = 3;
    [SerializeField] GameObject scrollContainer;
    [SerializeField] GameObject CheckButtonContainerSpellPrefab;
    [SerializeField] GameObject CheckButtonContainerEquipmentPrefab;
    [SerializeField] SpellTextPopup spellTextPopup;

    [SerializeField] [ReadOnly] private int currentSelected = 0;

    public SelectionHandlerMode selectionHandlerMode = SelectionHandlerMode.normal;

    // Color32 midGreen = new Color32(58,169,17,255);
    // Color32 bloodRed = new Color32(154,12,12,255);
    // Color32 darkTeal = new Color32(12,138,154,255);
    // Color32 purple = new Color32(140,11,166,255);
    // Color32 darkGreen = new Color32(39,121,48,255);
    // Color32 darkOrange = new Color32(198,65,0,255);
    // Color32 darkBlue = new Color32(39,54,166,255);
    // Color32 darkMagenta = new Color32(191,21,76,255);
    // Color32 goldenYellow = new Color32(217,153,0,255);
    // Color32 veryDarkBlue = new Color32(32,41,70,255);

    // Color32 lightGrey = new Color32(215,215,215,255);

    public void SetMaxSelectable(int max)
    {
        maxSelectable = max;
    }
    public void ResetCurrentSelected()
    {
        currentSelected = 0;
        // EnableAllTogglesNormal();
    }
    public bool CheckIfAllSelected()
    {
        return currentSelected == maxSelectable;
    }

    public void OnToggleSelectSpell(bool toggleState)
    {
        if(toggleState)
        {
            currentSelected++;
        } 
        else{
            currentSelected--;
        }

        if(currentSelected == maxSelectable)
        {
            AtMaxSelectedNormal();
        }
        else{
            EnableAllTogglesNormal(); //inefficient but should have the right behaviour
        }
    }


    //this selects spells per school, set max selectable to 1 for single selection per school
    public void OnSelectSpellSingle(GameObject go, SpellSelectionType spellSelectType)
    {
        CheckButtonContainer cbc = go.GetComponent<CheckButtonContainer>();
        WizardSchools referenceSchool;
        // if(spellSelectType == SpellSelectionType.scriptable)
        // {
        //     referenceSchool = cbc.spellButton.referenceScriptable.School;
        // }
        // else{
        //     referenceSchool = cbc.spellButton.referenceRuntimeSpell.referenceSpell.School;
        // }

        referenceSchool = cbc.spellButton.referenceScriptable.School;

        if(cbc.selectedItem.isOn == true)
        {
            currentSelected++;
            DisableAllTogglesButSelectedSpellForSchool(referenceSchool);
        }
        else{
            currentSelected--;
            ResetAllContainersForSelectedSpellSchool(referenceSchool);
        }

        if(currentSelected == maxSelectable)
        {
            AtMaxSelectedNormal();
        }
        else{
            EnableAllTogglesNormal();
        }

    }

    public void DisableAllUntoggledToggles()
    {
        foreach(Transform item in scrollContainer.transform)
        {
            if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
            {
                if(cbc.selectedItem.isOn == false)
                {
                    cbc.selectedItem.interactable = false;
                }    
            } 
        }
    }

    public void EnableAllTogglesNormal()
    {
        Debug.Log("EnableAllTogglesNormal called");
        foreach(Transform item in scrollContainer.transform)
        {
            if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
            {
                cbc.selectedItem.interactable = true;
            }
            else{
                 Debug.Log("failed to get component in EnableAllTogglesNormal");
            }
        }
    }
    public void AtMaxSelectedNormal()
    {
        DisableAllUntoggledToggles();
    }

    public List<SpellScriptable> GetCurrentlySelectedSpells()
    {
        List<SpellScriptable> temp = new List<SpellScriptable>();
        foreach(Transform item in scrollContainer.transform)
        {
            if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
            {
                if(cbc.selectedItem.isOn)
                {
                    if(cbc.spellButton.referenceScriptable != null)
                    {
                        temp.Add(cbc.spellButton.referenceScriptable);
                    }
                    else{
                        temp.Add(cbc.spellButton.referenceRuntimeSpell.referenceSpell);
                    }
                }
            }
        }
        return temp;
    }

    public List<EquipmentScriptable> GetCurrentlySelectedEquipment()
    {
        List<EquipmentScriptable> temp = new List<EquipmentScriptable>();
        foreach(Transform item in scrollContainer.transform)
        {
            if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
            {
                if(cbc.selectedItem.isOn)
                {
                    temp.Add(cbc.equipmentButton.GetEquipment());
                }
            } 
        }
        return temp;
    }

    // public Color32 GetColorBasedOnWizardSchool(WizardSchools _wizardSchool)
    // {
    //     if( _wizardSchool == WizardSchools.Chronomancer)
    //     {
    //         return midGreen;
    //     }
    //     else if( _wizardSchool == WizardSchools.Elementalist)
    //     {
    //         return bloodRed;
    //     }
    //     else if( _wizardSchool == WizardSchools.Enchanter)
    //     {
    //         return darkTeal;
    //     }
    //     else if( _wizardSchool == WizardSchools.Illusionist)
    //     {
    //         return purple;
    //     }
    //     else if( _wizardSchool == WizardSchools.Necromancer)
    //     {
    //         return darkGreen;
    //     }
    //     else if( _wizardSchool == WizardSchools.Sigilist)
    //     {
    //         return darkOrange;
    //     }
    //     else if( _wizardSchool == WizardSchools.Soothsayer)
    //     {
    //         return darkBlue;
    //     }
    //     else if( _wizardSchool == WizardSchools.Summoner)
    //     {
    //         return darkMagenta;
    //     }
    //     else if( _wizardSchool == WizardSchools.Thaumaturge)
    //     {
    //         return goldenYellow;
    //     }
    //     else if( _wizardSchool == WizardSchools.Witch)
    //     {
    //         return veryDarkBlue;
    //     }
    //     else{
    //         return lightGrey;
    //         // cbctemp.spellButton.GetComponent<Button>().image.color = Color.white;
    //         // cbctemp.spellButton.SpellNameText.color = Color.black;
    //         // cbctemp.spellButton.CastingNumberText.color = Color.black;
    //     }
    // }

    public void GenerateContainersForSpellsFromSchool(WizardSchools _wizardSchool)
    {
        
        foreach(var item in LoadAssets.spellObjects)
        {
            // Debug.Log("Wizard School: " + item.primarySchool.ToString());


            if(item.School == _wizardSchool)
            {
                GameObject temp = Instantiate(CheckButtonContainerSpellPrefab);
                CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();
                cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject, SpellSelectionType.scriptable);});
                cbctemp.spellButton.LoadSpellInfo(item);

                
                // Color32 incomingColor = GetColorBasedOnWizardSchool(item.School);
                // if(incomingColor.CompareRGB(lightGrey))
                // {
                //     cbctemp.spellButton.SpellNameText.color = Color.black;
                //     cbctemp.spellButton.CastingNumberText.color = Color.black;
                //     cbctemp.spellButton.GetComponent<Button>().image.color = incomingColor;
                // }
                // else{
                //     cbctemp.spellButton.SpellNameText.color = Color.white;
                //     cbctemp.spellButton.CastingNumberText.color = Color.white;
                //     cbctemp.spellButton.GetComponent<Button>().image.color = incomingColor;
                // }

                temp.transform.SetParent(scrollContainer.transform, false);
                // temp.transform.parent = scrollContainer.transform;
            }
        }
    }

    //this won't add spells that have been maxed out (their casting number can't be lower than 5)
    public void GenerateContainersForSpellsFromWizardSpellbook(WizardSpellbook spellbook)
    {
        foreach(var spellitem in spellbook.wizardSpellbookSpells)
        {
            if(spellitem.referenceSpell.CastingNumber + spellitem.currentWizardLevelMod > 5)
            {
                GameObject temp = Instantiate(CheckButtonContainerSpellPrefab);
                CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();

                // if(selectionHandlerMode == SelectionHandlerMode.single)
                // {
                //     cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnSelectSpellSingle(temp, SpellSelectionType.runtime);});
                // }
                // else{
                //     cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                // }
                cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject, SpellSelectionType.runtime);});
                cbctemp.spellButton.LoadRuntimeSpellInfo(spellitem);
                // cbctemp.spellButton.LoadSpellInfo(item);
                temp.transform.SetParent(scrollContainer.transform, false);
            }   
        }
    }

    // will not include spells the wizard already knows
    public void GenerateContainersForGrimoiresInWizardVault(List<MagicItemScriptable> vault, WizardSpellbook spellbook)
    {
        Debug.Log("Generating spell containers for grimoires in spellSelectionHandler");
        List<SpellScriptable> spells = new List<SpellScriptable>();
        foreach(var item in vault)
        {
            if(item.itemType == MagicItemType.Grimoire)
            {
                Debug.Log("grimoire name in vault :" + item.itemName);
                SpellScriptable refSpell = DetermineSpellFromGrimoire(item);
                if(refSpell != null)
                {
                    Debug.Log("Determined spell from grimoire: " + refSpell.Name);
                    if(!CheckIfSpellIsInSpellbook(refSpell, spellbook))
                    {
                        spells.Add(refSpell);
                    }
                    else{
                        Debug.Log("Spell is already in spellbook");
                    }
                }
            }
        }
        foreach(var spell in spells)
        {
            GameObject temp = Instantiate(CheckButtonContainerSpellPrefab);
            CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();
            cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
            cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject, SpellSelectionType.scriptable);});
            // cbctemp.spellButton.LoadRuntimeSpellInfo(spellitem);
            cbctemp.spellButton.LoadSpellInfo(spell);
            temp.transform.SetParent(scrollContainer.transform, false);
        }
    }

    private SpellScriptable DetermineSpellFromGrimoire(MagicItemScriptable grimoire)
    {
        string grimSpellName = grimoire.itemName;
        // char[] removeChar = {'G','r','i','m','o','i','r','e',' '};
        // grimSpellName = grimSpellName.TrimEnd(removeChar);
        grimSpellName = grimSpellName.Substring(0, grimSpellName.Length - 9);

        Debug.Log("trimmed spell name: " + grimSpellName);

        foreach(var spell in LoadAssets.spellObjects)
        {
            if(spell.Name == grimSpellName)
            {
                return spell;
            }
        }
        return null;
    }
    private bool CheckIfSpellIsInSpellbook(SpellScriptable spellScriptable, WizardSpellbook spellbook)
    {
        foreach(var spell in spellbook.wizardSpellbookSpells)
        {
            if(spell.referenceSpell == spellScriptable)
            {
                return true;
            }
        }
        return false;
    }

    //wizards can't equip armor or shields so we need to filter
    public void GenerateContainersForStandardWizardEquipment()
    {
        foreach(var item in LoadAssets.allEquipmentObjects)
        {
            if(CheckIfNotEquipableByWizard(item.equipmentName))
            {
                GameObject temp = Instantiate(CheckButtonContainerEquipmentPrefab);
                CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();
                cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                cbctemp.equipmentButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.equipmentButton.gameObject, SpellSelectionType.scriptable);});
                cbctemp.equipmentButton.SetEquipment(item);
                temp.transform.SetParent(scrollContainer.transform, false);
            }
        }
    }

    private bool CheckIfNotEquipableByWizard(string item)
    {
        bool temp = true;
        if(item == "Heavy Armor" || item == "Light Armor" || item == "Shield")
        {
            temp = false;
        }
        return temp;
    }

    public void GenerateContainersForSpellsFromMultipleSchools(List<WizardSchools> _wizardSchools)
    {

        foreach(var schoolItem in _wizardSchools)
        {
            foreach(var item in LoadAssets.spellObjects) //unfortunate to loop it this way but should be about the same cost as getting all of them and sorting
            {

                // Debug.Log("Wizard School: " + item.primarySchool.ToString());
                // bool matchesASchool = false;

                // foreach(WizardSchools schoolItem in _wizardSchools)
                // {
                //     if(item.School == schoolItem)
                //     {
                //         matchesASchool = true;
                //         break;
                //     }
                // }

                if(item.School == schoolItem)
                {
                    GameObject temp = Instantiate(CheckButtonContainerSpellPrefab);
                    CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();
                    if(selectionHandlerMode == SelectionHandlerMode.single)
                    {
                        cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnSelectSpellSingle(temp,SpellSelectionType.scriptable);});
                    }
                    else{
                        cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                    }
                    // temp.GetComponent<Button>().onClick.AddListener(delegate {OnClickWizardSchoolButton(temp);});
                    cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject, SpellSelectionType.scriptable);});
                    cbctemp.spellButton.LoadSpellInfo(item);

                    // Color32 incomingColor = GetColorBasedOnWizardSchool(item.School);
                    // if(incomingColor.CompareRGB(lightGrey))
                    // {
                    //     cbctemp.spellButton.SpellNameText.color = Color.black;
                    //     cbctemp.spellButton.CastingNumberText.color = Color.black;
                    //     cbctemp.spellButton.GetComponent<Button>().image.color = incomingColor;
                    // }
                    // else{
                    //     cbctemp.spellButton.SpellNameText.color = Color.white;
                    //     cbctemp.spellButton.CastingNumberText.color = Color.white;
                    //     cbctemp.spellButton.GetComponent<Button>().image.color = incomingColor;
                    // }

                    temp.transform.SetParent(scrollContainer.transform, false);
                    // temp.transform.parent = scrollContainer.transform;
                }
            }
        }
        
    }


    public void ClearSpellContainers()
    {
         foreach(Transform item in scrollContainer.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void EnableAndFillDescriptionPopUp(GameObject go, SpellSelectionType spellSelectType)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        if(spellSelectType == SpellSelectionType.scriptable)
        {
            SpellScriptable tempSpell = go.GetComponent<SpellButton>().referenceScriptable;
            spellTextPopup.UpdateInfo(tempSpell);
        }
        else{
            WizardRuntimeSpell tempSpell = go.GetComponent<SpellButton>().referenceRuntimeSpell;
            spellTextPopup.UpdateRuntimeInfo(tempSpell);
        }
        
    }


    //for spell selection for aligned schools, they must select 1 from each school
    public void DisableAllTogglesButSelectedSpellForSchool(WizardSchools _wizardSchool)
    {
        foreach(Transform item in scrollContainer.transform)
            {
                if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
                {
                    if(cbc.spellButton.referenceScriptable.School == _wizardSchool)
                    {
                        if(cbc.selectedItem.isOn == false)
                        {
                            cbc.gameObject.SetActive(false);
                            // cbc.selectedItem.interactable = false;
                        }     
                    }
                }
            }
    }

    public void ResetAllContainersForSelectedSpellSchool(WizardSchools _wizardSchool)
    {
        foreach(Transform item in scrollContainer.transform)
        {
            if(item.TryGetComponent<CheckButtonContainer>(out CheckButtonContainer cbc))
            {
                if(cbc.spellButton.referenceScriptable.School == _wizardSchool)
                {
                    cbc.gameObject.SetActive(true);
                    // if(cbc.selectedItem.isOn == false)
                    // {
                    //     cbc.gameObject.SetActive(false);
                    //     // cbc.selectedItem.interactable = false;
                    // }     
                }
            }  
        }
    }
    
}

public enum SelectionHandlerMode{
    normal,
    single
}

public enum SpellSelectionType{
    scriptable,
    runtime
}
