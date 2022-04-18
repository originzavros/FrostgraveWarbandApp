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

    [SerializeField] TextMeshProUGUI screenNameText;

    public void OnClickNavHome()
    {
        mainButtonMenu.SetActive(true);
        spellReferencePanel.SetActive(false);
        wizardBuilder.SetActive(false);
        warbandManager.SetActive(false);
        ChangeScreenName("Home");
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
    }
    public void GoToWizardBuilder()
    {
        mainButtonMenu.SetActive(false);
        wizardBuilder.SetActive(true);
        wizardBuilder.GetComponent<WizardBuilder>().Init();
        ChangeScreenName("Wizard Builder");
    }

    public void GoToWarbandManager()
    {
        mainButtonMenu.SetActive(false);
        warbandManager.SetActive(true);
        warbandManager.GetComponent<WarbandUIManager>().Init();
        ChangeScreenName("Warband Manager");
    }
}
