using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class JsonManager
{

    public static string playerDataPath = Application.persistentDataPath + "/playerSave.json";
    public static string spritString = "///";

    public static void SavePlayerData(PlayerDatabase playerDatabase)�@�@//���݂̃v���C���[�̃X�e�[�^�X�E�����A�C�e����json�`���ŕۑ�
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

    public static void LoadPlayerData()�@�@//��L�ŕۑ�����json�t�@�C����ǂݍ��݂Ŋe�f�[�^�x�[�X�Ɋi�[
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
            gameUIManager.ShowMessagePanel("�Z�[�u�f�[�^�ɕs������邽�ߍŏ�����n�߂܂�", 2f);
            PlayerDatabase.Instance.playerStatusData.ResetPlayerStatusData();
            Util.LoadMapScene();
        }

    }

    private static void LoadPlayerStatusData(string jsonText)
    {
         var playerStatusData = JsonUtility.FromJson<PlayerStatusData>(jsonText);
        PlayerDatabase.Instance.playerStatusData.SetPlayerStatusData(playerStatusData);
    }

    private static void LoadPlayerHaveItems(string jsonText)�@�@//jsonUtility�ł͔z������̂܂܈����Ȃ�����wrap����
    {
        var haveItems = PlayerDatabase.Instance.haveItems;
        ItemList itemListWrapper = JsonUtility.FromJson<ItemList>(jsonText);

        if (itemListWrapper != null && itemListWrapper.items != null)
        {
            haveItems.Clear(); // ���X�g���N���A���Ă���ǉ�����
            haveItems.AddRange(itemListWrapper.items);
        }
    }
    public static void LoadItemData()�@//json�ŕۑ����ꂽ�S�A�C�e���̏���ǂݏo��staticClass�ɕۑ�����
    {
        TextAsset itemDataJson = Resources.Load<TextAsset>("JsonFile/item");
        string json = "{\"items\":" + itemDataJson.text + "}";
        ItemList itemListWrapper = JsonUtility.FromJson<ItemList>(json);
        InventryManager.itemList = itemListWrapper.items.ToList();
    }
    public static void LoadQuestData()�@//�チ�\�b�h�̃N�G�X�g��
    {
        TextAsset questDataJson = Resources.Load<TextAsset>("JsonFile/quest");
        string json = "{\"quests\":" + questDataJson.text + "}";
        QuestList questListWrapper = JsonUtility.FromJson<QuestList>(json);
        QuestManager.allQuests = questListWrapper.quests.ToList();
    }
}
//jsonUtility�Ŕz��E���X�g���������߂̐�p�N���X

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