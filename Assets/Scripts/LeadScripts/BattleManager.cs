using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Vector3 playerPosition = new Vector3(0, 0, 0);  //バトルシーンでのplayerの初期位置
    [SerializeField] private Vector3 enemyPosition = new Vector3(0, 0, 10);   //バトルシーンでのenemyの初期位置

    [SerializeField] private BattleUIManager battleUIManager;
    [SerializeField] private GameObject playerPrefab;

    private EventDatabase eventDatabase;
    private PlayerDatabase playerDatabase;
    private AudioManager audioManager;
    private NavMeshAgent enemyAgent;

    private void Start()
    {
        eventDatabase = EventDatabase.Instance;
        playerDatabase = PlayerDatabase.Instance;
        audioManager = AudioManager.Instance;
        SetPlayer();
        if (eventDatabase.isQuest) GenerateEnemy();
        else SetEnemy();
    }

    private void SetPlayer()  //バトルシーン遷移時にプレイヤーを設定
    {
        var player = Instantiate(playerPrefab);
        player.transform.position = playerPosition;
        player.transform.LookAt(enemyPosition);
    }

    private void SetEnemy()  //バトルシーン遷移時にエネミーを設定
    {
        var enemy = GameObject.Find(eventDatabase.battleObjectName);
        enemyAgent = enemy.GetComponent<NavMeshAgent>();
        ChangeExistField(enemy);
        SetEnemyPosition(enemy);
    }

    private void GenerateEnemy()　　//討伐クエスト時に新たに敵を生成
    {
        var enemy = Instantiate(eventDatabase.questEnemyPrefab);
        enemyAgent = enemy.GetComponent<NavMeshAgent>();
        ChangeExistField(enemy);
        SetEnemyPosition(enemy);
    }

    private void ChangeExistField(GameObject enemy)　　//存在するフィールドを変更
    {
        var enemyStatus = enemy.GetComponent<StatusBase>();
        enemyStatus.existField = StatusBase.ExistField.Battle;  
        enemy.GetComponent<EnemyMove>().CheckMoveType(enemyStatus.existField);
    }

    private void SetEnemyPosition(GameObject enemy)
    {
        enemyAgent.enabled = false;                      //navMeshAgentをいったんFalseに変更
        enemy.transform.transform.position = enemyPosition;
        enemy.transform.LookAt(playerPosition);
        enemyAgent.enabled = true;
    }

    private void DropCheck(List<Item> getItems)
    {
        battleUIManager.SetItemsImage(getItems);
        if (getItems.Count ==0) return;
        InventryManager.GetItem(getItems);
    }

    public void PlayerWin(GameObject enemy)
    {
        var enemyStatus = enemy.GetComponent<EnemyStatus>();
        audioManager.ToggleBGM();
        audioManager.Play("win", AudioManager.EclipType.SE);
        battleUIManager.ShowBattlePanel(enemyStatus);
        LevelManager.AddExp(playerDatabase.playerStatusData, enemyStatus);
        var getItems = enemyStatus.DropItem();
        DropCheck(getItems);
        playerDatabase.playerStatusData.money += enemyStatus.haveMoney;  //所持金追加
    }
    public void PlayerLose()
    {
        audioManager.ToggleBGM();
        Util.PauseGame();
        audioManager.Play("gameover", AudioManager.EclipType.SE);  //音追加
        battleUIManager.ShowGameOverPanel();
        if (eventDatabase.isQuest) eventDatabase.isQuest = false;
    }

}
