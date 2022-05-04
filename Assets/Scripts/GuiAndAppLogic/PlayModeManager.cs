using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeManager : MonoBehaviour
{
    [SerializeField] GameObject gamePanelContents;
    [SerializeField] GameObject warbandViewContents;
    [SerializeField] GameObject soldierViewScroll;
    [SerializeField] GameObject playModeWindowPrefab;
    [SerializeField] WarbandInfoManager warbandInfoManager;

    [SerializeField] GameObject rollDicePopup;
    [SerializeField] GameObject addConditionPopup;
    [SerializeField] GameObject soldierEscapePopup;
    [SerializeField] GameObject changeSoldierNamePopup;

    [SerializeField] GameObject newGameButton;

    private PlayerWarband currentGameWarband;

    public void Init()
    {
        // PlayerWarband playerWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        // currentGameWarband = warbandInfoManager.LoadActiveGame(playerWarband.warbandName);
        // if(currentGameWarband.warbandName == "temp")
        // {
        //     // NewGameSetup(playerWarband);
        // }
        // else{
        //     NewGameSetup(currentGameWarband);
        // }
        
    }

    #region Playgame Tabs
    public void OnClickWarbandButton()
    {
        DisableAllContents();
        soldierViewScroll.SetActive(true);
    }
    public void OnClickWizardButton()
    {
        DisableAllContents();
    }
    public void OnClickBeastsButton()
    {
        DisableAllContents();
    }
    public void OnClickGameButton()
    {
        DisableAllContents();
        gamePanelContents.SetActive(true);
    }
    public void DisableAllContents()
    {
        gamePanelContents.SetActive(false);
        soldierViewScroll.SetActive(false);
    }
    #endregion

    public void OnClickNewGame()
    {
        currentGameWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        NewGameSetup(currentGameWarband);
        newGameButton.GetComponent<Button>().interactable = false;
    }
    public void OnClickEndGame()
    {
        
    }

    public void NewGameSetup(PlayerWarband _playerwarband)
    {
        currentGameWarband = _playerwarband;
        PopulateWarbandView();
    }

    public void PopulateWarbandView()
    {
        foreach(var item in currentGameWarband.warbandSoldiers)
        {
            CreateAndAttachPlaymodeSoldierContainer(item, warbandViewContents);
            
        }
    }

    public void PopulateWizardView()
    {

    }

    private void CreateAndAttachPlaymodeSoldierContainer(RuntimeSoldierData incoming, GameObject attachedTo)
    {
        GameObject temp = Instantiate(playModeWindowPrefab);
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        csw.UpdatePanelInfo(incoming);

        csw.SetRollDiceEvent(delegate{RollDicePopup(csw);});
        csw.SetStatusEvent(delegate{AddConditionPop(csw);});
        csw.SetDeathEscapeEvent(delegate{SoldierEscapePopup(csw);});
        csw.SetEditEvent(delegate{AddChangeSoldierNamePopup(csw);});
        
        temp.transform.SetParent(attachedTo.transform);
    }

    public void SaveActiveGame()
    {
        warbandInfoManager.SaveActiveGame(currentGameWarband);
    }

    public void RollDicePopup(PlaymodeWindow _playmodeWindow)
    {
        rollDicePopup.SetActive(true);
        rollDicePopup.GetComponent<RollDicePopup>().Init(_playmodeWindow.GetStoredSoldier());
    }

    public void AddConditionPop(PlaymodeWindow _playmodeWindow)
    {
        addConditionPopup.SetActive(true);
        addConditionPopup.GetComponent<AddConditionPopup>().Init(_playmodeWindow);
    }

    public void SoldierEscapePopup(PlaymodeWindow _playmodeWindow)
    {
        soldierEscapePopup.SetActive(true);
        soldierEscapePopup.GetComponent<SoldierEscapePopup>().Init(_playmodeWindow);
    }

    public void AddChangeSoldierNamePopup(PlaymodeWindow _playmodeWindow)
    {
        changeSoldierNamePopup.SetActive(true);
        changeSoldierNamePopup.GetComponent<ChangeSoldierNamePopup>().Init(_playmodeWindow);
    }




}
