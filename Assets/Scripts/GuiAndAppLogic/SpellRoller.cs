using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRoller
{
    
    private static int currentRoll;
    private static bool spellResult;

    public static int RollDice()
    {
        return Random.Range(1, 21);
    }

    public static bool MakeRollForSpell(WizardRuntimeSpell spell, int additionalMods = 0, int rollMods = 0)
    {
        currentRoll = RollDice();
        int targetMod = (spell.currentWizardLevelMod + spell.referenceSpell.CastingNumber);
        targetMod += additionalMods;
        currentRoll += rollMods;
        if( currentRoll >= targetMod)
        {
            spellResult = true;
        }
        else{
            spellResult = false;
        }
        return spellResult;
    }

    public static bool GetSpellResult()
    {
        return spellResult;
    }
    public static int GetCurrentRoll()
    {
        return currentRoll;
    }


}
