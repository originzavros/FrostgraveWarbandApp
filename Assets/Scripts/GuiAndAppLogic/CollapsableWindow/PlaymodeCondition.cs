using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaymodeCondition : MonoBehaviour
{
    [SerializeField] GameObject removeConditionButton;
    [SerializeField] TextMeshProUGUI conditionNameText;
    [SerializeField] TextMeshProUGUI conditionTargetNumberText;

    private PlaymodeWindow parentContainer;
    private StatusInfo statusInfo;

    public void UpdateCondition(StatusInfo si, PlaymodeWindow m_parentContainer)
    {
        conditionNameText.text = si.statusName;
        conditionTargetNumberText.text = "TN : " + si.statusValue;
        parentContainer = m_parentContainer;
        statusInfo = si;
    }

    public void OnClickRemoveCondition()
    {
        parentContainer.RemoveStatus(statusInfo);
        Destroy(this.gameObject);
    }
}
