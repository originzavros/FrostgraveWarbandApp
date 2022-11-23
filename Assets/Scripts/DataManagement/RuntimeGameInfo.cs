using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGameInfo
{
    int creaturesKilled = 0;
    int spellsPassed = 0;
    int spellsFailed = 0;
    int treasuresCaptured = 0;
    public List<RuntimeSoldierData> monstersInGame = new List<RuntimeSoldierData>();

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

    public void SetSpellsPassed(int val)
    {
        spellsPassed = val;
    }

    public void SetSpellsFailed(int val)
    {
        spellsFailed = val;
    }

}
