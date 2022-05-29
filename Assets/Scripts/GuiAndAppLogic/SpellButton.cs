using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellButton : MonoBehaviour
{
    public TextMeshProUGUI SpellNameText;
    public TextMeshProUGUI CastingNumberText;

    public SpellScriptable referenceScriptable;
    public WizardRuntimeSpell referenceRuntimeSpell;

    public void LoadSpellInfo(SpellScriptable _spell)
    {
        referenceScriptable = _spell;
        SpellNameText.text = referenceScriptable.Name;
        CastingNumberText.text = referenceScriptable.CastingNumber.ToString();
        SetColorBasedOnSpellSchool();
    }

    public void LoadRuntimeSpellInfo(WizardRuntimeSpell _spell)
    {
        referenceRuntimeSpell = _spell;
        SpellNameText.text = referenceRuntimeSpell.referenceSpell.Name;
        CastingNumberText.text = (referenceRuntimeSpell.currentWizardLevelMod + referenceRuntimeSpell.wizardSchoolMod + referenceRuntimeSpell.referenceSpell.CastingNumber).ToString();
        SetColorBasedOnSpellSchool();
    }

    //should probably put these in their own spot for easy access and so they are not instanced each time
    Color32 midGreen = new Color32(58,169,17,255);
    Color32 bloodRed = new Color32(154,12,12,255);
    Color32 darkTeal = new Color32(12,138,154,255);
    Color32 purple = new Color32(140,11,166,255);
    Color32 darkGreen = new Color32(39,121,48,255);
    Color32 darkOrange = new Color32(198,65,0,255);
    Color32 darkBlue = new Color32(39,54,166,255);
    Color32 darkMagenta = new Color32(191,21,76,255);
    Color32 goldenYellow = new Color32(217,153,0,255);
    Color32 veryDarkBlue = new Color32(32,41,70,255);
    Color32 lightGrey = new Color32(215,215,215,255);

    public void SetColorBasedOnSpellSchool()
    {
        WizardSchools temp;
        if(referenceScriptable != null)
        {
            temp = referenceScriptable.School;
        }
        else{
            temp = referenceRuntimeSpell.referenceSpell.School;
        }


        SpellNameText.color = Color.white;
        CastingNumberText.color = Color.white;
        if( temp == WizardSchools.Chronomancer)
        {
            this.gameObject.GetComponent<Button>().image.color = midGreen;
        }
        else if( temp == WizardSchools.Elementalist)
        {
            this.gameObject.GetComponent<Button>().image.color = bloodRed;
        }
        else if( temp == WizardSchools.Enchanter)
        {
            this.gameObject.GetComponent<Button>().image.color = darkTeal;
        }
        else if( temp == WizardSchools.Illusionist)
        {
            this.gameObject.GetComponent<Button>().image.color = purple;
        }
        else if( temp == WizardSchools.Necromancer)
        {
            this.gameObject.GetComponent<Button>().image.color = darkGreen;
        }
        else if( temp == WizardSchools.Sigilist)
        {
            this.gameObject.GetComponent<Button>().image.color = darkOrange;
        }
        else if( temp == WizardSchools.Soothsayer)
        {
            this.gameObject.GetComponent<Button>().image.color = darkBlue;
        }
        else if( temp == WizardSchools.Summoner)
        {
            this.gameObject.GetComponent<Button>().image.color = darkMagenta;
        }
        else if( temp == WizardSchools.Thaumaturge)
        {
            this.gameObject.GetComponent<Button>().image.color = goldenYellow;
        }
        else if( temp == WizardSchools.Witch)
        {
            this.gameObject.GetComponent<Button>().image.color = veryDarkBlue;
        }
        else{
            this.gameObject.GetComponent<Button>().image.color = lightGrey;
            SpellNameText.color = Color.black;
            CastingNumberText.color = Color.black;
        }
    }
    
}
