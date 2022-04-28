using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainNav : MonoBehaviour
{
    [SerializeField] NavBox navBox;


    public void OnClickSpellReferenceButton()
    {
        navBox.ChangeScreenName("Spell Reference");
        navBox.GoToSpellReference();
    }

    public void OnClickWizardBuilderButton()
    {
        navBox.ChangeScreenName("Wizard Builder");
        navBox.GoToWizardBuilder();
    }

    public void OnClickCrewAbilityButton()
    {
        navBox.GoToCrewabilities();
    }


    // public void OnNavHomeClick()
    // {
    //     this.gameObject.SetActive(true);
    // }
}
