using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class warbandXPGoldEditor : MonoBehaviour
{
    //[SerializeField] ModNumberPanel xpeditor;
    //[SerializeField] ModNumberPanel goldeditor;
    [SerializeField] WarbandInfoManager warbandInfoManager;

    [SerializeField] TMP_InputField goldTextField;
    [SerializeField] TMP_InputField xpTextField;
    [SerializeField] TMP_InputField wizardHpTextField;
    [SerializeField] TMP_InputField wizardFightTextField;
    [SerializeField] TMP_InputField wizardShootTextField;
    [SerializeField] TMP_InputField wizardWillTextField;


    [SerializeField] WarbandUIManager warbandUIManager;
    [SerializeField] BasicPopup errorPopup;

    [SerializeField] NavBox navBox;
    [SerializeField] GameObject editSpellPanelPrefab;

    [SerializeField] Transform mainContent;
    [SerializeField] SpellTextPopup spellTextPopup;

    [SerializeField] SpellSelectionHandler spellSelectionHandler;
    [SerializeField] GameObject spellSelectionPopup;

    public delegate void PopupDelegate(SpellButton sb);

    private PlayerWarband currentWarband;
    private int totalGold = 0;
    private int totalXp = 0;
    private int wizardHPStat = 0;
    private int wizardFightStat = 0;
    private int wizardShootStat = 0;
    private int wizardWillStat = 0;

    //private List<EditSpellPanel> spellPanels = new List<EditSpellPanel>();

    public void Init()
    {
        currentWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        // xpeditor.Init("Wizard XP", currentWarband.warbandWizard.playerWizardExperience);
        // goldeditor.Init("Total Gold", currentWarband.warbandGold);

        goldTextField.text = currentWarband.warbandGold.ToString();
        xpTextField.text = currentWarband.warbandWizard.playerWizardExperience.ToString();
        wizardHpTextField.text = currentWarband.warbandWizard.playerWizardProfile.health.ToString();
        wizardFightTextField.text = currentWarband.warbandWizard.playerWizardProfile.fight.ToString();
        wizardShootTextField.text = currentWarband.warbandWizard.playerWizardProfile.shoot.ToString();
        wizardWillTextField.text = currentWarband.warbandWizard.playerWizardProfile.will.ToString();


        PopulateWithSpells();

        

        navBox.ChangeFragmentName(AppFragment.EditWarbandStats);
    }

    public void FinalizeWarband()
    {
        // currentWarband.warbandGold = goldeditor.GetModNumberValue();
        // currentWarband.warbandWizard.playerWizardExperience = xpeditor.GetModNumberValue();

        currentWarband.warbandWizard.playerWizardExperience = totalXp;
        currentWarband.warbandGold = totalGold;
        currentWarband.warbandWizard.playerWizardProfile.health = wizardHPStat;
        currentWarband.warbandWizard.playerWizardProfile.fight = wizardFightStat;
        currentWarband.warbandWizard.playerWizardProfile.shoot = wizardShootStat;
        currentWarband.warbandWizard.playerWizardProfile.will = wizardWillStat;
        SaveSpells();

        warbandInfoManager.SaveCurrentWarband();


        warbandUIManager.BackToMain();
    }

    public void PopulateWithSpells()
    {
        foreach(var item in currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            CreateAndAddSpellPanel(item);
        }
    }

    public void CreateAndAddSpellPanel(WizardRuntimeSpell _spell)
    {
        GameObject temp = Instantiate(editSpellPanelPrefab);
        EditSpellPanel editSpellPanel = temp.GetComponent<EditSpellPanel>();
        //spellPanels.Add(editSpellPanel);
        PopupDelegate pass = EnableAndFillDescriptionPopUp;

        editSpellPanel.Init(_spell, pass);

        temp.transform.SetParent(mainContent);
    }

    public void SaveSpells()
    {
        currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells.Clear();

        foreach(Transform item in mainContent)
        {
            if(item.TryGetComponent<EditSpellPanel>(out EditSpellPanel temp))
            {
                WizardRuntimeSpell currentSpell = temp.GetRuntimeSpell();
                //int newCastingNum = temp.GetSpellTN();
                //if(newCastingNum > currentSpell.GetFullModedCastingNumber())
                //{
                //    currentSpell.currentWizardLev
                //}
                currentSpell.currentWizardLevelMod += temp.GetSpellTN() - currentSpell.GetFullModedCastingNumber();
                currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells.Add(currentSpell);
                Destroy(item.gameObject);
            }

        }

        //foreach (var item in spellPanels)
        //{
        //    WizardRuntimeSpell currentSpell = item.GetRuntimeSpell();
        //    currentSpell.currentWizardLevelMod = item.GetSpellTN();
        //    currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells.Add(currentSpell);

        //    Destroy(item.gameObject);
        //}
    }

    public void EnableAndFillDescriptionPopUp(SpellButton sb)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        WizardRuntimeSpell tempSpell = sb.referenceRuntimeSpell;
        // go.GetComponent<SpellButton>().referenceRuntimeSpell;
        spellTextPopup.UpdateRuntimeInfo(tempSpell);
    }


    public void OnClickAddSpellButton()
    {

        spellSelectionPopup.SetActive(true);
        List<WizardSchools> temp = new List<WizardSchools>();
        var temp2 = System.Enum.GetValues(typeof(WizardSchools));
        foreach(WizardSchools item in temp2)
        {
            temp.Add(item);
        }

        spellSelectionHandler.SetMaxSelectable(1000);
        spellSelectionHandler.GenerateContainersForSpellsFromMultipleSchools(temp);
    }

    public void OnClickFinalizeSpellsButton()
    {
        List<SpellScriptable> newSpells = spellSelectionHandler.GetCurrentlySelectedSpells();
        spellSelectionHandler.ClearSpellContainers();

        foreach(var item in newSpells)
        {
            WizardRuntimeSpell newTempSpell = new WizardRuntimeSpell();
            newTempSpell.Init(item);
            newTempSpell.wizardSchoolMod = WizardBuilder.CheckSpellAlignmentMod(newTempSpell, currentWarband.warbandWizard.playerWizardSpellbook.wizardSchool);

            CreateAndAddSpellPanel(newTempSpell);
            //currentWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells.Add(newTempSpell);
        }

        spellSelectionPopup.SetActive(false);
    }

    //public void RemoveSpell(EditSpellPanel temp)
    //{
    //    foreach(var item in spellPanels)
    //    {
    //        Debug.Log(item.GetRuntimeSpell().referenceSpell.Name);
    //    }
    //    spellPanels.Remove(temp);
    //    Debug.Log("Removing " + temp.GetRuntimeSpell().referenceSpell.Name);
    //    foreach (var item in spellPanels)
    //    {
    //        Debug.Log(item.GetRuntimeSpell().referenceSpell.Name);
    //    }

    //}







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
        int parseTotal = 0;
        if(int.TryParse(goldTextField.text,out totalGold))
        {
            parseTotal++;
        }
        

        if(int.TryParse(xpTextField.text,out totalXp))
        {
            parseTotal++;
        }
        

        if (int.TryParse(wizardHpTextField.text, out wizardHPStat))
        {
            parseTotal++;
        }
        

        if (int.TryParse(wizardShootTextField.text, out wizardShootStat))
        {
            parseTotal++;
        }
       

        if (int.TryParse(wizardFightTextField.text, out wizardFightStat))
        {
            parseTotal++;
        }

        if (int.TryParse(wizardWillTextField.text, out wizardWillStat))
        {
            parseTotal++;
        }

        if(parseTotal == 6)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
