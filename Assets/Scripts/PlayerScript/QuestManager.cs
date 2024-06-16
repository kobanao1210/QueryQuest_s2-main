using System.Collections.Generic;

public static class QuestManager
{
    public static List<Quest> allQuests = new List<Quest>();�@�@//json�t�@�C������ϊ������A�S�N�G�X�g�̏�񃊃X�g

    public static bool isShowQuestResult = false;�@�@//TownScene�ɖ߂����ۂɁA�N�G�X�g���ʉ�ʂ�\�����邩�ǂ���
    public static bool isQuestResult = false;�@�@�@�@//�N�G�X�g�̐��ۂ��m�肵�����ǂ���
    public static void QuestSucces(Quest quest)�@�@//�N�G�X�g�����̏���
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

    public static void QuestFailed(Quest quest)�@�@//�N�G�X�g���s�̏���
    {
        isShowQuestResult = true;
        isQuestResult = false;
        EventDatabase.Instance.isQuest = false;
        var playerStatusData = PlayerDatabase.Instance.playerStatusData;
        var audioManager = AudioManager.Instance;
        GetRankPoint(-quest.rewardrankpoint / 2, playerStatusData);
        audioManager.Play("bad", AudioManager.EclipType.SE);
    }

    public static void QuestEscape(Quest quest)�@�@//�����N�G�X�g�󒍒��ɐ퓬���痣�E�����ꍇ�̏���
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
