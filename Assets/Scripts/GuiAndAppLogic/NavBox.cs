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

    [SerializeField] GameObject pirateOathStuff;

    [SerializeField] TextMeshProUGUI screenNameText;

    private AppFragment currentLocation = AppFragment.Home;

    public void OnClickNavHome()
    {
        mainButtonMenu.SetActive(true);
        spellReferencePanel.SetActive(false);
        wizardBuilder.SetActive(false);
        warbandManager.SetActive(false);
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
    }
}

public enum AppFragment
{
    Home,
    SpellReference,
    WizardBuilder,
    WarbandManagerMain,
    WarbandHiring,
    Other
}

