using System.Collections.Generic;

public static class QuestManager
{
    public static List<Quest> allQuests = new List<Quest>();　　//jsonファイルから変換した、全クエストの情報リスト

    public static bool isShowQuestResult = false;　　//TownSceneに戻った際に、クエスト結果画面を表示するかどうか
    public static bool isQuestResult = false;　　　　//クエストの成否が確定したかどうか
    public static void QuestSucces(Quest quest)　　//クエスト成功の処理
    {
        isShowQuestResult = true;
        isQuestResult = true;
        EventDatabase.Instance.isQuest = false;
        var playerStatusData = PlayerDatabase.Instance.playerStatusData;
        var audioManager = AudioManager.Instance;
        GetRankPoint(quest.rewardrankpoint, playerStatusData);
        var getItem = InventryManager.itemList.Find(x => x.itemName == quest.rewardItem);
        InventryManager.GetItem(getItem, quest.rewardValue);
        audioManager.Play("lvup", AudioManager.EclipType.SE);
    }

    public static void QuestFailed(Quest quest)　　//クエスト失敗の処理
    {
        isShowQuestResult = true;
        isQuestResult = false;
        EventDatabase.Instance.isQuest = false;
        var playerStatusData = PlayerDatabase.Instance.playerStatusData;
        var audioManager = AudioManager.Instance;
        GetRankPoint(-quest.rewardrankpoint / 2, playerStatusData);
        audioManager.Play("bad", AudioManager.EclipType.SE);
    }

    public static void QuestEscape(Quest quest)　　//討伐クエスト受注中に戦闘から離脱した場合の処理
    {
        Util.LoadTownScene();
        QuestFailed(quest);
    }

    private static void GetRankPoint(int rankPoint, PlayerStatusData playerStatusData)
    {
        playerStatusData.rankpoint += rankPoint;
        if (playerStatusData.rankpoint < 0) playerStatusData.rankpoint = 0;
    }
}
