using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    [SerializeField] List<ItemDrop> dropItems = new List<ItemDrop>();  //ドロップするアイテムのリスト

    //インスペクターで各パラメーターを調整できるようにしただけ
    [SerializeField] private int vLifeMax;
    [SerializeField] private int vLife;
    [SerializeField] private int vAttack;
    [SerializeField] private int vDefense;

    public string enemyName;
    public int haveExp;
    public int haveMoney;
    private const int remitDropCount = 2;

    private void Start()
    {
        LifeMax = vLifeMax;
        Life = vLife;
        Attack = vAttack;
        Defense = vDefense;
    }

    public List<Item>  DropItem()
    {
        var num = UnityEngine.Random.Range(0, 101);
        var getItems = new List<Item>();
        var allItemlist = InventryManager.itemList;
        for (int i = 0; dropItems.Count > i;i++)
        {
            if (dropItems[i].dropRatio > num)
            {
                var getItem= allItemlist.Find(x => x.number == dropItems[i].itemNumber);
                getItems.Add(getItem);
            }
            if(getItems.Count == remitDropCount) break;
        }
        return getItems;
    }

    [Serializable]
    public class ItemDrop
    {
        public int itemNumber;
        public int dropRatio;
    }
}
