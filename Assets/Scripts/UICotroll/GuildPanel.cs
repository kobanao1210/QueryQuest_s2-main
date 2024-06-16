using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildPanel : MonoBehaviour
{
    private List<QuestButton> questButtons = new List<QuestButton>();�@�@//�{�[�h�ɒ���o�����N�G�X�g�̃{�^�����X�g�iinstantiate��i�[�j
    [SerializeField] private List<TextMeshProUGUI> questInfoTexts = new List<TextMeshProUGUI>();�@�@//�N�G�X�g�̏ڍ׏��e�L�X�g���X�g
    [SerializeField] private Button questAcceptButton;�@�@//�N�G�X�g����E�[�i�{�^��
    [SerializeField] public GameObject questInfoPanel;�@�@//�M���h�p�l���E���ɕ\�������I���N�G�X�g�̏ڍ׏��p�l��
    [SerializeField] private GameObject boardImage;
    [SerializeField] private GameObject questButtonPrefab;

    [SerializeField] private GameObject questResultPanel;�@�@
    [SerializeField] private Image questRewardItemImage;
    [SerializeField] private List<TextMeshProUGUI> questResultTexts = new List<TextMeshProUGUI>();

    public List<GameObject> questEnemies = new List<GameObject>();

    private void Start()
    {
        questResultPanel.SetActive(false);
        if(QuestManager.isShowQuestResult)
        {
            var eventDatabase = EventDatabase.Instance;
            ShowQuestResultPanel(eventDatabase.acceptQuest, QuestManager.isQuestResult);
        }
    }

    public void SetQuestButton()�@�@//�v���C���[������\�ȃN�G�X�g���������A�N�G�X�g�{�^�����{�[�h�ɒ���o��
    {
        ClearQuestButton();
        var rankPoint = PlayerDatabase.Instance.playerStatusData.rankpoint;
        var setQuestList = QuestManager.allQuests.Where(x => x.needpoint <= rankPoint).ToList();
        for (int i = 0; i < setQuestList.Count; i++)
        {
            GenerateQuestButton();
            questButtons[i].quest = setQuestList[i];
            questButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = setQuestList[i].questname;
            questButtons[i].questInfoPanel = questInfoPanel;
            questButtons[i].questInfoTexts = questInfoTexts;
            questButtons[i].acceptButton = questAcceptButton;
            questButtons[i].questEnemyPrefabs = questEnemies;
            questButtons[i].guildPanel = this;
        }
    }

    private void GenerateQuestButton()
    {
        var questButton = Instantiate(questButtonPrefab, boardImage.transform);
        questButtons.Add(questButton.GetComponent<QuestButton>());
    }

    private void ClearQuestButton()�@�@//�N�G�X�g�{�^�������������Ȃ��悤�ɑO��\���������N�G�X�g�{�^�����폜����
    {
        for(int i = questButtons.Count - 1; i >=0 ;i--)
        {
            Destroy(questButtons[i].gameObject);
            questButtons.RemoveAt(i);
        }
    }

    public void ShowQuestResultPanel(Quest quest,bool isSucces)�@�@//�N�G�X�g���ہE��V�����p�l���ɕ\��������
    {
        QuestManager.isShowQuestResult = false;
        questResultPanel.SetActive(true);
        if(isSucces)
        {
            questRewardItemImage.color = Color.white;
            questRewardItemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{quest.rewardItem}");
        }
        else questRewardItemImage.color = new Color(0,0,0,0);
        SetQuestResultTexts(quest,isSucces);
        Invoke(nameof(CloseQuestResultpanel), 2f);
    }

    private void CloseQuestResultpanel()
    {
        questResultPanel.SetActive(false);
        GameUIManager.CanShowMenu();
    }

    private void SetQuestResultTexts(Quest quest,bool isSucces)
    {
        questResultTexts[0].text = isSucces ? "�N�G�X�g����" : "�N�G�X�g���s";
        questResultTexts[1].text = isSucces ? $"�����N�|�C���g{quest.rewardrankpoint}��" : $"�����N�|�C���g{quest.rewardrankpoint / 2}��";
        questResultTexts[2].text = isSucces ? "�l������" : "������";
        questResultTexts[3].text = isSucces ? $"��{quest.rewardValue}�l������" : "";
    }
}
