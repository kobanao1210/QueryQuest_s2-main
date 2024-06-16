public class LevelManager
{
    public static bool canLevelUp;  //LVアップできるかどうか
    public static PlayerStatusData beforeStatusData;  //LVアップ前のステータス
    public static PlayerStatusData afterStatusData;   //LVアップ後のステータス

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

    private static PlayerStatusData CashPlayerStatusData(PlayerStatusData playerStatusData)  //参照型を無理くり値型にしてキャッシュ
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

    private static void LevelUp(PlayerStatusData playerStatusData)　　//Playerの持つEXPがEXPMAX以下になるまでLVアップ処理
    {
        canLevelUp = true;
        while (playerStatusData.Exp > playerStatusData.ExpMax)
        {
            playerStatusData.LV++;
            playerStatusData.Exp -= playerStatusData.ExpMax;
            StatusUp(playerStatusData);
        }
    }

    private static void StatusUp(PlayerStatusData playerStatusData)  //LVアップに伴うステータス上昇処理
    {
        if (playerStatusData.LV % 3 == 0) playerStatusData.Attack++;
        if (playerStatusData.LV % 5 == 0) playerStatusData.Defense++;
        playerStatusData.LifeMax += playerStatusData.LV / 2;
        playerStatusData.ExpMax += playerStatusData.LV * 5;  //やばかったら調整
    }
}
