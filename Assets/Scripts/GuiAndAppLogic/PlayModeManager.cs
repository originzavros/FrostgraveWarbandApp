using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeManager : MonoBehaviour
{
    [SerializeField] GameObject gamePanelContents;
    [SerializeField] GameObject warbandViewContents;
    [SerializeField] GameObject wizardViewContents;
    [SerializeField] GameObject soldierViewScroll;
    [SerializeField] GameObject wizardViewScroll;
    [SerializeField] GameObject monsterViewContents;
    [SerializeField] GameObject monsterViewScroll;
    [SerializeField] GameObject playModeWindowPrefab;
    [SerializeField] GameObject spellButtonPrefab;
    [SerializeField] WarbandInfoManager warbandInfoManager;

    [SerializeField] GameObject rollDicePopup;
    [SerializeField] GameObject addConditionPopup;
    [SerializeField] GameObject soldierEscapePopup;
    [SerializeField] GameObject changeSoldierNamePopup;
    [SerializeField] SpellTextPopup spellTextPopup;
    [SerializeField] MonsterKeywordPopup monsterKeywordPopup;
    [SerializeField] AddMonsterPopup addMonsterPopup;

    [SerializeField] GameObject newGameButton;

    [SerializeField] GameObject monsterKeywordButtonPrefab;

    private PlayerWarband currentGameWarband;

    private int monsterKillCount = 0;

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
        wizardViewScroll.SetActive(true);
    }
    public void OnClickBeastsButton()
    {
        DisableAllContents();
        monsterViewScroll.SetActive(true);
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
        wizardViewScroll.SetActive(false);
        monsterViewScroll.SetActive(false);
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
        PopulateWizardView();
        PopulateMonsterPopup();
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
        // GameObject wizardAttachPanel = wizardViewContents.transform.GetChild(0).gameObject;
        CreateAndAttachPlaymodeSoldierContainer(currentGameWarband.warbandWizard.playerWizardProfile, wizardViewContents);
        foreach(var spellItem in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            GameObject temp = Instantiate(spellButtonPrefab);
            temp.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(temp);});
            SpellButton sb = temp.GetComponent<SpellButton>();
            sb.LoadRuntimeSpellInfo(spellItem);
            temp.transform.SetParent(wizardViewContents.transform);
        }
    }

    public void PopulateMonsterPopup()
    {
        addMonsterPopup.Init();
    }
    public void EnableAndFillDescriptionPopUp(GameObject go)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        WizardRuntimeSpell tempSpell = go.GetComponent<SpellButton>().referenceRuntimeSpell;
        spellTextPopup.UpdateRuntimeInfo(tempSpell);
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

    private void CreateAndAttachMonsterContainer(MonsterScriptable incoming, GameObject attachedTo)
    {
        GameObject temp = Instantiate(playModeWindowPrefab);
        PlaymodeWindow csw = temp.GetComponentInChildren<PlaymodeWindow>();
        RuntimeSoldierData newMonster = new RuntimeSoldierData();
        newMonster.Init(incoming);
        csw.UpdatePanelInfo(newMonster);

        csw.SetRollDiceEvent(delegate{RollDicePopup(csw);});
        csw.SetStatusEvent(delegate{AddConditionPop(csw);});
        csw.SetDeathEscapeEvent(delegate{DeleteMonsterEvent(csw);});
        csw.SetEditEvent(delegate{AddChangeSoldierNamePopup(csw);});

        foreach(var _keyword in incoming.monsterKeywordList)
        {
            AddMonsterKeywordToMonster(csw, _keyword);
        }
        
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
    public void AddMonsterKeywordTextPopup(MonsterKeywordScriptable _keyword)
    {
        monsterKeywordPopup.gameObject.SetActive(true);
        monsterKeywordPopup.Init(_keyword);
    }

    public void DeleteMonsterEvent(PlaymodeWindow _playmodeWindow)
    {
        monsterKillCount++;
        Destroy(_playmodeWindow.gameObject);
    }
    public void AddMonsterToMonsterScroll(MonsterScriptable _monster)
    {
        CreateAndAttachMonsterContainer(_monster, monsterViewContents);
    }


    public void AddMonsterKeywordToMonster(PlaymodeWindow _playmodeWindow, MonsterKeywordScriptable _keyword)
    {
        GameObject temp = Instantiate(monsterKeywordButtonPrefab);
        temp.GetComponent<MonsterKeywordButton>().Init(_keyword);

        temp.GetComponent<MonsterKeywordButton>().SetPopupEvent(delegate {AddMonsterKeywordTextPopup(_keyword);});

        _playmodeWindow.AddItemToContents(temp);
    }

    public void OnClickAddMonster()
    {
        addMonsterPopup.gameObject.SetActive(true);
    }







}
