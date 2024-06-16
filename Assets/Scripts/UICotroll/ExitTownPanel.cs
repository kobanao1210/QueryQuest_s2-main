using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExitTownPanel : MonoBehaviour
{
    [SerializeField] private List<Vector3> posionsInGlass = new List<Vector3>();  //�����G���A�̓G�������W
    [SerializeField] private List<SpwanEnemy> EnemiesInGlass = new List<SpwanEnemy>();  //�����G���A�̏o������G���X�g


    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private List<GameObject> ChoiceEnemies(int enemyCount, List<SpwanEnemy> EnemiesInField)  //ratio(�o�������̏��Ȃ����ɕ��ѕς����Ԃɔ���)
    {
        EnemiesInField.OrderBy(x => x.ratio);
        var returnList = new List<GameObject>();
        for (int i = 0; i < enemyCount; i++)
        {
            var num = UnityEngine.Random.Range(0, 101);  //inspecter��ratio���v100�ȏ�ɂ��Ȃ��悤�ɒ���
            int ratio = 0;
            for (int j = 0; j < EnemiesInField.Count; j++)   //List�̓����珇�� (ratio + �O���ratio ) >= num �����Ă͂܂���̂�������܂Ŕ���
            {
                ratio += EnemiesInField[j].ratio;
                if (ratio >= num)
                {
                    returnList.Add(EnemiesInField[j].enemyPrefab);
                    j = EnemiesInField.Count;
                    ratio = 0;
                }
            }
        }
        return returnList;
    }

    public void OnToGlass()  //�����փ{�^�������ꂽ��
    {
        EventDatabase.Instance.mapInEnemies = SetMapInEnemies(posionsInGlass, EnemiesInGlass);
        EventDatabase.Instance.mapInPlayerPosition = Vector3.zero;
        Util.LoadMapScene();
    }

    private List<MapInEnemy> SetMapInEnemies(List<Vector3> fieldInpositions, List<SpwanEnemy> fieldInEnemies)
    {
        var enemyCount = UnityEngine.Random.Range(4, 8);
        var positions = fieldInpositions.OrderBy(x => Guid.NewGuid()).Take(enemyCount).ToList();
        var prefabs = ChoiceEnemies(enemyCount, fieldInEnemies);

        var mapInEnemies = new List<MapInEnemy>();
        for (int i = 0; i < enemyCount; i++)
        {
            var mapInEnemy = new MapInEnemy(prefabs[i], $"{prefabs[i].name}{i}", positions[i]);
            mapInEnemies.Add(mapInEnemy);
        }
        return mapInEnemies;
    }

    [Serializable]
    public class SpwanEnemy
    {
        public int ratio;
        public GameObject enemyPrefab;
    }
}
