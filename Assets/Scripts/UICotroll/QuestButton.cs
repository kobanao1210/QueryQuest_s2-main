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

    private void SetQuestInfo()  //�N���b�N���ꂽ�N�G�X�g�{�^���̏���\��������
    {
        questInfoTexts[0].text = $"{quest.targetName}��";
        if (quest.questType == "Item")
        {
            questInfoTexts[1].text = $"{quest.targetValue}��";
            questInfoTexts[2].text = $"�[�i����";
            acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = "�[�i";
        }
        else
        {
            questInfoTexts[1].text = $"{quest.targetValue}��";
            questInfoTexts[2].text = $"��������";
            acceptButton.GetComponentInChildren<TextMeshProUGUI>().text = "���";
        }
        questInfoTexts[3].text = $"��V: {quest.rewardItem}";
        questInfoTexts[4].text = $"{quest.rewardValue}��";
        questInfoTexts[5].text = $"�����N�|�C���g+ [{quest.rewardrankpoint}]";
    }

    private void SetAcceptbutton()
    {
        if (quest.questType == "Item") acceptButton.onClick.AddListener(OnAcceptItemButton);
        else acceptButton.onClick.AddListener(OnAcceptBattleButton);
    }

    private void OnAcceptItemButton()�@�@//�̎�N�G�X�g�̔[�i�{�^���������ꂽ�ꍇ�̏���
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
            gameUiManager.ShowMessagePanel($"{quest.targetName}�̐��ʂ�����܂���", 1.5f);
        }
    }
    private void OnAcceptBattleButton()�@//�����N�G�X�g�̎���������ꂽ�ꍇ�̏���
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

    private bool CheckTargetItem(Quest quest, List<Item> items)�@�@//inventrymanager�̃A�C�e�����X�g����N�G�X�g�Ώۂ̃A�C�e����I��
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
