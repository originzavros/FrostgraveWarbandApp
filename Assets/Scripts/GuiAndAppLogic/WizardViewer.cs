using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class WizardViewer : MonoBehaviour
{
    [SerializeField] PlayModeManager playModeManager;
    [SerializeField] WarbandInfoManager warbandInfoManager;
    [SerializeField] WarbandUIManager warbandUIManager;

    [SerializeField] GameObject contents;
    [SerializeField] GameObject spellButtonNormalPrefab;
    [SerializeField] SpellTextPopup spellTextPopup;

    [SerializeField] GameObject wizardImagePrefab;
    [SerializeField] GameObject basicButtonPrefab;

    private PlayerWarband currentGameWarband;
    private string wizardPicturePath;
    private GameObject wizardPictureObject = null;

    public void Init()
    {
        currentGameWarband = warbandInfoManager.GetCurrentlyLoadedWarband();

        playModeManager.ClearContent(contents);

        CheckIfPictureExists();

        //create a picture button
        GameObject pictureButton = Instantiate(basicButtonPrefab);
        pictureButton.GetComponent<Button>().onClick.AddListener(delegate { OnTakePictureButton(); });
        pictureButton.GetComponentInChildren<TextMeshProUGUI>().text = "Take Wizard Picture";
        pictureButton.transform.SetParent(contents.transform, false);

        
        playModeManager.CreateAndAttachPlaymodeSoldierContainer(currentGameWarband.warbandWizard.playerWizardProfile, contents);

        foreach (var item in currentGameWarband.warbandWizard.playerWizardSpellbook.wizardSpellbookSpells)
        {
            GameObject temp = Instantiate(spellButtonNormalPrefab);
            temp.GetComponent<Button>().onClick.AddListener(delegate { EnableAndFillDescriptionPopUp(temp); });
            SpellButton sb = temp.GetComponent<SpellButton>();
            sb.LoadRuntimeSpellInfo(item);
            sb.SetColorBasedOnSpellSchool();
            temp.transform.SetParent(contents.transform, false);
        }
    }

    public void EnableAndFillDescriptionPopUp(GameObject go)
    {
        spellTextPopup.transform.gameObject.SetActive(true);
        WizardRuntimeSpell tempSpell = go.GetComponent<SpellButton>().referenceRuntimeSpell;
        spellTextPopup.UpdateRuntimeInfo(tempSpell);
    }

    public void OnClickBackButton()
    {
        warbandUIManager.BackToWarbandMain();
    }

    public void OnTakePictureButton()
    {
        TakePicture(512);
    }

    private void TakePicture(int maxSize)
    {
        if(wizardPictureObject != null)
        {
            Destroy(wizardPictureObject);
            wizardPictureObject = null;
        }


        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                ES3.Save( currentGameWarband.warbandWizard.playerWizardProfile.soldierName + "_picture", path);

                wizardPictureObject = Instantiate(wizardImagePrefab);
                wizardPictureObject.GetComponent<RawImage>().texture = texture;
                wizardPictureObject.gameObject.SetActive(true);
                wizardPictureObject.transform.SetParent(contents.transform, false);
            }
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }

    private void CheckIfPictureExists()
    {
        if(ES3.KeyExists(currentGameWarband.warbandWizard.playerWizardProfile.soldierName + "_picture"))
        {
            string path =  ES3.Load<string>(currentGameWarband.warbandWizard.playerWizardProfile.soldierName + "_picture");
            if(path != null)
            {
                Texture2D texture = NativeCamera.LoadImageAtPath(path, 512);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                wizardPictureObject = Instantiate(wizardImagePrefab);
                wizardPictureObject.GetComponent<RawImage>().texture = texture;
                wizardPictureObject.gameObject.SetActive(true);
                wizardPictureObject.transform.SetParent(contents.transform, false);
            }
        }
    }
}
