using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class JsonManager
{

    public static string playerDataPath = Application.persistentDataPath + "/playerSave.json";
    public static string spritString = "///";

    public static void SavePlayerData(PlayerDatabase playerDatabase)　　//現在のプレイヤーのステータス・所持アイテムをjson形式で保存
    {
        var playerStatus = playerDatabase.playerStatusData;
        var haveItems = playerDatabase.haveItems;
        var jsonText = JsonUtility.ToJson(playerStatus);
        var json = new ItemList();
        json.items = haveItems.ToArray();
        var jsonItem = JsonUtility.ToJson(json);
        jsonText += (spritString  + jsonItem);
        File.WriteAllText(playerDataPath, jsonText);
    }

    public static void LoadPlayerData()　　//上記で保存したjsonファイルを読み込みで各データベースに格納
    {
        try
        {
            var jsonText = File.ReadAllText(playerDataPath);
            if (jsonText == null) return;
            var jsonTexts = jsonText.Split(spritString);
            LoadPlayerStatusData(jsonTexts[0]);
            LoadPlayerHaveItems(jsonTexts[1]);
            Util.LoadTownScene();
        }   
        catch
        {
            var gameUIManager= GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
            gameUIManager.ShowMessagePanel("セーブデータに不具合があるため最初から始めます", 2f);
            PlayerDatabase.Instance.playerStatusData.ResetPlayerStatusData();
            Util.LoadMapScene();
        }

    }

    private static void LoadPlayerStatusData(string jsonText)
    {
         var playerStatusData = JsonUtility.FromJson<PlayerStatusData>(jsonText);
        PlayerDatabase.Instance.playerStatusData.SetPlayerStatusData(playerStatusData);
    }

    private static void LoadPlayerHaveItems(string jsonText)　　//jsonUtilityでは配列をそのまま扱えないためwrapする
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        ItemList itemListWrapper = JsonUtility.FromJson<ItemList>(jsonText);

        if (itemListWrapper != null && itemListWrapper.items != null)
        {
            haveItems.Clear(); // リストをクリアしてから追加する
            haveItems.AddRange(itemListWrapper.items);
        }
    }
    public static void LoadItemData()　//jsonで保存された全アイテムの情報を読み出しstaticClassに保存する
    {
        TextAsset itemDataJson = Resources.Load<TextAsset>("JsonFile/item");
        string json = "{\"items\":" + itemDataJson.text + "}";
        ItemList itemListWrapper = JsonUtility.FromJson<ItemList>(json);
        InventryManager.itemList = itemListWrapper.items.ToList();
    }
    public static void LoadQuestData()　//上メソッドのクエスト版
    {
        TextAsset questDataJson = Resources.Load<TextAsset>("JsonFile/quest");
        string json = "{\"quests\":" + questDataJson.text + "}";
        QuestList questListWrapper = JsonUtility.FromJson<QuestList>(json);
        QuestManager.allQuests = questListWrapper.quests.ToList();
    }
}
//jsonUtilityで配列・リストを扱うための専用クラス

[Serializable]
public class ItemList
{
    public Item[] items;
}

[Serializable]
public class QuestList
{
    public Quest[] quests;
}