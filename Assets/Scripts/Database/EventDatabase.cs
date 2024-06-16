using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDatabase : MonoBehaviour
{
    private static EventDatabase instance;

    [NonSerialized] public List<MapInEnemy> mapInEnemies = new List<MapInEnemy>();  //マップに残っている敵の位置と種類

    [NonSerialized] public string battleObjectName;  //バトルシーンに出現させる敵のObjectName
    [NonSerialized] public Vector3 mapInPlayerPosition;  //接敵した際にplayerがいた座標
    [NonSerialized] public Vector3 mapInPlayerRotation;  //接敵した際のplayerの向き

    [NonSerialized] public bool isQuest = false;  //討伐クエストとして、バトルシーンに遷移したかの確認用
    [NonSerialized] public Quest acceptQuest;　　 //現在受注中のクエスト
    [NonSerialized] public GameObject questEnemyPrefab;  //現在受注中のクエストの標的のプレファブ

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
    public static EventDatabase Instance
    {
        get { return instance; }
    }
}

public class MapInEnemy
{
    public GameObject enemyPrefab;
    public string enemyName;
    public Vector3 enemyPosition;

    public MapInEnemy(GameObject prefab, string enemyname, Vector3 position)
    {
        enemyPrefab = prefab;
        enemyName = enemyname;
        enemyPosition = position;
    }
}
