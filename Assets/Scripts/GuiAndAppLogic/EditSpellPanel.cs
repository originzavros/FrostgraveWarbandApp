using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EditSpellPanel : MonoBehaviour
{

    //[SerializeField] TMP_InputField spellTNField;

    [SerializeField] SpellButton _spellButton;
    [SerializeField] ModNumberPanel spellTNField;

    private warbandXPGoldEditor WarbandXPGoldEditor;
    private WizardRuntimeSpell currentSpell;

    public void Init(WizardRuntimeSpell _spell, warbandXPGoldEditor.PopupDelegate popupDelegate)
    {
        //spellTNField.text = currentTN;
        _spellButton.LoadRuntimeSpellInfo(_spell);
        spellTNField.Init("Spell TN: ", _spell.GetFullModedCastingNumber());
        currentSpell = _spell;

        _spellButton.GetComponent<Button>().onClick.AddListener(delegate { popupDelegate(_spellButton); });
    }

    public int GetSpellTN()
    {
        return spellTNField.GetModNumberValue();
    }

    public WizardRuntimeSpell GetRuntimeSpell()
    {
        return currentSpell;
    }

    

    public void OnDeleteClicked()
    {
        //parent delete spell from list
        //WarbandXPGoldEditor.RemoveSpell(this);
        DeleteContainer();
    }

    private void DeleteContainer()
    {
        Destroy(this.gameObject);
    }

    //public void EnableAndFillDescriptionPopUp(SpellButton sb)
    //{

       
    //    //spellContainerButton.spellButtonPrefab.GetComponent<Button>().onClick.AddListener(delegate { EnableAndFillDescriptionPopUp(_spellButton); });
    //    //sb.LoadRuntimeSpellInfo(spellItem);

    //    //spellTextPopup.transform.gameObject.SetActive(true);
    //    //WizardRuntimeSpell tempSpell = sb.referenceRuntimeSpell;
    //    //// go.GetComponent<SpellButton>().referenceRuntimeSpell;
    //    //spellTextPopup.UpdateRuntimeInfo(tempSpell);
    //}
}
