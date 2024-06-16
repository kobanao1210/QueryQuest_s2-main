using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public List<GameObject> questEnemyPrefabs = new List<GameObject>();
    public List<TextMeshProUGUI> questInfoTexts = new List<TextMeshProUGUI>();
    public GuildPanel guildPanel;
    public Quest quest;
    public Button acceptButton;
    public GameObject questInfoPanel;
    public TownUIManager townUIManager;

    public void OnQuestButton()
    {
        RemoveButtonListner();
        SetQuestInfo();
        SetAcceptbutton();
        questInfoPanel.SetActive(true);
    }

    private void SetQuestInfo()  //クリックされたクエストボタンの情報を表示させる
    {
        questInfoTexts[0].text = $"{quest.targetName}を";
        if (quest.questType == "Item")
        {
            questInfoTexts[1].text = $"{quest.targetValue}個";
            questInfoTexts[2].text = $"納品せよ";
            acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = "納品";
        }
        else
        {
            questInfoTexts[1].text = $"{quest.targetValue}体";
            questInfoTexts[2].text = $"討伐せよ";
            acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = "受諾";
        }
        questInfoTexts[3].text = $"報酬: {quest.rewardItem}";
        questInfoTexts[4].text = $"{quest.rewardValue}個";
        questInfoTexts[5].text = $"ランクポイント+ [{quest.rewardrankpoint}]";
    }

    private void SetAcceptbutton()
    {
        if (quest.questType == "Item") acceptButton.onClick.AddListener(OnAcceptItemButton);
        else acceptButton.onClick.AddListener(OnAcceptBattleButton);
    }

    private void OnAcceptItemButton()　　//採取クエストの納品ボタンが押された場合の処理
    {
        questInfoPanel.SetActive(false);
        var haveItems = PlayerDatabase.Instance.haveItems;
        if (CheckTargetItem(quest, PlayerDatabase.Instance.haveItems))
        {
            QuestManager.QuestSucces(quest);
            guildPanel.ShowQuestResultPanel(quest, true);
            QuestManager.isShowQuestResult = false;
        }
        else
        {
            var gameUiManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
            AudioManager.Instance.Play("bad", AudioManager.EclipType.SE);
            gameUiManager.ShowMessagePanel($"{quest.targetName}の数量が足りません", 1.5f);
        }
    }
    private void OnAcceptBattleButton()　//討伐クエストの受諾が押された場合の処理
    {
        var instance = EventDatabase.Instance;
        questInfoPanel.SetActive(false);
        instance.isQuest = true;
        instance.acceptQuest = quest;

        var enemylist = questEnemyPrefabs.Select(x => x.GetComponent<EnemyStatus>()).ToList();
        instance.questEnemyPrefab = enemylist.Find(x => x.enemyName == quest.targetName).gameObject;
        Util.LoadBattleScene();
    }

    private void RemoveButtonListner()
    {
        acceptButton.onClick.RemoveAllListeners();
    }

    private bool CheckTargetItem(Quest quest, List<Item> items)　　//inventrymanagerのアイテムリストからクエスト対象のアイテムを選択
    {
        if (!items.Any(x => x.itemName == quest.targetName)) return false;
        else
        {
            var targetItem = items.Find(x => x.itemName == quest.targetName);
            if (targetItem.count < quest.targetValue) return false;
            else
            {
                InventryManager.LostItem(targetItem, quest.targetValue);
                return true;
            }
        }
    }
}
