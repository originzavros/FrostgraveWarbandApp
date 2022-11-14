using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;

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
    [BoxGroup("Popups")][SerializeField] GameObject confirmationPopup;
    [BoxGroup("Popups")][SerializeField] GameObject spellRollDicePopup;
    [BoxGroup("Popups")] [SerializeField] GameObject monsterKOPopup;
    

    [BoxGroup("GameButtons")][SerializeField] GameObject newGameButton;
    [BoxGroup("GameButtons")][SerializeField] GameObject endGameButton;
    [BoxGroup("GameButtons")][SerializeField] GameObject cancelGameButton;

    [BoxGroup("Prefabs")][SerializeField] GameObject monsterKeywordButtonPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject modNumberPanelPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject injuryKeywordButtonPrefab;
    [BoxGroup("Prefabs")][SerializeField] SoldierScriptable warhoundPrefab;
    [BoxGroup("Prefabs")][SerializeField] GameObject spellDiceContainerPrefab;

    [SerializeField] NavBox navBox;



    private PlayerWarband currentGameWarband;
    private RuntimeGameInfo gameInfo;

    private int spellsPassed = 0;
    private int spellsFailed = 0;
    private int treasuresCaptured = 0;
    private List<ItemRemove> itemsToRemove = new List<ItemRemove>();

    public void Init()
    {
        OnClickGameButton();
        navBox.ChangeFragmentName(AppFragment.PlayGame);
    }

    #region Playgame Tabs
    public void OnClickWarbandButton()
    {
        // SaveWarbandState();
        DisableAllContents();
        soldierViewScroll.SetActive(true);
    }
    public void OnClickWizardButton()
    {
        // SaveWarbandState();
        DisableAllContents();
        wizardViewScroll.SetActive(true);
    }
    public void OnClickBeastsButton()
    {
        // SaveWarbandState();
        DisableAllContents();
        monsterViewScroll.SetActive(true);
    }
    public void OnClickGameButton()
    {
        // SaveWarbandState();
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
        itemsToRemove.Clear();
        currentGameWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
        // LoadWarbandState();
        NewGameSetup(currentGameWarband);
        newGameButton.GetComponent<Button>().interactable = false;
        endGameButton.GetComponent<Button>().interactable = true;
        cancelGameButton.GetComponent<Button>().interactable = true;
        OnClickWizardButton();
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
        ClearContent(wizardViewContents);
        ClearContent(warbandViewContents);
        ClearContent(monsterViewContents);
        newGameButton.GetComponent<Button>().interactable = true;
        endGameButton.GetComponent<Button>().interactable = false;
        cancelGameButton.GetComponent<Button>().interactable = true;

        warbandUIManager.SwitchToPostgameAndInit(gameInfo);
    }

    public void OnClickCancelGame()
    {
        //go through each window and clear it's contents, reset new game button
        confirmationPopup.SetActive(true);
        confirmationPopup.GetComponent<ConfirmationPopup>().Init("Cancel Current Game?");
        ConfirmationPopup.OnConfirmChosen += ReceiveCancelConfirmation;
    }

    public void CancelCurrentGame()
    {
        ClearContent(wizardViewContents);
        ClearContent(warbandViewContents);
        ClearContent(monsterViewContents);
        newGameButton.GetComponent<Button>().interactable = true;
        endGameButton.GetComponent<Button>().interactable = false;
        cancelGameButton.GetComponent<Button>().interactable = true;

        // string id = currentGameWarband.warbandName;
        // warbandInfoManager.DeleteActiveGame(id);

        warbandUIManager.BackToWarbandMain();
    }

    //these both need work, save states for games need a lot more to make it easy to do
    // public void SaveWarbandState()
    // {
    //     warbandInfoManager.SaveActiveGame(currentGameWarband);
    // }

    // public void LoadWarbandState()
    // {
    //     string loadName = currentGameWarband.warbandName;
    //     currentGameWarband = warbandInfoManager.LoadActiveGame(loadName);
    //     if(currentGameWarband.warbandName == "temp")
    //     {
    //         Debug.Log("Could not load active game");
    //         currentGameWarband = warbandInfoManager.GetCurrentlyLoadedWarband();
    //     }
    // }

    public void ReceiveCancelConfirmation(bool result)
    {
        if(result)
        {
            CancelCurrentGame();
        }
        ConfirmationPopup.OnConfirmChosen -= ReceiveCancelConfirmation;
        confirmationPopup.SetActive(false);
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

        RemoveItemsFromSoldiers();
        itemsToRemove.Clear();

        // warbandInfoManager.Init(currentGameWarband); //the current game warband should be an active game
        // warbandInfoManager.SaveCurrentWarband();

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
            if(item.status == SoldierStatus.ready)
            {
                CreateAndAttachPlaymodeSoldierContainer(item, warbandViewContents);
            }
                
        }
        foreach(var item in currentGameWarband.warbandVault)
        {
            if(item.itemName == "Kennel")
            {
                RuntimeSoldierData doggo = new RuntimeSoldierData();
                doggo.Init(warhoundPrefab);
                doggo.soldierName = "Kennel Hound";
                CreateAndAttachPlaymodeSoldierContainer(doggo, warbandViewContents);
            }
        }
    }

    public void PopulateWizardView()
    {
        // GameObject wizardAttachPanel = wizardViewContents.transform.GetChild(0).gameObject;

        RollForPregameSpells();
        CreateAndAttachPlaymodeSoldierContainer(currentGameWarband.warbandWizard.playerWizardProfile, wizardViewContents);
        foreach(var spellItem in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            GameObject temp = Instantiate(spellDiceContainerPrefab);
            SpellDiceContainer spellContainerButton = temp.GetComponent<SpellDiceContainer>();
            
            
            SpellButton sb = spellContainerButton.spellButtonPrefab.GetComponent<SpellButton>();
            spellContainerButton.spellButtonPrefab.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(sb);});
            sb.LoadRuntimeSpellInfo(spellItem);
            // temp.GetComponent<SpellDiceContainer>().rollDiceButton.onClick.AddListener(delegate {SpellRollDicePopup(spellItem);});
            // spellContainerButton.Init(spellItem);
            // Debug.Log(spellItem.referenceSpell.Name);
            spellContainerButton.SetRollDiceEvent(delegate {SpellRollDicePopup(spellItem);});

            temp.transform.SetParent(wizardViewContents.transform);
            // GameObject temp = Instantiate(spellButtonPrefab);
            // temp.GetComponent<Button>().onClick.AddListener(delegate {EnableAndFillDescriptionPopUp(temp);});
            // SpellButton sb = temp.GetComponent<SpellButton>();
            // sb.LoadRuntimeSpellInfo(spellItem);
            // temp.transform.SetParent(wizardViewContents.transform);
        }

        CreateAndAttachModNumberPanel("Spells Passed", wizardViewContents);
        CreateAndAttachModNumberPanel("Spells Failed", wizardViewContents);
    }

    public void RollForPregameSpells()
    {
        foreach(var item in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            if(CheckIfSpellIsRollableForPregame(item))
            {
                SpellRollAndResult(item);
                SpellRollAndResult(item, true);
            }
        }
    }

    public bool CheckIfSpellIsRollableForPregame(WizardRuntimeSpell wrs)
    {
        if(wrs.referenceSpell.Restriction == "Out of Game(B)")
        {
            return true;
        }
        else if(wrs.referenceSpell.Restriction == "Out of Game(B) OR Touch")
        {
            return true;
        }
        else if(wrs.referenceSpell.Name == "Summon Demon")
        {
            foreach(var item in currentGameWarband.warbandVault)
            {
                if(item.itemName == "Summoning Circle")
                {
                   return true;
                } 
            }
            return false;
        }
        else{
            return false;
        }
    }

    public void SpellRollAndResult(WizardRuntimeSpell wrs, bool isApprentice = false)
    {
        if(wrs.referenceSpell.Name == "Summon Demon") 
        {
            if(isApprentice)
            {
                RollForSummonDemon(wrs, "Apprentice");
            }
            else{
                RollForSummonDemon(wrs, "Wizard");
            }
        }
        else{
            int currentRoll = Random.Range(1, 20);
            int mods = CheckForSpellBonusesFromBaseResources(wrs.referenceSpell.Name);
            currentRoll += mods;
            string spellcasterType = "Wizard";
            if(isApprentice){ 
                spellcasterType = "Apprentice";
                currentRoll -= 2;    
            }
            if(currentRoll >= wrs.GetFullModedCastingNumber())
            {
                CreateAndAttachSpellRollSuccess(wrs,spellcasterType,currentRoll, mods);
            }
            else{
                CreateAndAttachSpellRollFail(wrs,spellcasterType,currentRoll, mods);
            }
        }
    }

    public void CreateAndAttachSpellRollSuccess(WizardRuntimeSpell wrs, string spellcasterType, int currentRoll, int mods = 0)
    {
        GameObject temp = Instantiate(infoDisplayElementPrefab);
        temp.GetComponent<InfoDisplayElement>().UpdateText(spellcasterType + " <color=green>Success</color>: " + currentRoll.ToString() + "(Mod: "+ mods.ToString() + ")" + "\nSpell: " + wrs.referenceSpell.Name);
        temp.transform.SetParent(wizardViewContents.transform);
    }

    public void CreateAndAttachSpellRollFail(WizardRuntimeSpell wrs, string spellcasterType, int currentRoll, int mods = 0)
    {
        GameObject temp = Instantiate(infoDisplayElementPrefab);
        temp.GetComponent<InfoDisplayElement>().UpdateText(spellcasterType +" <color=red>Fail</color>: " + currentRoll.ToString() + "(Mod: "+ mods.ToString() + ")" + "\nSpell: " + wrs.referenceSpell.Name);
        temp.transform.SetParent(wizardViewContents.transform);
    }

    public void RollForSummonDemon(WizardRuntimeSpell wrs, string spellcasterType)
    {
        bool hasArcaneCandle = false;
        bool hasSummoningCandle = false;
        WizardRuntimeSpell ControlDemonSpell = null;
        foreach(var spell in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            if(spell.referenceSpell.Name == "Control Demon")
            {
                ControlDemonSpell = spell;
            }
        }

        if(ControlDemonSpell == null)
        {
            string displayInfo = "Have Summon Circle But No Control Demon Spell";
            CreateAndAttachInfoDisplay(displayInfo, wizardViewContents);
        }
        else{
            foreach(var item in currentGameWarband.warbandVault)
            {
                if(item.itemName == "Arcane Candle")
                {
                    hasArcaneCandle = true;
                }
                else if(item.itemName == "Summoning Candle")
                {
                    hasSummoningCandle = true;
                }
            }
            
            int summonRoll = Random.Range(1, 20);
            int controlRoll = Random.Range(1, 20);
            if(spellcasterType == "Apprentice")
            {
                summonRoll -= 2;
                controlRoll -= 2;
            }
            if(hasSummoningCandle){summonRoll += 1;}
            if(summonRoll >= wrs.GetFullModedCastingNumber())
            {
                CreateAndAttachSpellRollSuccess(wrs, spellcasterType, summonRoll);
                if(hasArcaneCandle){controlRoll += 1;}
                if(controlRoll >= ControlDemonSpell.GetFullModedCastingNumber())
                {
                    CreateAndAttachSpellRollSuccess(ControlDemonSpell, spellcasterType, controlRoll);
                    int rollDifference = summonRoll - wrs.GetFullModedCastingNumber();
                    if( rollDifference < 6 )
                    {
                        string displayInfo = "Summoning An Imp";
                        CreateAndAttachInfoDisplay(displayInfo, wizardViewContents);
                    }
                    else if( rollDifference > 5 && rollDifference < 13)
                    {
                        string displayInfo = "Summoning A Minor Demon";
                        CreateAndAttachInfoDisplay(displayInfo, wizardViewContents);
                    }
                    else if( rollDifference > 12)
                    {
                        string displayInfo = "Summoning A Major Demon";
                        CreateAndAttachInfoDisplay(displayInfo, wizardViewContents);
                    }
                }
                else{
                    CreateAndAttachSpellRollFail(wrs, spellcasterType, controlRoll);
                }
            }
            else{
                CreateAndAttachSpellRollFail(wrs, spellcasterType, summonRoll);
            }
        }        
    }

    public int CheckForSpellBonusesFromBaseResources(string spellName)
    {
        int mod = 0;
        foreach(var item in currentGameWarband.warbandVault)
        {
            if(item.itemName == "Crypt") //+2 raise zombie and animate skull
            {
                if(spellName == "Raise Zombie")
                {
                    mod += 2;
                }
                else if(spellName == "Animate Skull")
                {
                    mod += 2;
                }
            }
            else if(item.itemName == "Tower")//+2 for reveal secret and awareness
            {
                if(spellName == "Reveal Secret")
                {
                    mod += 2;
                }
                else if(spellName == "Awareness")
                {
                    mod += 2;
                }
            }
            else if(item.itemName == "Giant Cauldron")//+1 brew potion
            {
                if(spellName == "Brew Potion")
                {
                    mod += 1;
                }
            }
            else if(item.itemName == "Enchanter's Workshop")//+1 animate construct, embed enchantment
            {
                if(spellName == "Animate Construct")
                {
                    mod += 1;
                }
                else if(spellName == "Embed Enchantment")
                {
                    mod += 1;
                }
            }
            else if(item.itemName == "Crystal Ball")//+1 reveal secret
            {
                if(spellName == "Reveal Secret")
                {
                    mod += 1;
                }
            }
            else if(item.itemName == "Scriptorium")//+1 write scroll
            {
                if(spellName == "Write Scroll")
                {
                    mod += 1;
                }
            }
        }
        return mod;
    }


    public void CreateAndAttachInfoDisplay(string displayInfo, GameObject attachTo)
    {
        GameObject temp = Instantiate(infoDisplayElementPrefab);
        temp.GetComponent<InfoDisplayElement>().UpdateText(displayInfo);
        temp.transform.SetParent(attachTo.transform);
    }

    public void PopulateMonsterPopup()
    {
        addMonsterPopup.Init(LoadAssets.allMonsterObjects.ToList());
        addMonsterPopup.AssignMonsterEvent(delegate { AddMonsterToMonsterScroll(); });
    }
    public void EnableAndFillDescriptionPopUp(SpellButton sb)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        WizardRuntimeSpell tempSpell = sb.referenceRuntimeSpell;
        // go.GetComponent<SpellButton>().referenceRuntimeSpell;
        spellTextPopup.UpdateRuntimeInfo(tempSpell);
    }

    public void CreateAndAttachPlaymodeSoldierContainer(RuntimeSoldierData incoming, GameObject attachedTo)
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
                iss.SetUseItemEvent(delegate {UseItemEvent(item, iss, incoming);});
                iss.SetItemToPlaymode();
                csw.AddItemToContents(newItemSlot);
            }
        }

        foreach(var _keyword in incoming.soldierPermanentInjuries)
        {
            AddInjuryKeywordToSoldier(csw, _keyword);
        }

        //for soldier abilities/monsters in warband
        foreach(var _keyword in incoming.monsterKeywordList)
        {
            AddMonsterKeywordToMonster(csw, _keyword);
        }

        csw.SetBodyPermaActive();
        
        
        temp.transform.SetParent(attachedTo.transform);
    }

    private void CreateAndAttachMonsterContainer(SoldierScriptable incoming, GameObject attachedTo)
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
            RuntimeMonsterKeyword newKeyword = new RuntimeMonsterKeyword();
            newKeyword.Init(_keyword);
            AddMonsterKeywordToMonster(csw, newKeyword);
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

    public void SpellRollDicePopup(WizardRuntimeSpell wrs)
    {
        spellRollDicePopup.SetActive(true);
        int mods = CheckForSpellBonusesFromBaseResources(wrs.referenceSpell.Name);
        // Debug.Log(wrs.referenceSpell.Name);
        spellRollDicePopup.GetComponent<RollSpellPopup>().Init(wrs, mods);
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
    public void AddMonsterKeywordTextPopup(RuntimeMonsterKeyword _keyword)
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
        //MonsterKOPopup
        monsterKOPopup.SetActive(true);
        monsterKOPopup.GetComponent<MonsterKOPopup>().Init(_playmodeWindow);
    }

    public void SlayMonsterEvent(bool playerSlayed, PlaymodeWindow _playmodeWindow)
    {
        if(playerSlayed)
        {
            gameInfo.KillCreature();
        }
        Destroy(_playmodeWindow.gameObject);
    }
    public void AddMonsterToMonsterScroll()
    {
        SoldierScriptable _monster = addMonsterPopup.GetMonster();
        CreateAndAttachMonsterContainer(_monster, monsterViewContents);
    }


    public void AddMonsterKeywordToMonster(PlaymodeWindow _playmodeWindow, RuntimeMonsterKeyword _keyword)
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

    public void AddItemInfoToItemPopup(MagicItemRuntime itemScriptable)
    {
        itemDescriptionPopup.gameObject.SetActive(true);
        itemDescriptionPopup.GetComponent<ItemDescriptionPopup>().Init(itemScriptable);
    }

    public void UseItemEvent(MagicItemRuntime _item, ItemSlotSoldier iss, RuntimeSoldierData rsd)
    {
        if(_item.itemType == MagicItemType.Scroll || _item.itemType == MagicItemType.LesserPotion || _item.itemType == MagicItemType.GreaterPotion)
        {
            // Destroy(iss.gameObject);
            // var match = rsd.soldierInventory.FirstOrDefault(x => x.itemName == _item.itemName);
            // if(match != null)
            // {
            //     rsd.soldierInventory.Remove(match);
            // }
            ItemRemove temp = new ItemRemove();
            temp.soldier = rsd;
            temp.item = _item;
            itemsToRemove.Add(temp);
        }
    }

    public void RemoveItemsFromSoldiers()
    {
        foreach(var remove in itemsToRemove)
        {
            var match = remove.soldier.soldierInventory.FirstOrDefault(x => x.itemName == remove.item.itemName);
            if(match != null)
            {
                remove.soldier.soldierInventory.Remove(match);
            }
        }
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

    public struct ItemRemove{
        public RuntimeSoldierData soldier;
        public MagicItemRuntime item;
    }

}
