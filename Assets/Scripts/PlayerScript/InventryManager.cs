using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InventryManager
{
    public static List<Item> itemList = new List<Item>();  //�A�C�e���̌��ʂȂǂ��Q�Ƃ��邽�߂̂��ׂẴA�C�e���̃��X�g
    public static bool UseMoney(int price)   //������������邩�̔��胁�\�b�h
    {
        var money = PlayerDatabase.Instance.playerStatusData.money;
        if (money > price) return true;
        else
        {
            var UIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
            var audioManager = AudioManager.Instance;
            UIManager.ShowMessagePanel("������������܂���", 1.5f);
            audioManager.Play("bad", AudioManager.EclipType.SE);
            return false;
        }
    }
    public static void GetItem(Item item, int num = 1)�@�@//�A�C�e�����擾�����ۂɁA���łɏ������Ă����ނ��𔻒�
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        if (haveItems.Any(x => x.itemName == item.itemName))
            haveItems.Find(x => x.itemName == item.itemName).count++;
        else
        {
            haveItems.Add(item);
            haveItems.Find(x => x.itemName == item.itemName).count++;
        }
    }
    public static void GetItem(List<Item> items, int num = 1)�@�@//��̃��\�b�h�́A������ޓ���������
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        for (int i = 0; items.Count > i; i++)
        {
            if (haveItems.Any(x => x.itemName == items[i].itemName))
                haveItems.Find(x => x.itemName == items[i].itemName).count++;
            else
            {
                haveItems.Add(items[i]);
                haveItems.Find(x => x.itemName == items[i].itemName).count++;
            }
        }
    }

    public static void UseItem(Item item, int num = 1)   //�A�C�e�����g�p�����ۂ̌��ʔ���
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        var usedItem = haveItems.Find(x => x.itemName == item.itemName);
        var isUse = CheckItemType(usedItem);
        if (isUse) LostItem(item, num);
    }

    public static void DisPoseItem(Item item, int num = 1)  //�A�C�e�����̂Ă�ۂ̏���
    {
        var gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        gameUIManager.ShowMessagePanel($"{item.itemName}��{num}������", 1.5f);
        LostItem(item, num);
    }
    public static void LostItem(Item item, int num = 1)�@�@//�A�C�e���̐���0�ɂȂ�ꍇ�́A�����A�C�e���̃��X�g����폜
    {
        var gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        var haveItems = PlayerDatabase.Instance.haveItems;
        var lostItem = haveItems.Find(x => x.itemName == item.itemName);
        lostItem.count -= num;
        if (lostItem.count == 0)
        {
            haveItems.Remove(lostItem);
            gameUIManager.SetItem();
        }
    }

    private static bool CheckItemType(Item item)�@�@//�g�p���ꂽ�A�C�e���̌��ʃ^�C�v�ɂ���ĕ��򂳂���
    {
        var gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        if (item.type == "None")
        {
            gameUIManager.ShowMessagePanel($"{item.itemName}���g�����������N����Ȃ�", 1.5f);
            return false;
        }
        else
        {
            var text = AddStatus(item);
            gameUIManager.ShowMessagePanel(text, 1.5f);
            gameUIManager.SetStatus();
            return true;
        }
    }

    private static string AddStatus(Item item)�@�@//changeStatus�̕�����ɂ���āA�ω�������X�e�[�^�X�𕪊�
    {
        var playerStatus = PlayerDatabase.Instance.playerStatusData;
        var changeText = "";
        if (item.changeStatus == "Life") 
        {
            var healValue = playerStatus.LifeMax - playerStatus.Life;
            if(healValue > item.value) healValue = item.value;
            if (healValue == 0) changeText = "����ȏ�񕜂ł��Ȃ�";
            else 
            {
                playerStatus.Life += item.value;
                changeText = $"�̗͂�{item.value}�񕜂���";
            }
            if (playerStatus.Life > playerStatus.LifeMax) playerStatus.Life = playerStatus.LifeMax;
            ChangeHPBar();
            //�q�[���G�t�F�N�g�H
            //����?
        }
        else if (item.changeStatus == "LifeMax")
        {
            playerStatus.LifeMax += item.value;
            changeText = $"�̗͍ő�l��{item.value}�オ����!!";
        }
        else if (item.changeStatus == "Attack")
        {
            playerStatus.Attack += item.value;
            changeText = $"�U���ő�l��{item.value}�オ����!!";
        }
        else if (item.changeStatus == "Defense")
        {
            playerStatus.Defense += item.value;
            changeText = $"�h��ő�l��{item.value}�オ����!!";
        }
        var activeScene = SceneManager.GetActiveScene().name;
        if (activeScene.Contains("Battle"))
        {
            var battlePlayerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
            battlePlayerStatus.SetStatus(playerStatus);
        }
        return changeText;
    }

    private static void ChangeHPBar()  //�퓬���ł����HP�o�[��ϓ�������
    {
        if (GameObject.FindGameObjectWithTag("Player")== null) return;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        if (player.existField == StatusBase.ExistField.Map) return;
        GameObject.Find("PlayerHPBar").GetComponent<HPBarController>().RefreshHPBar();
    }
}
