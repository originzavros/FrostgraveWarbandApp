using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class PlayModeManager : MonoBehaviour
{
    [BoxGroup("Gui Contents")][SerializeField] GameObject gamePanelContents;
    [BoxGroup("Gui Contents")][SerializeField] GameObject warbandViewContents;
    [BoxGroup("Gui Contents")][SerializeField] GameObject wizardViewContents;
    [BoxGroup("Gui Panels")][SerializeField] GameObject soldierViewScroll;
    [BoxGroup("Gui Panels")][SerializeField] GameObject wizardViewScroll;
    [BoxGroup("Gui Contents")][SerializeField] GameObject monsterViewContents;
    [BoxGroup("Gui Panels")] [SerializeField] GameObject monsterViewScroll;
    [BoxGroup("Prefabs")][SerializeField] GameObject playModeWindowPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject spellButtonPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject itemSlotPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject infoDisplayElementPrefab;
    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] PostGameManager postGameManager;
    [SerializeField] WarbandUIManager warbandUIManager;

    [BoxGroup("Popups")][SerializeField] GameObject rollDicePopup;
    [BoxGroup("Popups")][SerializeField] GameObject addConditionPopup;
    [BoxGroup("Popups")][SerializeField] GameObject soldierEscapePopup;
    [BoxGroup("Popups")][SerializeField] GameObject changeSoldierNamePopup;
    [BoxGroup("Popups")][SerializeField] SpellTextPopup spellTextPopup;
    [BoxGroup("Popups")][SerializeField] MonsterKeywordPopup monsterKeywordPopup;
    [BoxGroup("Popups")][SerializeField] InjuryKeywordPopup injuryKeywordPopup;
    [BoxGroup("Popups")][SerializeField] AddMonsterPopup addMonsterPopup;
    [BoxGroup("Popups")][SerializeField] GameObject itemDescriptionPopup;

    [BoxGroup("GameButtons")][SerializeField] GameObject newGameButton;
    [BoxGroup("GameButtons")][SerializeField] GameObject endGameButton;

    [BoxGroup("Prefabs")][SerializeField] GameObject monsterKeywordButtonPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject modNumberPanelPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject injuryKeywordButtonPrefab;


    private PlayerWarband currentGameWarband;
    private RuntimeGameInfo gameInfo;

    private int spellsPassed = 0;
    private int spellsFailed = 0;
    private int treasuresCaptured = 0;

    public void Init()
    {
        OnClickGameButton();
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
        ClearContent(wizardViewContents);
        ClearContent(warbandViewContents);
        ClearContent(monsterViewContents);
        currentGameWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        NewGameSetup(currentGameWarband);
        newGameButton.GetComponent<Button>().interactable = false;
        endGameButton.GetComponent<Button>().interactable = true;
    }

    public void ClearContent(GameObject window)
    {
        foreach(Transform item in window.transform)
        {
            Destroy(item.gameObject);
        }
    }
    public void OnClickEndGame()
    {
        //update soldiers for warband and postgame
        //save game data (monsters killed, treasures captured ?)
        //go to post game?
        UpdateWarbandInfoWithGameInfo();
        newGameButton.GetComponent<Button>().interactable = true;
        endGameButton.GetComponent<Button>().interactable = false;
        warbandUIManager.SwitchToPostgameAndInit(gameInfo);
    }

    public void OnClickCancelGame()
    {
        //go through each window and clear it's contents, reset new game button
    }

    //just want to grab their status info for postgame.
    public void UpdateWarbandInfoWithGameInfo()
    {
        //they are all references to the original, so should be fine

        // currentGameWarband.warbandSoldiers.Clear();
        // foreach(Transform child in warbandViewContents.transform)
        // {
        //     RuntimeSoldierData rsd = child.GetComponent<PlaymodeWindow>().GetStoredSoldier();
        //     currentGameWarband.warbandSoldiers.Add(rsd);
        // }
        // currentGameWarband.warbandWizard.playerWizardProfile = wizardViewContents.GetComponentInChildren<PlaymodeWindow>().GetStoredSoldier();
        warbandInfoManager.SaveCurrentWarband();

        spellsPassed = FindAndRetrieveInfoFromModPanel("Spells Passed", wizardViewContents);
        spellsFailed = FindAndRetrieveInfoFromModPanel("Spells Failed", wizardViewContents);

        for(int i = spellsPassed; i > 0; i--)
        {
            gameInfo.PassSpell();
        }
        for(int i = spellsFailed; i > 0; i--)
        {
            gameInfo.FailSpell();
        }

    }

    public void NewGameSetup(PlayerWarband _playerwarband)
    {
        currentGameWarband = _playerwarband;
        PopulateWarbandView();
        PopulateWizardView();
        PopulateMonsterPopup();
        gameInfo = new RuntimeGameInfo();
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

        RollForPregameSpells();
        CreateAndAttachPlaymodeSoldierContainer(currentGameWarband.warbandWizard.playerWizardProfile, wizardViewContents);
        foreach(var spellItem in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            GameObject temp = Instantiate(spellButtonPrefab);
            temp.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(temp);});
            SpellButton sb = temp.GetComponent<SpellButton>();
            sb.LoadRuntimeSpellInfo(spellItem);
            temp.transform.SetParent(wizardViewContents.transform);
        }

        CreateAndAttachModNumberPanel("Spells Passed", wizardViewContents);
        CreateAndAttachModNumberPanel("Spells Failed", wizardViewContents);
    }

    public void RollForPregameSpells()
    {
        foreach(var item in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            if(item.referenceSpell.Restriction == "Out of Game(B)" || item.referenceSpell.Restriction == "Out of Game(B) OR Touch")
            {
                int  currentRoll = Random.Range(1, 20);
                if(currentRoll >= item.GetFullModedCastingNumber())
                {
                    GameObject temp = Instantiate(infoDisplayElementPrefab);
                    temp.GetComponent<InfoDisplayElement>().UpdateText("Wizard <color=green>Success</color>: " + currentRoll.ToString() + "\nSpell: " + item.referenceSpell.Name);
                    temp.transform.SetParent(wizardViewContents.transform);
                }
                else{
                    GameObject temp = Instantiate(infoDisplayElementPrefab);
                    temp.GetComponent<InfoDisplayElement>().UpdateText("Wizard <color=red>Fail</color>: " + currentRoll.ToString() + "\nSpell: " + item.referenceSpell.Name);
                    temp.transform.SetParent(wizardViewContents.transform);
                }
                currentRoll = Random.Range(1, 20);
                currentRoll -= 2; //apprentice roll mod
                if(currentRoll >= item.GetFullModedCastingNumber())
                {
                    GameObject temp = Instantiate(infoDisplayElementPrefab);
                    temp.GetComponent<InfoDisplayElement>().UpdateText("Apprentice <color=green>Success</color>: " + currentRoll.ToString() + "\nSpell: " + item.referenceSpell.Name);
                    temp.transform.SetParent(wizardViewContents.transform);
                } 
                else{
                    GameObject temp = Instantiate(infoDisplayElementPrefab);
                    temp.GetComponent<InfoDisplayElement>().UpdateText("Apprentice <color=red>Fail</color>: " + currentRoll.ToString() + "\nSpell: " + item.referenceSpell.Name);
                    temp.transform.SetParent(wizardViewContents.transform);
                }
            }
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

        if(incoming.soldierInventory.Count > 0)
        {
            foreach(var item in incoming.soldierInventory)
            {
                Debug.Log(item.itemName);
                GameObject newItemSlot = Instantiate(itemSlotPrefab);
                ItemSlotSoldier iss = newItemSlot.GetComponent<ItemSlotSoldier>();
                iss.SetItemDescriptionButtonEvent(delegate { AddItemInfoToItemPopup(item);});
                iss.SetItem(item);
                iss.SetItemToPlaymode();
                csw.AddItemToContents(newItemSlot);
            }
        }

        foreach(var _keyword in incoming.soldierPermanentInjuries)
        {
            AddInjuryKeywordToSoldier(csw, _keyword);
        }

        csw.SetBodyPermaActive();
        
        
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

    private void CreateAndAttachModNumberPanel(string name, GameObject attachedTo)
    {
        GameObject temp = Instantiate(modNumberPanelPrefab);
        temp.name = name;
        ModNumberPanel mnp = temp.GetComponent<ModNumberPanel>();
        mnp.Init(name);

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

    public void AddInjuryKeywordTextPopup(InjuryScriptable _keyword)
    {
        injuryKeywordPopup.gameObject.SetActive(true);
        injuryKeywordPopup.Init(_keyword);
    }

    public void DeleteMonsterEvent(PlaymodeWindow _playmodeWindow)
    {
        // monsterKillCount++;
        gameInfo.KillCreature();
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

    public void AddInjuryKeywordToSoldier(PlaymodeWindow _playmodeWindow, InjuryScriptable _keyword)
    {
        GameObject temp = Instantiate(injuryKeywordButtonPrefab);
        temp.GetComponent<InjuryKeywordButton>().Init(_keyword);

        temp.GetComponent<InjuryKeywordButton>().SetPopupEvent(delegate {AddInjuryKeywordTextPopup(_keyword);});

        _playmodeWindow.AddItemToContents(temp);
    }

    public void OnClickAddMonster()
    {
        addMonsterPopup.gameObject.SetActive(true);
    }

    public void AddItemInfoToItemPopup(MagicItemScriptable itemScriptable)
    {
        itemDescriptionPopup.gameObject.SetActive(true);
        itemDescriptionPopup.GetComponent<ItemDescriptionPopup>().Init(itemScriptable);
    }

    public int FindAndRetrieveInfoFromModPanel(string panelName, GameObject parentObject)
    {
        foreach(Transform item in parentObject.transform)
        {
            if(item.gameObject.name == panelName)
            {
                return item.gameObject.GetComponent<ModNumberPanel>().GetModNumberValue();
            }
        }
        return -1;
    }

}
