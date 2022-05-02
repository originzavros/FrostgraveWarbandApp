using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SpellSelectionHandler : MonoBehaviour
{

    /*
        Purpose is to handle any menu where spells need to be selected with a checkbox

    */
    private int maxSelectable = 3;
    [SerializeField] GameObject scrollContainer;
    [SerializeField] GameObject CheckButtonContainerSpellPrefab;
    [SerializeField] GameObject CheckButtonContainerEquipmentPrefab;
    [SerializeField] SpellTextPopup spellTextPopup;

    [SerializeField] [ReadOnly] private int currentSelected = 0;

    public SelectionHandlerMode selectionHandlerMode = SelectionHandlerMode.normal;

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

    public void OnSelectSpellSingle(GameObject go)
    {
        CheckButtonContainer cbc = go.GetComponent<CheckButtonContainer>();
        if(cbc.selectedItem.isOn == true)
        {
            currentSelected++;
            DisableAllTogglesButSelectedSpellForSchool(cbc.spellButton.referenceScriptable.School);
        }
        else{
            currentSelected--;
            ResetAllContainersForSelectedSpellSchool(cbc.spellButton.referenceScriptable.School);
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
            CheckButtonContainer cbc = item.GetComponent<CheckButtonContainer>();
            if(cbc.selectedItem.isOn == false)
            {
                cbc.selectedItem.interactable = false;
            }       
        }
    }

    public void EnableAllTogglesNormal()
    {
        Debug.Log("EnableAllTogglesNormal called");
        foreach(Transform item in scrollContainer.transform)
        {
            try{
                item.gameObject.GetComponent<CheckButtonContainer>().selectedItem.interactable = true;
            }
            catch{
                Debug.Log("failed to get component i think");
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
            CheckButtonContainer cbc = item.GetComponent<CheckButtonContainer>();
            if(cbc.selectedItem.isOn)
            {
                temp.Add(cbc.spellButton.referenceScriptable);
            }
        }
        return temp;
    }

    public List<EquipmentScriptable> GetCurrentlySelectedEquipment()
    {
        List<EquipmentScriptable> temp = new List<EquipmentScriptable>();
        foreach(Transform item in scrollContainer.transform)
        {
            CheckButtonContainer cbc = item.GetComponent<CheckButtonContainer>();
            if(cbc.selectedItem.isOn)
            {
                temp.Add(cbc.equipmentButton.GetEquipment());
            }
        }
        return temp;
    }

   

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
                cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject);});
                cbctemp.spellButton.LoadSpellInfo(item);
                temp.transform.SetParent(scrollContainer.transform, false);
                // temp.transform.parent = scrollContainer.transform;
            }
        }
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
                cbctemp.equipmentButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.equipmentButton.gameObject);});
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
        foreach(var item in LoadAssets.spellObjects)
        {
            // Debug.Log("Wizard School: " + item.primarySchool.ToString());
            bool matchesASchool = false;

            foreach(WizardSchools schoolItem in _wizardSchools)
            {
                if(item.School == schoolItem)
                {
                    matchesASchool = true;
                    break;
                }
            }

            if(matchesASchool)
            {
                GameObject temp = Instantiate(CheckButtonContainerSpellPrefab);
                CheckButtonContainer cbctemp = temp.GetComponent<CheckButtonContainer>();
                if(selectionHandlerMode == SelectionHandlerMode.single)
                {
                    cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnSelectSpellSingle(temp);});
                }
                else{
                    cbctemp.selectedItem.onValueChanged.AddListener(delegate {OnToggleSelectSpell(cbctemp.selectedItem.isOn);});
                }
                // temp.GetComponent<Button>().onClick.AddListener(delegate {OnClickWizardSchoolButton(temp);});
                cbctemp.spellButton.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(cbctemp.spellButton.gameObject);});
                cbctemp.spellButton.LoadSpellInfo(item);
                temp.transform.SetParent(scrollContainer.transform, false);
                // temp.transform.parent = scrollContainer.transform;
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

    public void EnableAndFillDescriptionPopUp(GameObject go)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        SpellScriptable tempSpell = go.GetComponent<SpellButton>().referenceScriptable;
        spellTextPopup.UpdateInfo(tempSpell);
    }


    //for spell selection for aligned schools, they must select 1 from each school
    public void DisableAllTogglesButSelectedSpellForSchool(WizardSchools _wizardSchool)
    {
        foreach(Transform item in scrollContainer.transform)
            {
                CheckButtonContainer cbc = item.GetComponent<CheckButtonContainer>();
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

    public void ResetAllContainersForSelectedSpellSchool(WizardSchools _wizardSchool)
    {
        foreach(Transform item in scrollContainer.transform)
            {
                CheckButtonContainer cbc = item.GetComponent<CheckButtonContainer>();
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

public enum SelectionHandlerMode{
    normal,
    single
}
