using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NavBox : MonoBehaviour
{
    [SerializeField] GameObject mainButtonMenu;
    [SerializeField] GameObject spellReferencePanel;
    [SerializeField] GameObject wizardBuilder;
    [SerializeField] GameObject warbandManager;
    [SerializeField] GameObject campaignSettingsManager;
    [SerializeField] GameObject spellReference;

    [SerializeField] GameObject pirateOathStuff;

    [SerializeField] TextMeshProUGUI screenNameText;
    [SerializeField] BasicPopup infoPopup;

    private AppFragment currentLocation = AppFragment.Home;

    public void OnClickNavHome()
    {
        mainButtonMenu.SetActive(true);
        spellReferencePanel.SetActive(false);
        wizardBuilder.SetActive(false);
        warbandManager.SetActive(false);
        campaignSettingsManager.SetActive(false);
        ChangeScreenName("Home");
        currentLocation = AppFragment.Home;
    }

    public void ChangeScreenName(string name)
    {
        screenNameText.text = name;
    }

    public void GoToSpellReference()
    {
        mainButtonMenu.SetActive(false);
        spellReferencePanel.SetActive(true);
        spellReference.GetComponent<ScrollBox>().Init();
        ChangeScreenName("Spell Reference");
        currentLocation = AppFragment.SpellReference;
    }
    public void GoToWizardBuilder()
    {
        mainButtonMenu.SetActive(false);
        wizardBuilder.SetActive(true);
        wizardBuilder.GetComponent<WizardBuilder>().Init();
        ChangeScreenName("Wizard Builder");
        currentLocation = AppFragment.WizardBuilder;
    }

    public void GoToWarbandManager()
    {
        mainButtonMenu.SetActive(false);
        warbandManager.SetActive(true);
        warbandManager.GetComponent<WarbandUIManager>().Init();
        ChangeScreenName("Warband Manager");
        currentLocation = AppFragment.WarbandManagerSelection;
    }
    public void GoToCampaignSettings()
    {
        mainButtonMenu.SetActive(false);
        campaignSettingsManager.SetActive(true);
        // campaignSettingsManager.GetComponent<CampaignSettingsManager>().Init();
    }

    public void GoToWarbandManagerMain()
    {
        ChangeScreenName("Warband Manager");
        currentLocation = AppFragment.WarbandManagerMain;
    }

    public void GoToCrewabilities()
    {
        pirateOathStuff.SetActive(true);
        mainButtonMenu.SetActive(false);
        currentLocation = AppFragment.Other;
    }

    //need update for checking android inputs
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape)) {
                HandleAndroidBackButton();
            }
        }
    }

    private void HandleAndroidBackButton()
    {
        Debug.Log("android back pressed");
        if(currentLocation == AppFragment.Other)
        {
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.SpellReference)
        {
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.WizardBuilder)
        {
            //might want to have wizard builder go back to previous step instead
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.WarbandManagerCategory)
        {
            GoToWarbandManagerMain();
        }
        if(currentLocation == AppFragment.WarbandManagerMain)
        {
            GoToWarbandManager();
        }
    }

    public void OnClickInfoButton()
    {
        infoPopup.gameObject.SetActive(true);
        if(currentLocation == AppFragment.Home)
        {
            string hometext = "Frostgrave is owned by Joseph A. McCullough and Published by Osprey Games.\n";
            hometext += "Please support Frostgrave by purchasing the books.\n";
            hometext += "This app is provided for free. If you would like to support me, paypal.me/NicholasAlaniz\n";
            hometext += "\n";
            hometext += "Use the wizard builder to set up your first wizard. Then go to the warband manager to hire soldiers including an Apprentice\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if(currentLocation == AppFragment.SpellReference)
        {
            
        }
        else if(currentLocation == AppFragment.WizardBuilder)
        {
            
        }
        else if(currentLocation == AppFragment.WarbandManagerMain)
        {
            string hometext = "Use Hire soldiers to manage who is in your warband, remember to unequip any items attached to them using the vault.\n";
            hometext += "Shop/Vault lets you buy items and equip your soldiers with items. To finalize changes go to save/settings tab and save changes button\n";
            hometext += "Playgame will start a game with new game button. Ending game leads to post game.\n";
            infoPopup.UpdatePopupText(hometext);
        }
    }
}

public enum AppFragment
{
    Home,
    SpellReference,
    WizardBuilder,
    WarbandManagerSelection,
    WarbandManagerMain,
    WarbandManagerCategory,
    Other
}

