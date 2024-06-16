using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour
{
    private static PlayerDatabase instance;

    [Serialize] public List<Item> haveItems = new List<Item>();  //プレイヤーが所持しているアイテムのリスト
    public PlayerStatusData playerStatusData;　　        //プレイヤーの現在のステータス情報(PlayerStatusクラスとは連動していないので注意)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //外部からロード？
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public static PlayerDatabase Instance
    {
        get { return instance; }
    }

    public int[] SendArrayStatusData()
    {
        var statusDataArray = playerStatusData.ConvertIntArray();
        return statusDataArray;
    }
}

[Serializable]
public class PlayerStatusData
{
    public int LV = 1;
    public int LifeMax = 5;
    public int Life = 5;
    public int Attack = 2;
    public int Defense = 1;
    public int ExpMax = 10;
    public int Exp = 0;
    public int money = 0;
    public int rankpoint = 0;
    public int[] ConvertIntArray()
    {
        var array = new int[]
        {
            LV,
            LifeMax,
            Life,
            Attack,
            Defense,
            ExpMax,
            Exp,
            money,
            rankpoint
        };
        return array;
    }

    public void SetPlayerStatusData(PlayerStatusData playerStatus)
    {
        LV = playerStatus.LV;
        LifeMax = playerStatus.LifeMax;
        Life = playerStatus.Life;
        Attack = playerStatus.Attack;
        Defense = playerStatus.Defense;
        ExpMax = playerStatus.ExpMax;
        Exp = playerStatus.Exp;
        money = playerStatus.money;
        rankpoint = playerStatus.rankpoint;
    }

    public void ResetPlayerStatusData()
    {
        LV = 1;
        Life = 5;
        LifeMax = 5;
        Attack = 2;
        Defense = 1;
        ExpMax = 10;
        Exp = 0;
        money = 0;
        rankpoint = 0;
    }
}

