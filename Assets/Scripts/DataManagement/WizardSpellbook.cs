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

//saving by reference is causing problems, so had to awkwardly patch this
public class WizardRuntimeSpell
{
    public SaveSpellRuntime referenceSpell; 
    public int currentWizardLevelMod = 0;
    public int wizardSchoolMod = 0;

    public void Init(SpellScriptable reference)
    {
        referenceSpell = new SaveSpellRuntime();
        referenceSpell.Init(reference);
        // referenceSpell = reference;
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

public class SaveSpellRuntime
{
    public string Name;
    public int CastingNumber;
    public WizardSchools School;
    public string Restriction;
    public string Description;
    public FrostgraveBook bookEdition;

    public void Init(SpellScriptable spell)
    {
        Name = spell.Name;
        CastingNumber = spell.CastingNumber;
        School = spell.School;
        Restriction = spell.Restriction;
        Description = spell.Description;
        bookEdition = spell.bookEdition;
    }

    public SpellScriptable GetReferenceSpell()
    {
        foreach(var item in LoadAssets.spellObjects)
        {
            if(item.Name == this.Name)
            {
                return item;
            }
        }
        Debug.Log("Failed to get reference spell in SaveSpellRuntime");
        return null;
    }

    
}

