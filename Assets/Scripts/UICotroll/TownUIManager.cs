using System.Collections.Generic;
using UnityEngine;

public class TownUIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> UIPanels = new List<GameObject>();  //画面右部に表示されるUIパネル群
    [SerializeField] GuildPanel guildPanel;

    private void Start()
    {
        AllPanelActiveFalse();
        GameUIManager.canShowMenu = true;
    }

    private void AllPanelActiveFalse()
    {
        for (int i = 0; i < UIPanels.Count; i++)
        {
            UIPanels[i].SetActive(false);
        }
    }
    public void OnExitTownButton()
    {
        AllPanelActiveFalse();
        UIPanels[0].SetActive(true);
    }

    public void OnInnButton()
    {
        AllPanelActiveFalse();
        UIPanels[1].SetActive(true);
    }

    public void OnGuildButton()
    {
        AllPanelActiveFalse();
        guildPanel.SetQuestButton();
        guildPanel.questInfoPanel.SetActive(false);
        UIPanels[2].SetActive(true);
    }

    public void OnChurchButton()
    {
        AllPanelActiveFalse();
        UIPanels[3].SetActive(true);
    }

    public void OnPrayButton()
    {
        JsonManager.SavePlayerData(PlayerDatabase.Instance);
    }

    public void OnPrayLoadTitle()
    {
        JsonManager.SavePlayerData(PlayerDatabase.Instance);
        Util.LoadTitleScene();
    }
}
