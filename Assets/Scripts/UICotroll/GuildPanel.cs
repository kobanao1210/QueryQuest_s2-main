using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildPanel : MonoBehaviour
{
    private List<QuestButton> questButtons = new List<QuestButton>();　　//ボードに張り出されるクエストのボタンリスト（instantiate後格納）
    [SerializeField] private List<TextMeshProUGUI> questInfoTexts = new List<TextMeshProUGUI>();　　//クエストの詳細情報テキストリスト
    [SerializeField] private Button questAcceptButton;　　//クエスト受諾・納品ボタン
    [SerializeField] public GameObject questInfoPanel;　　//ギルドパネル右側に表示される選択クエストの詳細情報パネル
    [SerializeField] private GameObject boardImage;
    [SerializeField] private GameObject questButtonPrefab;

    [SerializeField] private GameObject questResultPanel;　　
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

    public void SetQuestButton()　　//プレイヤーが受諾可能なクエスト数分だけ、クエストボタンをボードに張り出す
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

    private void ClearQuestButton()　　//クエストボタンが増え続けないように前回表示させたクエストボタンを削除する
    {
        for(int i = questButtons.Count - 1; i >=0 ;i--)
        {
            Destroy(questButtons[i].gameObject);
            questButtons.RemoveAt(i);
        }
    }

    public void ShowQuestResultPanel(Quest quest,bool isSucces)　　//クエスト成否・報酬等をパネルに表示させる
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
        questResultTexts[0].text = isSucces ? "クエスト成功" : "クエスト失敗";
        questResultTexts[1].text = isSucces ? $"ランクポイント{quest.rewardrankpoint}を" : $"ランクポイント{quest.rewardrankpoint / 2}を";
        questResultTexts[2].text = isSucces ? "獲得した" : "失った";
        questResultTexts[3].text = isSucces ? $"を{quest.rewardValue}獲得した" : "";
    }
}
