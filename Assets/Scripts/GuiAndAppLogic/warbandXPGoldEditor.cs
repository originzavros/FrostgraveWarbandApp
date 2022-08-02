using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warbandXPGoldEditor : MonoBehaviour
{
    [SerializeField] ModNumberPanel xpeditor;
    [SerializeField] ModNumberPanel goldeditor;
    [SerializeField] WarbandInfoManager warbandInfoManager;

    [SerializeField] WarbandUIManager warbandUIManager;

    private PlayerWarband currentWarband;

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        xpeditor.Init("Wizard XP", currentWarband.warbandWizard.playerWizardExperience);
        goldeditor.Init("Total Gold", currentWarband.warbandGold);
    }

    public void FinalizeWarband()
    {
        currentWarband.warbandGold = goldeditor.GetModNumberValue();
        currentWarband.warbandWizard.playerWizardExperience = xpeditor.GetModNumberValue();
        warbandInfoManager.SaveCurrentWarband();
        warbandUIManager.BackToMain();

    }
}
