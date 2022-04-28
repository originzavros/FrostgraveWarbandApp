using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DescriptionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionField;

    public void UpdateDescription(SoldierScriptable soldier)
    {
        descriptionField.text = soldier.description;
    }
}
