using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRoller
{
    
    private static int currentRoll;
    private static bool spellResult;

    public static int RollDice()
    {
        return Random.Range(1, 20);
    }

    public static bool MakeRollForSpell(WizardRuntimeSpell spell, int additionalMods = 0)
    {
        currentRoll = RollDice();
        int totalMods = (spell.currentWizardLevelMod + spell.referenceSpell.CastingNumber);
        totalMods += additionalMods;
        if( totalMods >= currentRoll)
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
