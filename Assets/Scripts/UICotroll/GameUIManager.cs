using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Events;

public class GameUIManager : MonoBehaviour
{
    public static bool canShowMenu = false;  //���j���[��ʉ�ʕ\���\��

    [SerializeField] private GameObject menuPanelBase;  //���j���[��ʑS�̂̃p�l��
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private List<GameObject> menuItems = new List<GameObject>();�@�@//���j���[�p�l���̉E���ɕ\�������p�l���̃��X�g
    [SerializeField] private List<TextMeshProUGUI> statusTexts = new List<TextMeshProUGUI>();

    [SerializeField] private List<Button> itemButtons = new List<Button>();�@�@//�A�C�e�����X�g�̊e�A�C�e���{�^��
    [SerializeField] private List<Button> itemActionButtons = new List<Button>();�@�@//�g���E�̂Ă铙�̃A�C�e���ɑ΂���A�N�V�����{�^��
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInfoText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private bool stoppedGameTimer = false;  //�Q�[���|�[�Y���Ɏw��b����̔���Ɏg�p����ϐ�
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

    private void ToggleMenuPanel()�@�@//�e�폈���ƃ��j���[��ʂ̊J����
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
            SetStatus();  //�Ƃ肠����
            SetItem();
            itemInfoPanel.SetActive(false);
        }
    }

    private void AllPanelActiveFalse()�@�@//���j���[�p�l���E���̃p�l�������ׂĕ���
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].SetActive(false);
        }
    }

    public void SetStatus()�@�@�@//playerDatabase����v���C���[�̃X�e�[�^�X�����擾���e�L�X�g���X�g�ɑ��
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
        select.ShowSelectPanel("�Z�[�u���Ă��Ȃ��f�[�^�͎����Ă��܂��܂����A���̂܂܃^�C�g����ʂɖ߂�܂����H", action);
    }

    public void OnExitGameButton()
    {
        var uiCanvas = GameObject.Find("UICanvas");
        var select = uiCanvas.transform.Find("SelectPanel").GetComponent<SelectButton>();
        var action = new UnityAction(GameExit);
        ToggleMenuPanel();
        Time.timeScale = 0;
        select.ShowSelectPanel("�Z�[�u���Ă��Ȃ��f�[�^�͎����Ă��܂��܂����A���̂܂܃Q�[���I�����܂����H", action);
    }

    private void GameExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SetItem()�@�@//�A�C�e���p�l�������ɏ������Ă���A�C�e���̎�ޕ��{�^�������
    {
        ClearItemButtonEvent();
        var itemList = playerDatabase.haveItems;
        for (int i = 0; i < itemList.Count; i++)
        {
            SetItemButton(itemButtons[i], itemList[i]);
        }
    }

    public void SetItemInfo(Item item)�@�@//�N���b�N���ꂽ�A�C�e���̏ڍ׏����A�C�e���p�l���E���̃p�l���ɕ\��������
    {
        itemImage.sprite = Resources.Load<Sprite>($"ItemSprite/{item.itemName}");
        itemInfoText.text = item.itemInfo;
        itemCountText.text = item.count + "����";
    }

    private void ClearItemButtonEvent()�@�@//�A�C�e���{�^���̐ݒ�O�Ɋe�{�^��������������
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].onClick.RemoveAllListeners();
            itemButtons[i].enabled = false;
            itemButtons[i].image.color = new Color(0, 0, 0, 255);
            itemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    private void SetItemButton(Button button, Item item)�@�@//�e�A�C�e���{�^���ɏ��𒍓�����
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

    public void FadeOutPanel(int num)�@�@//�w��b���Ńt�F�[�h�A�E�g�����A������A���t�@�l��߂�
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

    public void ShowMessagePanel(string message, float num)�@�@//�����̕��������ʏ�ɁA�w��b���ԕ\��������
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

    public static void CanShowMenu()�@�@//���݃��j���[��ʂ��\���ł��邩���ȈՓI�ɊǗ�
    {
        if (canShowMenu) canShowMenu = false;
        else canShowMenu = true;
    }

    private void SetTime(float num)�@�@//Time.Scale = 0 �̍ۂɂ��b���w��C�x���g���N�������߂̃��\�b�h
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
