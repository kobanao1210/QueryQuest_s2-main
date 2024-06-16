using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : StatusBase
{
    [SerializeField] private float guardTime = 2.5f;

    private GameObject uiCanvas;
    private MapManager mapManager;
    private SelectButton selectButton;

    private UnityAction action;
    private void Start()
    {
        var instance = PlayerDatabase.Instance;
        SetStatus(instance.playerStatusData);
        uiCanvas = GameObject.Find("UICanvas");
        selectButton = uiCanvas.transform.Find("SelectPanel").GetComponent<SelectButton>();
    }

    private void OnTriggerEnter(Collider other)  //�ړG����
    {
        if (other.tag != "Enemy" || existField != ExistField.Map) return;

        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.ContactEnemy(other);
    }

    private void OnCollisionEnter(Collision collision)  //Area�^�O�̂����I�u�W�F�N�g�ɓ��������ۂ̊e�폈��
    {
        if (collision.gameObject.tag == "Untagged") return;

        if (EventDatabase.Instance.isQuest && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(() => QuestManager.QuestEscape(EventDatabase.Instance.acceptQuest));
            selectButton.ShowSelectPanel("�N�G�X�g��j�����Ē��ɖ߂�H", action);
        }
        else if (existField == ExistField.Map && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(Util.LoadTownScene);
            selectButton.ShowSelectPanel("���ɖ߂�?", action);
        }
        else if (existField == ExistField.Battle && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(PlayerEscape);
            selectButton.ShowSelectPanel("�퓬���痣�E����?", action);
        }
    }

    public void SetStatus(PlayerStatusData playerStatusData)  //�퓬�J�n���̃X�e�[�^�X
    {
        LV = playerStatusData.LV;
        LifeMax = playerStatusData.LifeMax;
        Life = playerStatusData.Life;
        Attack = playerStatusData.Attack;
        Defense = playerStatusData.Defense;
    }
    private void PlayerEscape()�@�@//MAP�V�[���ɑJ�ځA����Ă��������X�^�[���o�g���V�[���E�}�b�v���̏�񂩂�폜����
    {
        var instance = EventDatabase.Instance;  //���������I�u�W�F�N�g��List�ł�index���擾
        var index = instance.mapInEnemies.FindIndex(x => x.enemyName == EventDatabase.Instance.battleObjectName);
        instance.mapInEnemies.RemoveAt(index);
        Destroy(GameObject.Find(instance.battleObjectName));
        Util.LoadMapScene();
    }

    public override void OnGuard()
    {
        base.OnGuard();
        Invoke(nameof(GuardFinished), guardTime);
    }

    public void GuardFinished()
    {
        GetComponent<Animator>().SetFloat("Guard", 0);
        CancelInvoke();
        OnNormal();
    }
}
