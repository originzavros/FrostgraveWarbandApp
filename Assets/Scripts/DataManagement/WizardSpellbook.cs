using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpellbook 
{
    /*
        Purpose is to hold all spells for a wizard. These spells can be modified, unlike spells from the spell reference.
        When the book is saved, write just the spell name and it's modified casting number to file,
        On load, we load in the spells in the app by spell name, then overried their casting num with the player one.
    */

    // public List<SpellScriptable> wizardSpellbookSpells;
    public List<WizardRuntimeSpell> wizardSpellbookSpells;

    public WizardSchoolScriptable wizardSchool;

    public WizardSpellbook()
    { 
        wizardSpellbookSpells =  new List<WizardRuntimeSpell>();
    }
    public void Init(WizardSchoolScriptable selectedSchool)
    {
        wizardSchool = selectedSchool;  
    }
}

public class WizardRuntimeSpell
{
    public SpellScriptable referenceSpell;
    public int currentWizardLevelMod = 0;
    public int wizardSchoolMod = 0;

    public void Init(SpellScriptable reference)
    {
        referenceSpell = reference;
        currentWizardLevelMod = 0;
        wizardSchoolMod = 0;
    }

    public int GetAllMods()
    {
        return currentWizardLevelMod + wizardSchoolMod;
    }

    public int GetFullModedCastingNumber()
    {
        int temp = GetAllMods();
        return (referenceSpell.CastingNumber + temp);
    }
}
