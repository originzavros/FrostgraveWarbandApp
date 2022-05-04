using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DescriptionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionField;

    public void UpdateDescription(RuntimeSoldierData soldier)
    {
        descriptionField.text = soldier.description;
        // if(descriptionField.text == "None")
        // {
        //     // this.transform.parent.gameObject.SetActive(false);
        //     this.gameObject.SetActive(false);
        // }
    }
}
