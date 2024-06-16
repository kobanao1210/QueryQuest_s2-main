using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Events;

public class GameUIManager : MonoBehaviour
{
    public static bool canShowMenu = false;  //メニュー画面画面表示可能か

    [SerializeField] private GameObject menuPanelBase;  //メニュー画面全体のパネル
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private List<GameObject> menuItems = new List<GameObject>();　　//メニューパネルの右側に表示されるパネルのリスト
    [SerializeField] private List<TextMeshProUGUI> statusTexts = new List<TextMeshProUGUI>();

    [SerializeField] private List<Button> itemButtons = new List<Button>();　　//アイテムリストの各アイテムボタン
    [SerializeField] private List<Button> itemActionButtons = new List<Button>();　　//使う・捨てる等のアイテムに対するアクションボタン
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private bool stoppedGameTimer = false;  //ゲームポーズ時に指定秒数後の判定に使用する変数
    private float setTimer = 0;
    private float time = 0;

    private PlayerDatabase playerDatabase;
    private AudioManager audioManager;


    private void Start()
    {
        playerDatabase = PlayerDatabase.Instance;
        audioManager = AudioManager.Instance;
        AllPanelActiveFalse();
        menuPanelBase.SetActive(false);
        fadePanel.SetActive(false);
        messagePanel.SetActive(false);
    }
    private void Update()
    {
        if (stoppedGameTimer) AddTime();
        if (Input.GetKeyDown(KeyCode.X) && canShowMenu)
        {
            ToggleMenuPanel();
        }
    }

    private void ToggleMenuPanel()　　//各種処理とメニュー画面の開け閉め
    {
        AllPanelActiveFalse();
        if (menuPanelBase.activeSelf)
        {
            Time.timeScale = 1;
            menuPanelBase.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            menuPanelBase.SetActive(true);
            SetStatus();  //とりあえず
            SetItem();
            itemInfoPanel.SetActive(false);
        }
    }

    private void AllPanelActiveFalse()　　//メニューパネル右側のパネルをすべて閉じる
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].SetActive(false);
        }
    }

    public void SetStatus()　　　//playerDatabaseからプレイヤーのステータス情報を取得しテキストリストに代入
    {
        var statusData = playerDatabase.SendArrayStatusData();
        for (int i = 0; i < statusTexts.Count; i++)
        {
            statusTexts[i].text = statusData[i].ToString();
        }
    }

    public void OnStatusButton()
    {
        AllPanelActiveFalse();
        menuItems[0].SetActive(true);
    }

    public void OnItemButton()
    {
        AllPanelActiveFalse();
        menuItems[1].SetActive(true);
    }

    public void OnLoadTitleButton()
    {
        var uiCanvas = GameObject.Find("UICanvas");
        var select = uiCanvas.transform.Find("SelectPanel").GetComponent<SelectButton>();
        var action = new UnityAction(Util.LoadTitleScene);
        ToggleMenuPanel();
        Time.timeScale = 0;
        select.ShowSelectPanel("セーブしていないデータは失われてしまいますが、このままタイトル画面に戻りますか？", action);
    }

    public void OnExitGameButton()
    {
        var uiCanvas = GameObject.Find("UICanvas");
        var select = uiCanvas.transform.Find("SelectPanel").GetComponent<SelectButton>();
        var action = new UnityAction(GameExit);
        ToggleMenuPanel();
        Time.timeScale = 0;
        select.ShowSelectPanel("セーブしていないデータは失われてしまいますが、このままゲーム終了しますか？", action);
    }

    private void GameExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SetItem()　　//アイテムパネル左側に所持しているアイテムの種類分ボタンを作る
    {
        ClearItemButtonEvent();
        var itemList = playerDatabase.haveItems;
        for (int i = 0; i < itemList.Count; i++)
        {
            SetItemButton(itemButtons[i], itemList[i]);
        }
    }

    public void SetItemInfo(Item item)　　//クリックされたアイテムの詳細情報をアイテムパネル右側のパネルに表示させる
    {
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{item.itemName}");
        itemInfoText.text = item.itemInfo;
        itemCountText.text = item.count + "個所持";
    }

    private void ClearItemButtonEvent()　　//アイテムボタンの設定前に各ボタンを初期化する
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].onClick.RemoveAllListeners();
            itemButtons[i].enabled = false;
            itemButtons[i].image.color = new Color(0, 0, 0, 255);
            itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    private void SetItemButton(Button button, Item item)　　//各アイテムボタンに情報を注入する
    {
        button.enabled = true;
        button.image.color = Color.white;
        button.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
        var itemButton = button.GetComponent<ItemButton>();
        if (itemButton == null)
        {
            button.AddComponent<ItemButton>();
            itemButton = button.GetComponent<ItemButton>();
        }
        button.onClick.AddListener(itemButton.OnItemButton);
        itemButton.item = item;
        itemButton.gameUIManager = this;
        itemButton.buttons = itemActionButtons;
        itemButton.itemInfoPanel = itemInfoPanel;
    }

    public void FadeOutPanel(int num)　　//指定秒数でフェードアウトさせ、完了後アルファ値を戻す
    {
        var fadeImage = fadePanel.GetComponent<Image>();
        ToggleFadePanel();
        fadeImage.color = new Color(0, 0, 0, 0);
        fadePanel.GetComponent<Image>().DOFade(1, num).OnComplete(ToggleFadePanel);
    }

    private void ToggleFadePanel()
    {
        CanShowMenu();
        if (fadePanel.activeSelf) fadePanel.SetActive(false);
        else fadePanel.SetActive(true);
    }

    public void ShowMessagePanel(string message, float num)　　//引数の文字列を画面上に、指定秒数間表示させる
    {
        ToggleMessagePanel();
        if (Time.timeScale == 0 && messagePanel.activeSelf) SetTime(num);
        else if (messagePanel.activeSelf) Invoke(nameof(ToggleMessagePanel), num);
        messagePanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    private void ToggleMessagePanel()
    {
        CanShowMenu();
        if (stoppedGameTimer) stoppedGameTimer = false;
        if (!messagePanel.activeSelf) messagePanel.SetActive(true);
        else messagePanel.SetActive(false);
    }

    public static void CanShowMenu()　　//現在メニュー画面が表示できるかを簡易的に管理
    {
        if (canShowMenu) canShowMenu = false;
        else canShowMenu = true;
    }

    private void SetTime(float num)　　//Time.Scale = 0 の際にも秒数指定イベントを起こすためのメソッド
    {
        time = 0;
        setTimer = num;
        stoppedGameTimer = true;
    }
    private void AddTime()
    {
        time += Time.unscaledDeltaTime;
        if (time > setTimer)
        {
            Invoke(nameof(ToggleMessagePanel), 0);
        }
    }
}
