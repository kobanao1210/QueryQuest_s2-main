using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] GameObject HPBarCanvas;　　　//敵にアタッチされるHPバーのキャンバスプレファブ

    [SerializeField] GameObject battleResultPanel;　　//戦闘終了時のリザルト画面
    [SerializeField] TextMeshProUGUI enemyNameText;
    [SerializeField] TextMeshProUGUI GetExpText;
    [SerializeField] TextMeshProUGUI GetMoneyText;
    [SerializeField] List<Image> getItemImages = new List<Image>();

    [SerializeField] GameObject levelUpPanel;　　　　//LVアップ後のステータス比較画面（リザルトの後に表示）
    [SerializeField] List<TextMeshProUGUI> beforeStatusTexts;
    [SerializeField] List<TextMeshProUGUI> afterStatusTexts;

    [SerializeField] private GameObject gameOverPanel;　　//ゲームオーバー時に表示するパネル

    private EventDatabase instance;
    private void Start()
    {
        instance = EventDatabase.Instance;
        battleResultPanel.SetActive(false);
        levelUpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        SetEnemyHPBar();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (battleResultPanel.activeSelf) CloseBattlePanel();
            else if (levelUpPanel.activeSelf && instance.isQuest)
            {
                QuestManager.QuestSucces(instance.acceptQuest);
                Util.LoadTownScene();
            }
            else if ((levelUpPanel.activeSelf && !instance.isQuest))
            {
                Util.LoadMapScene();
            }
        }
    }

    public void ShowBattlePanel(EnemyStatus enemyStatus)    //戦闘勝利のUIパネルを表示する(time.scale変更しゲーム時間も止める)
    {
        Util.PauseGame();
        battleResultPanel.SetActive(true);

        enemyNameText.text = enemyStatus.enemyName + "を";
        GetExpText.text = enemyStatus.haveExp.ToString() + "EXP";
        GetMoneyText.text = enemyStatus.haveMoney.ToString() + "Gを獲得した";

    }

    private void CloseBattlePanel()　　//LVアップ処理が行われていない場合はクリック後直ちにマップ画面へ遷移
    {
        battleResultPanel.SetActive(false);
        if (LevelManager.canLevelUp)
        {
            ShowLevelUpPanel(LevelManager.beforeStatusData, LevelManager.afterStatusData);
            LevelManager.canLevelUp = false;
        }
        else if (instance.isQuest)
        {
            QuestManager.QuestSucces(instance.acceptQuest);
            Util.LoadTownScene();
        }
        else Util.LoadMapScene();
    }

    public void ShowLevelUpPanel(PlayerStatusData beforeStatusData, PlayerStatusData afterStatusData)  //StatusDataを表示UIに合わせてコンバート
    {
        Util.PauseGame();
        levelUpPanel.SetActive(true);
        var beforeData = beforeStatusData.ConvertIntArray();
        var afterData = afterStatusData.ConvertIntArray();
        SetStatusTexts(beforeData, afterData);
        AudioManager.Instance.Play("lvup", AudioManager.EclipType.SE);
    }

    private void SetStatusTexts(int[] beforeData, int[] afterData)
    {
        for (int i = 0; i < beforeStatusTexts.Count; i++)
        {
            beforeStatusTexts[i].text = beforeData[i].ToString();
        }
        for (int i = 0; i < afterStatusTexts.Count; i++)
        {
            afterStatusTexts[i].text = afterData[i].ToString();
        }
    }

    public void SetItemsImage(List<Item> items)
    {
        ClearItemsImage();
        if (items.Count == 0) return;
        for (int i = 0; items.Count > i; i++)
        {
            getItemImages[i].color = Color.white;
            getItemImages[i].sprite = Resources.Load<Sprite>($"ItemSprite/{items[i].itemName}");
        }
    }

    private void ClearItemsImage()
    {
        var alphaColor = new Color(255, 255, 255, 0);
        getItemImages[0].color = alphaColor;
        getItemImages[1].color = alphaColor;

    }

    private void SetEnemyHPBar()
    {
        var enemis = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemis)
        {
            var position = enemy.transform.position + new Vector3(0, 1, 0);
            Instantiate(HPBarCanvas, position, Quaternion.identity, enemy.transform);
        }
    }

    public void ShowGameOverPanel()
    {
        GameUIManager.canShowMenu = false;
        gameOverPanel.SetActive(true);
    }

    public void ExitGameForGameOver()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void loadTitleForGameOver()
    {
        Util.LoadTitleScene();
    }
}
