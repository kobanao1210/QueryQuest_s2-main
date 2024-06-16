using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InventryManager
{
    public static List<Item> itemList = new List<Item>();  //アイテムの効果などを参照するためのすべてのアイテムのリスト
    public static bool UseMoney(int price)   //所持金が足りるかの判定メソッド
    {
        var money = PlayerDatabase.Instance.playerStatusData.money;
        if (money > price) return true;
        else
        {
            var UIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
            var audioManager = AudioManager.Instance;
            UIManager.ShowMessagePanel("所持金が足りません", 1.5f);
            audioManager.Play("bad", AudioManager.EclipType.SE);
            return false;
        }
    }
    public static void GetItem(Item item, int num = 1)　　//アイテムを取得した際に、すでに所持している種類かを判定
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
    public static void GetItem(List<Item> items, int num = 1)　　//上のメソッドの、複数種類同時処理版
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

    public static void UseItem(Item item, int num = 1)   //アイテムを使用した際の効果判別
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        var usedItem = haveItems.Find(x => x.itemName == item.itemName);
        var isUse = CheckItemType(usedItem);
        if (isUse) LostItem(item, num);
    }

    public static void DisPoseItem(Item item, int num = 1)  //アイテムを捨てる際の処理
    {
        var gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        gameUIManager.ShowMessagePanel($"{item.itemName}を{num}個失った", 1.5f);
        LostItem(item, num);
    }
    public static void LostItem(Item item, int num = 1)　　//アイテムの数が0になる場合は、所持アイテムのリストから削除
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

    private static bool CheckItemType(Item item)　　//使用されたアイテムの効果タイプによって分岐させる
    {
        var gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        if (item.type == "None")
        {
            gameUIManager.ShowMessagePanel($"{item.itemName}を使ったが何も起こらない", 1.5f);
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

    private static string AddStatus(Item item)　　//changeStatusの文字列によって、変化させるステータスを分岐
    {
        var playerStatus = PlayerDatabase.Instance.playerStatusData;
        var changeText = "";
        if (item.changeStatus == "Life") 
        {
            var healValue = playerStatus.LifeMax - playerStatus.Life;
            if(healValue > item.value) healValue = item.value;
            if (healValue == 0) changeText = "これ以上回復できない";
            else 
            {
                playerStatus.Life += item.value;
                changeText = $"体力が{item.value}回復した";
            }
            if (playerStatus.Life > playerStatus.LifeMax) playerStatus.Life = playerStatus.LifeMax;
            ChangeHPBar();
            //ヒールエフェクト？
            //音も?
        }
        else if (item.changeStatus == "LifeMax")
        {
            playerStatus.LifeMax += item.value;
            changeText = $"体力最大値が{item.value}上がった!!";
        }
        else if (item.changeStatus == "Attack")
        {
            playerStatus.Attack += item.value;
            changeText = $"攻撃最大値が{item.value}上がった!!";
        }
        else if (item.changeStatus == "Defense")
        {
            playerStatus.Defense += item.value;
            changeText = $"防御最大値が{item.value}上がった!!";
        }
        var activeScene = SceneManager.GetActiveScene().name;
        if (activeScene.Contains("Battle"))
        {
            var battlePlayerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
            battlePlayerStatus.SetStatus(playerStatus);
        }
        return changeText;
    }

    private static void ChangeHPBar()  //戦闘中であればHPバーを変動させる
    {
        if (GameObject.FindGameObjectWithTag("Player")== null) return;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        if (player.existField == StatusBase.ExistField.Map) return;
        GameObject.Find("PlayerHPBar").GetComponent<HPBarController>().RefreshHPBar();
    }
}
