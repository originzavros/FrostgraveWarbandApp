using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WarbandLister : MonoBehaviour
{
    [SerializeField] GameObject contentBox;

    [SerializeField] GameObject basicButtonPrefab;

    [SerializeField] WarbandUIManager warbandUIManager;
    
    public void PopulateListerWithWarbands()
    {
        LoadAssets.LoadWarbandNames();
        foreach(var item in LoadAssets.warbandNames)
        {
            GameObject temp = Instantiate(basicButtonPrefab);
            temp.GetComponentInChildren<TMP_Text>().text = item;

            temp.GetComponent<Button>().onClick.AddListener(delegate {GoToNextWindow(temp.GetComponentInChildren<TMP_Text>().text);});
            
            
            // WizardSchoolButton wb = temp.GetComponent<WizardSchoolButton>();
            // wb.SetWizardSchool(item.primarySchool);
            // wb.schoolScriptableReference = item;
            // temp.transform.parent = scrollContainer.transform;
            temp.transform.SetParent(contentBox.transform, false);
        }
    }

    public void GoToNextWindow(string name)
    {
        if(warbandUIManager != null)
        {
            warbandUIManager.WarbandSelected(name);
        }
    }




}
