using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameInfo
{
    int creaturesKilled = 0;
    int spellsPassed = 0;
    int spellsFailed = 0;
    int treasuresCaptured = 0;

    public void KillCreature()
    {
        creaturesKilled++;
    }
    public void PassSpell()
    {
        spellsPassed++;
    }
    public void FailSpell()
    {
        spellsFailed++;
    }
    public void CaptureTreasure()
    {
        treasuresCaptured++;
    }

    public int GetCreaturesKilled()
    {
        return creaturesKilled;
    }
    public int GetSpellsPassed()
    {
        return spellsPassed;
    }
    public int GetSpellsFailed()
    {
        return spellsFailed;
    }
    public int GetTreasuresCaptured()
    {
        return treasuresCaptured;
    }
}
