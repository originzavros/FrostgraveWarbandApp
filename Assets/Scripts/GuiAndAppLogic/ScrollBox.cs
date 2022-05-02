using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;


//This is the spell reference, not named great will refactor later
public class ScrollBox : MonoBehaviour
{
    [SerializeField] GameObject contentBox;
    [SerializeField] GameObject UIButtonPrefab;
    [SerializeField] GameObject spellButtonNormalPrefab;
    [SerializeField] GameObject spellButtonReversePrefab;
    [SerializeField] TMP_Dropdown spellDropDown;
    [SerializeField] TMP_Dropdown rangeTypeDropDown;

    [SerializeField] SpellTextPopup spellTextPopup;

   void Start()
   {
        PopulateWithAllSpells();
        PopulateSchoolDropdown();
        PopulateRangeTypeDropdown();
   }

    public void PopulateWithAllSpells()
    {
        foreach(var item in LoadAssets.spellObjects)
        {
            GameObject temp = Instantiate(spellButtonNormalPrefab);
            temp.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(temp);});
            SpellButton sb = temp.GetComponent<SpellButton>();
            sb.SpellNameText.text = item.Name;
            sb.CastingNumberText.text = item.CastingNumber.ToString();
            sb.referenceScriptable = item;
            // temp.transform.parent = contentBox.transform;
            temp.transform.SetParent(contentBox.transform, false);
        }
    }

    public void PopulateSchoolDropdown()
    {
        foreach(var item in LoadAssets.wizardSchoolObjects)
        {
            spellDropDown.options.Add(new TMP_Dropdown.OptionData(item.primarySchool.ToString()));
        }
    }
    public void PopulateRangeTypeDropdown()
    {
        foreach(var item in LoadAssets.spellRangeTypeForDisplay)
        {
            rangeTypeDropDown.options.Add(new TMP_Dropdown.OptionData(item));
        }
    }

    public void EnableAndFillDescriptionPopUp(GameObject go)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        SpellScriptable tempSpell = go.GetComponent<SpellButton>().referenceScriptable;
        spellTextPopup.UpdateInfo(tempSpell);
    }


    // [Button]
    // public void ShowOnlyButtonsOfSchool(string spellSchool)
    // {
    //     foreach(Transform item in contentBox.transform)
    //     {
    //         SpellScriptable tempSpell = item.GetComponent<SpellButton>().referenceScriptable;
    //         if(tempSpell.School != spellSchool)
    //         {
    //             item.gameObject.SetActive(false);
    //         }
    //         else{
    //             item.gameObject.SetActive(true);
    //         }
    //     }
    // }

    public void CheckRestrictions(string spellSchool = "null", string rangeType = "null", int castingNum =-1)
    {
        // Debug.Log(spellSchool);
        foreach(Transform item in contentBox.transform)
        {
            SpellScriptable tempSpell = item.GetComponent<SpellButton>().referenceScriptable;
            item.gameObject.SetActive(true);
            if(spellSchool != "null")
            {
                if(spellSchool != "Select School")
                {
                    if(tempSpell.School != (WizardSchools)System.Enum.Parse(typeof(WizardSchools), spellSchool)){ item.gameObject.SetActive(false);}
                }
                
            }
            if(rangeType != "null")
            {
                if(rangeType != "Range/Type")
                {
                    if(tempSpell.Restriction != rangeType){ item.gameObject.SetActive(false);}
                }
                
            }
            if(castingNum > -1)
            {
                if(tempSpell.CastingNumber != castingNum){ item.gameObject.SetActive(false);}
            }
        }
    }

    [Button]
    public void ResetButtonList()
    {
        spellDropDown.value = 0;
        rangeTypeDropDown.value = 0;

        foreach(Transform item in contentBox.transform)
        {
            item.gameObject.SetActive(true);
        }
        
    }

    public void OnSchoolDropdownChanged()
    {

        string temp = spellDropDown.options[spellDropDown.value].text;
        // Debug.Log("Dropdown text: " + temp);
        //string temp = spellDropDown.GetComponent<drop
        // ShowOnlyButtonsOfSchool(temp);
        CheckRestrictions(temp);
    }

    public void OnRangeTypeDropdownChanged()
    {
        string temp = rangeTypeDropDown.options[rangeTypeDropDown.value].text;
        CheckRestrictions(rangeType: temp);
    }

    public void OnEitherDropdownChanged()
    {
        string spellDropdownText = spellDropDown.options[spellDropDown.value].text;
        string rangeDropdownText = rangeTypeDropDown.options[rangeTypeDropDown.value].text;
        CheckRestrictions(spellDropdownText, rangeDropdownText);
    }

    


}
