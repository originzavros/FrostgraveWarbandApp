using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WarbandLister : MonoBehaviour
{
    [SerializeField] GameObject contentBox;

    [SerializeField] GameObject basicButtonPrefab;
    [SerializeField] GameObject warbandContainerPrefab;
    [SerializeField] GameObject confirmationPopup;

    [SerializeField] WarbandUIManager warbandUIManager;
    
    private string currentlySelectedWarband = "";
    public void PopulateListerWithWarbands()
    {
        foreach(Transform item in contentBox.transform)
        {
            Destroy(item.gameObject);
        }
        LoadAssets.LoadWarbandNames();
        foreach(var item in LoadAssets.warbandNames)
        {
            GameObject temp = Instantiate(warbandContainerPrefab);
            GameObject warbandButton = temp.GetComponent<WarbandContainer>().GetWarbandButton();
            warbandButton.GetComponentInChildren<TMP_Text>().text = item;

            warbandButton.GetComponent<Button>().onClick.AddListener(delegate {GoToNextWindow(temp.GetComponentInChildren<TMP_Text>().text);});

            temp.GetComponent<WarbandContainer>().GetDeleteButton().GetComponent<Button>().onClick.AddListener(delegate {ConfirmationPopupInit(item);});

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

    public void DeleteWarband()
    {
        LoadAssets.warbandNames.Remove(currentlySelectedWarband);
        currentlySelectedWarband = "";
        ConfirmationPopup.OnConfirmChosen -= Confirmed;
        ES3.Save("warbandNames", LoadAssets.warbandNames);
        PopulateListerWithWarbands();
    }

    public void ConfirmationPopupInit(string name)
    {
        currentlySelectedWarband = name;
        confirmationPopup.SetActive(true);
        confirmationPopup.GetComponent<ConfirmationPopup>().Init("Delete " + name);
        ConfirmationPopup.OnConfirmChosen += Confirmed;
    }

    public void Confirmed(bool result)
    {
        if(result)
        {
            DeleteWarband();
        }
    }






}
