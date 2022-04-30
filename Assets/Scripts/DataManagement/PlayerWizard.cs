using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWizard
{
    public void Init()
    {
        playerWizardLevel = 0;
        playerWizardExperience = 0;
        playerWizardProfile = new RuntimeSoldierData();
        playerWizardSpellbook = new WizardSpellbook();
    }

    public int playerWizardLevel;
    public int playerWizardExperience;

    public string wizardProfilekey;

    
    [ES3NonSerializable]
    public RuntimeSoldierData playerWizardProfile;

    public WizardSpellbook playerWizardSpellbook;


}

// public class RuntimePlayerWizard
// {
//     public void Init(PlayerWizard pw)
//     {
//         playerWizardLevel = pw.playerWizardLevel;
//         playerWizardExperience = pw.playerWizardExperience;
//         playerWizardSpellbook = pw.playerWizardSpellbook;
//     }

//     public int playerWizardLevel;
//     public int playerWizardExperience;

//     public WizardSpellbook playerWizardSpellbook;
// }
