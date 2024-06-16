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

    private void OnTriggerEnter(Collider other)  //接敵判定
    {
        if (other.tag != "Enemy" || existField != ExistField.Map) return;

        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mapManager.ContactEnemy(other);
    }

    private void OnCollisionEnter(Collision collision)  //Areaタグのついたオブジェクトに当たった際の各種処理
    {
        if (collision.gameObject.tag == "Untagged") return;

        if (EventDatabase.Instance.isQuest && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(() => QuestManager.QuestEscape(EventDatabase.Instance.acceptQuest));
            selectButton.ShowSelectPanel("クエストを破棄して町に戻る？", action);
        }
        else if (existField == ExistField.Map && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(Util.LoadTownScene);
            selectButton.ShowSelectPanel("町に戻る?", action);
        }
        else if (existField == ExistField.Battle && collision.gameObject.tag == "Area")
        {
            Util.PauseGame();
            action = new UnityAction(PlayerEscape);
            selectButton.ShowSelectPanel("戦闘から離脱する?", action);
        }
    }

    public void SetStatus(PlayerStatusData playerStatusData)  //戦闘開始時のステータス
    {
        LV = playerStatusData.LV;
        LifeMax = playerStatusData.LifeMax;
        Life = playerStatusData.Life;
        Attack = playerStatusData.Attack;
        Defense = playerStatusData.Defense;
    }
    private void PlayerEscape()　　//MAPシーンに遷移、戦っていたモンスターをバトルシーン・マップ内の情報から削除する
    {
        var instance = EventDatabase.Instance;  //消したいオブジェクトのListでのindexを取得
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
