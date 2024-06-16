public class LevelManager
{
    public static bool canLevelUp;  //LV�A�b�v�ł��邩�ǂ���
    public static PlayerStatusData beforeStatusData;  //LV�A�b�v�O�̃X�e�[�^�X
    public static PlayerStatusData afterStatusData;   //LV�A�b�v��̃X�e�[�^�X

    public static void AddExp(PlayerStatusData playerStatusData, EnemyStatus enemyStatus)
    {
        playerStatusData.Exp += enemyStatus.haveExp;
        if (playerStatusData.Exp > playerStatusData.ExpMax)
        {
            beforeStatusData = CashPlayerStatusData(playerStatusData);
            LevelUp(playerStatusData);
        }
        afterStatusData = playerStatusData;

    }

    private static PlayerStatusData CashPlayerStatusData(PlayerStatusData playerStatusData)  //�Q�ƌ^�𖳗�����l�^�ɂ��ăL���b�V��
    {
        var statusData = new PlayerStatusData();
        statusData.LV = playerStatusData.LV;
        statusData.LifeMax = playerStatusData.LifeMax;
        statusData.Life = playerStatusData.Life;
        statusData.Attack = playerStatusData.Attack;
        statusData.Defense = playerStatusData.Defense;
        statusData.ExpMax = playerStatusData.ExpMax;
        statusData.Exp = playerStatusData.Exp;
        return statusData;
    }

    private static void LevelUp(PlayerStatusData playerStatusData)�@�@//Player�̎���EXP��EXPMAX�ȉ��ɂȂ�܂�LV�A�b�v����
    {
        canLevelUp = true;
        while (playerStatusData.Exp > playerStatusData.ExpMax)
        {
            playerStatusData.LV++;
            playerStatusData.Exp -= playerStatusData.ExpMax;
            StatusUp(playerStatusData);
        }
    }

    private static void StatusUp(PlayerStatusData playerStatusData)  //LV�A�b�v�ɔ����X�e�[�^�X�㏸����
    {
        if (playerStatusData.LV % 3 == 0) playerStatusData.Attack++;
        if (playerStatusData.LV % 5 == 0) playerStatusData.Defense++;
        playerStatusData.LifeMax += playerStatusData.LV / 2;
        playerStatusData.ExpMax += playerStatusData.LV * 5;  //��΂������璲��
    }
}
