using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DescriptionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionField;

    public void UpdateDescription(RuntimeSoldierData soldier)
    {
        string newText = soldier.soldierName + "\n" + soldier.description;
        descriptionField.text = newText;
        // if(descriptionField.text == "None")
        // {
        //     // this.transform.parent.gameObject.SetActive(false);
        //     this.gameObject.SetActive(false);
        // }
    }
}
