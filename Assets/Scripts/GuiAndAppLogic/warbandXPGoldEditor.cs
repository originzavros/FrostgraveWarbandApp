using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class warbandXPGoldEditor : MonoBehaviour
{
    [SerializeField] ModNumberPanel xpeditor;
    [SerializeField] ModNumberPanel goldeditor;
    [SerializeField] WarbandInfoManager warbandInfoManager;

    [SerializeField] TMP_InputField goldTextField;
    [SerializeField] TMP_InputField xpTextField;

    [SerializeField] WarbandUIManager warbandUIManager;
    [SerializeField] BasicPopup errorPopup;

    private PlayerWarband currentWarband;
    private int totalGold = 0;
    private int totalXp = 0;

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        // xpeditor.Init("Wizard XP", currentWarband.warbandWizard.playerWizardExperience);
        // goldeditor.Init("Total Gold", currentWarband.warbandGold);

        goldTextField.text = currentWarband.warbandGold.ToString();
        xpTextField.text = currentWarband.warbandWizard.playerWizardExperience.ToString();
    }

    public void FinalizeWarband()
    {
        // currentWarband.warbandGold = goldeditor.GetModNumberValue();
        // currentWarband.warbandWizard.playerWizardExperience = xpeditor.GetModNumberValue();

        currentWarband.warbandWizard.playerWizardExperience = totalXp;
        currentWarband.warbandGold = totalGold;
        warbandInfoManager.SaveCurrentWarband();
        warbandUIManager.BackToMain();



    }

    public void ReadFields()
    {
        if(ParseFields())
        {
            FinalizeWarband();
        }
        else{
            errorPopup.EnablePopup();
            errorPopup.UpdatePopupText("Please enter a readable number into fields");
        }
    }

    private bool ParseFields()
    {
        bool canparse = false;
        if(int.TryParse(goldTextField.text,out totalGold))
        {
            canparse = true;
        }
        else{
            errorPopup.EnablePopup();
            errorPopup.UpdatePopupText("Please enter a readable number into fields");
        }

        if(int.TryParse(xpTextField.text,out totalXp))
        {
            canparse = true;
        }
        else{
            errorPopup.EnablePopup();
            errorPopup.UpdatePopupText("Please enter a readable number into fields");
        }


        return canparse;
        
    }
}
