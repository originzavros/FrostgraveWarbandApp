using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NavBox : MonoBehaviour
{
    [SerializeField] GameObject mainButtonMenu;
    [SerializeField] GameObject spellReferencePanel;
    [SerializeField] GameObject wizardBuilder;
    [SerializeField] GameObject warbandManager;
    [SerializeField] GameObject campaignSettingsManager;
    [SerializeField] GameObject spellReference;

    [SerializeField] GameObject pirateOathStuff;

    [SerializeField] TextMeshProUGUI screenNameText;
    [SerializeField] BasicPopup infoPopup;

    private AppFragment currentLocation = AppFragment.Home;

    public void OnClickNavHome()
    {
        mainButtonMenu.SetActive(true);
        spellReferencePanel.SetActive(false);
        wizardBuilder.SetActive(false);
        warbandManager.SetActive(false);
        campaignSettingsManager.SetActive(false);
        ChangeScreenName("Home");
        currentLocation = AppFragment.Home;
    }

    public void ChangeScreenName(string name)
    {
        screenNameText.text = name;
    }

    public void GoToSpellReference()
    {
        mainButtonMenu.SetActive(false);
        spellReferencePanel.SetActive(true);
        spellReference.GetComponent<ScrollBox>().Init();
        ChangeScreenName("Spell Reference");
        currentLocation = AppFragment.SpellReference;
    }
    public void GoToWizardBuilder()
    {
        mainButtonMenu.SetActive(false);
        wizardBuilder.SetActive(true);
        wizardBuilder.GetComponent<WizardBuilder>().Init();
        ChangeScreenName("Wizard Builder");
        currentLocation = AppFragment.WizardBuilder;
    }

    public void GoToWarbandManager()
    {
        mainButtonMenu.SetActive(false);
        warbandManager.SetActive(true);
        warbandManager.GetComponent<WarbandUIManager>().Init();
        ChangeScreenName("Warband Manager");
        currentLocation = AppFragment.WarbandManagerSelection;
    }
    public void GoToCampaignSettings()
    {
        mainButtonMenu.SetActive(false);
        campaignSettingsManager.SetActive(true);
        currentLocation = AppFragment.CampaignSettings;
        // campaignSettingsManager.GetComponent<CampaignSettingsManager>().Init();
    }

    public void GoToWarbandManagerMain()
    {
        ChangeScreenName("Warband Manager");
        currentLocation = AppFragment.WarbandManagerMain;
    }

    public void GoToCrewabilities()
    {
        pirateOathStuff.SetActive(true);
        mainButtonMenu.SetActive(false);
        currentLocation = AppFragment.Other;
    }


    public void ChangeFragmentName(AppFragment fragment)
    {
        currentLocation = fragment;
    }

    //need update for checking android inputs
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android) {
            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape)) {
                HandleAndroidBackButton();
            }
        }
    }

    private void HandleAndroidBackButton()
    {
        Debug.Log("android back pressed");
        if(currentLocation == AppFragment.Other)
        {
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.SpellReference)
        {
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.WizardBuilder)
        {
            //might want to have wizard builder go back to previous step instead
            OnClickNavHome();
        }

        if(currentLocation == AppFragment.WarbandManagerCategory)
        {
            GoToWarbandManagerMain();
        }
        if(currentLocation == AppFragment.WarbandManagerMain)
        {
            GoToWarbandManager();
        }
        if (currentLocation == AppFragment.CampaignSettings)
        {
            OnClickNavHome();
        }
    }

    public void OnClickInfoButton()
    {
        infoPopup.gameObject.SetActive(true);
        if(currentLocation == AppFragment.Home)
        {
            string hometext = "Frostgrave is owned by Joseph A. McCullough and Published by Osprey Games.\n";
            hometext += "Please support Frostgrave by purchasing the books.\n";
            hometext += "This app is provided for free. If you would like to support me, paypal.me/NicholasAlaniz\n";
            hometext += "\n";
            hometext += "Use the wizard builder to set up your first wizard. Then go to the warband manager to hire soldiers including an Apprentice\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if(currentLocation == AppFragment.SpellReference)
        {
            string hometext = "You can sort by school and spell target type.\n";
            hometext += "Use the arrow to reset the filters.\n";
            hometext += "Spells from expansion books will show up if that expansion is enabled.\n";
            hometext += "You can navigate back to the home screen with the home button.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if(currentLocation == AppFragment.CampaignSettings)
        {
            string hometext = "Check which campaigns are enabled here, their respective soldiers/monsters/items will be shown.\n";
            hometext += "All Soldiers enables hiring from all campaign books. (note that soldiers like the Crowmaster does not have the normal hiring restrictions)\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if(currentLocation == AppFragment.WarbandManagerMain)
        {
            string hometext = "Use Hire soldiers to manage who is in your warband, remember to unequip any items attached to them using the vault before firing them.\n";
            hometext += "Shop/Vault lets you buy items and equip your soldiers with items. To finalize changes go to save/settings tab and save changes button\n";
            hometext += "Playgame will start a game with new game button. Ending game leads to assisted post game.\n";
            hometext += "Use Edit gold/xp to fix mistakes and account for extra scenario bonuses.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.HireSoldiers)
        {
            string hometext = "Here you can hire soldiers for your warband. Note that firing a soldier does not refund their cost.\n";
            hometext += "If you make a mistake in hiring, changes aren't saved until you press the finalize warband button, hit back arrow to cancel.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.ShopVault)
        {
            string hometext = "The Black Market resets every time its pressed. \n";
            hometext += "Use Custom Item to add unique campaign and scenario items to your crew. It will be added to your vault.\n";
            hometext += "You can only have one base at a time, you can change by selling the old one in the vault tab.\n";
            hometext += "Use the warband tab to equip and unequip items from soldiers.\n";
            hometext += "Make sure to press Save and Exit at the bottom when done with all changes to go back to the Warband Manager\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.PlayGame)
        {
            string hometext = "Press New Game to set up your warband for playing.\n";
            hometext += "On Soldier profiles, the Die rolls for that soldier, the unhappy face is for conditions (like poison, or spells like Blinding Light).\n";
            hometext += "Make sure to press the skull icon when the soldier is KO'd, the app will track them for the postgame.\n";
            hometext += "Tap the potion icons to adjust the soldier's health\n";
            hometext += "Add monster contains all non-character monsters from all enabled campaigns. For special characters please reference the related book.\n";
            hometext += "When adding monsters, roll random monster will give one of any monster and does not roll on a monster table.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.Postgame)
        {
            string hometext = "Only press Next when ready, there is no going back in the postgame assistant.\n";
            hometext += "For soldier injuries, you can select the soldier and use available spells/potions to attempt to resurrect(like miraculous cure).\n";
            hometext += "On the add treasures section, press a treasure to select the type (like central, or campaign specific).\n";
            hometext += "During wizard leveling, if you do not wish to level a stat or spell, simply press next to skip.\n";
            hometext += "After wizard leveling, rolls will be made for base resources and After Game spells.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.SoldierManager)
        {
            string hometext = "Here you can swap soldiers in and out of your main party, rename them, and add summonable soldiers.\n";
            hometext += "The bench can hold any amount of soldiers for ease of use, though the rules support only warbands with an Inn as having extra space for soldiers.\n";
            hometext += "When adding a summon, it counts as a full soldier, and is initially added to your bench.\n";
            hometext += "To remove soldiers, add them to the main warband, then go to Hire Soldiers and fire them.\n";
            infoPopup.UpdatePopupText(hometext);
        }
        else if (currentLocation == AppFragment.EditWarbandStats)
        {
            string hometext = "Mistakes happen, so here you can edit your warband xp and gold. Use Responsibly :) \n";
            infoPopup.UpdatePopupText(hometext);
        }
    }
}

public enum AppFragment
{
    Home,
    SpellReference,
    WizardBuilder,
    WarbandManagerSelection,
    WarbandManagerMain,
    WarbandManagerCategory,
    Other,
    CampaignSettings,
    HireSoldiers,
    ShopVault,
    PlayGame,
    Postgame,
    SoldierManager,
    EditWarbandStats
}

