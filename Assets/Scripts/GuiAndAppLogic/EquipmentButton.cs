using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ButtonText;
    
    private EquipmentScriptable referenceEquipment;

    public void SetEquipment(EquipmentScriptable setequip)
    {  
        referenceEquipment = setequip;
        ButtonText.text = referenceEquipment.equipmentName;
    }

    public EquipmentScriptable GetEquipment()
    {
        return referenceEquipment;
    }


}
