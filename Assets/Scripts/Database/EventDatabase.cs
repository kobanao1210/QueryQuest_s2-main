using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDatabase : MonoBehaviour
{
    private static EventDatabase instance;

    [NonSerialized] public List<MapInEnemy> mapInEnemies = new List<MapInEnemy>();  //�}�b�v�Ɏc���Ă���G�̈ʒu�Ǝ��

    [NonSerialized] public string battleObjectName;  //�o�g���V�[���ɏo��������G��ObjectName
    [NonSerialized] public Vector3 mapInPlayerPosition;  //�ړG�����ۂ�player���������W
    [NonSerialized] public Vector3 mapInPlayerRotation;  //�ړG�����ۂ�player�̌���

    [NonSerialized] public bool isQuest = false;  //�����N�G�X�g�Ƃ��āA�o�g���V�[���ɑJ�ڂ������̊m�F�p
    [NonSerialized] public Quest acceptQuest;�@�@ //���ݎ󒍒��̃N�G�X�g
    [NonSerialized] public GameObject questEnemyPrefab;  //���ݎ󒍒��̃N�G�X�g�̕W�I�̃v���t�@�u

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //�O�����烍�[�h�H
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
