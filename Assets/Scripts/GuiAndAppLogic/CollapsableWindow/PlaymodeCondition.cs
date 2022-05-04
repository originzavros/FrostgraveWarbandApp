using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaymodeCondition : MonoBehaviour
{
    [SerializeField] GameObject removeConditionButton;
    [SerializeField] TextMeshProUGUI conditionNameText;
    [SerializeField] TextMeshProUGUI conditionTargetNumberText;

    public void UpdateCondition(string name, string targetNumber = "14")
    {
        conditionNameText.text = name;
        conditionTargetNumberText.text = "TN : " + targetNumber;
    }

    public void OnClickRemoveCondition()
    {
        Destroy(this.gameObject);
    }
}
