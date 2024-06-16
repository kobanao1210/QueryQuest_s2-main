using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapUIManager mapUIManager;
    [SerializeField] private GameObject playerPrefab;

    private GameObject player;
    private EventDatabase instance;

    private void Start()
    {
        instance = EventDatabase.Instance;
        GeneratePlayer();
        GenerateEnemy();
    }

    private void GeneratePlayer()  //�o�g���V�[���J�ڎ��Ƀv���C���[�𐶐�
    {
        player = Instantiate(playerPrefab);
        player.transform.position = instance.mapInPlayerPosition;
        player.transform.localEulerAngles = instance.mapInPlayerRotation;
        player.GetComponent<StatusBase>().existField = StatusBase.ExistField.Map;
    }
    private void GenerateEnemy()  //�}�b�v�V�[����������eventDatabase��mapInEnemies���Q�Ƃ��ēG�𐶐�����
    {
        var Enemies = instance.mapInEnemies;
        for (int i = 0; i < Enemies.Count; i++)
        {
            var enemyPrefab = Enemies[i].enemyPrefab;
            var enemy = Instantiate(enemyPrefab, Enemies[i].enemyPosition, Quaternion.identity);
            enemy.name = Enemies[i].enemyName;
            enemy.transform.LookAt(player.transform);

            var enemyStatus = enemy.GetComponent<StatusBase>();
            enemyStatus.existField = StatusBase.ExistField.Map;
            enemy.GetComponent<EnemyMove>().CheckMoveType(enemyStatus.existField);
        }
    }

    public void ContactEnemy(Collider other)   //Map�V�[���ŐڐG�����G�I�u�W�F�N�g�̏���eventDatabase�Ɋi�[����
    {
        DontDestroyOnLoad(other);
        instance.battleObjectName = other.gameObject.name;
        instance.mapInPlayerPosition = player.transform.position;
        instance.mapInPlayerRotation = player.transform.localEulerAngles;
        var enemyStatus = other.GetComponent<EnemyStatus>();
        mapUIManager.ShowMapPanel(enemyStatus.enemyName);
    }
}
