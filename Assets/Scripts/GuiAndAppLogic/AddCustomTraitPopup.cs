using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddCustomTraitPopup : MonoBehaviour
{
    
    [SerializeField] TMP_InputField NameField;
    [SerializeField] TMP_InputField DescriptionField;

    [SerializeField] EditTraitsPopup editTraitsPopup;

    public void OnClickAddItemButton()
    {
        RuntimeMonsterKeyword rmk = new RuntimeMonsterKeyword();
        rmk.keywordName = NameField.text;
        rmk.keywordDescription = DescriptionField.text;

        editTraitsPopup.AddSoldierTrait(rmk);

        this.gameObject.SetActive(false);

    }



}
