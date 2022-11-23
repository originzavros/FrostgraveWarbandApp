using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddConditionPopup : MonoBehaviour
{
    [SerializeField] TMP_InputField customCondition;
    [SerializeField] TextMeshProUGUI targetNumberValue;

    PlaymodeWindow currentSoldierWindow;
    private int targetNumber = 14;

    public void Init(PlaymodeWindow _playmodeWindow)
    {
        targetNumber = 14;
        UpdateTargetNumberValue();
        currentSoldierWindow = _playmodeWindow;
    }

    public void AddStatusToPlaymodeWindow(string statusName)
    {
        currentSoldierWindow.AddStatus(statusName, targetNumber.ToString(),true);
        ClosePopup();
    }

    public void OnClickLeftTargetNumberButton()
    {
        if(targetNumber > 0)
        {
            targetNumber--;
        }
        UpdateTargetNumberValue();
    }
    public void OnClickRightTargetNumberButton()
    {   if(targetNumber < 20)
        {
            targetNumber++;
        }
        UpdateTargetNumberValue();
    }

    public void UpdateTargetNumberValue()
    {
        targetNumberValue.text = targetNumber.ToString();
    }


    public void OnClickCustomCondition()
    {
        if(customCondition.text != "")
        { 
            AddStatusToPlaymodeWindow(customCondition.text);
        }
    }

    public void OnClickQuickCondition(GameObject go)
    {
        AddStatusToPlaymodeWindow(go.GetComponentInChildren<TextMeshProUGUI>().text);
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }



    
}
