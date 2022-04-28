using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireSoldiersManager : MonoBehaviour
{
    [SerializeField] GameObject soldierCollapsableWindowPrefab;
    [SerializeField] GameObject currentSoldiersScroll;
    [SerializeField] GameObject soldierHiringScrollBox;


    public void Init(PlayerWarband playerWarband)
    {
        //init current warband
        // foreach(var item in playerWarband.warbandSoldiers)
        // {
        //     GameObject temp = Instantiate(soldierCollapsableWindowPrefab);
        //     CollapsableSoldierWindow csw = temp.GetComponent<CollapsableSoldierWindow>();

        //     csw.UpdatePanelInfo(item);

        //     temp.transform.SetParent(currentSoldiersScroll.transform);
        // }

        //init hiring soldiers
        foreach(var item in LoadAssets.allSoldierObjects)
        {
            GameObject temp = Instantiate(soldierCollapsableWindowPrefab);
            CollapsableSoldierWindow csw = temp.GetComponent<CollapsableSoldierWindow>();

            csw.UpdatePanelInfo(item);

            temp.transform.SetParent(soldierHiringScrollBox.transform);
        }
    }

    public void OnClickViewWarband()
    {
        soldierHiringScrollBox.SetActive(false);
    }
    public void OnClickHireWarband()
    {
        soldierHiringScrollBox.SetActive(true);
    }


}
