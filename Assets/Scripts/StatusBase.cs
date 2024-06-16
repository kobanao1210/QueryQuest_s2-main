using System;
using UnityEngine;

public abstract class StatusBase : MonoBehaviour
{
    [NonSerialized] public int LV;               //PlayerのLV
    [NonSerialized] public int LifeMax;               //Lifeの最大値
    [NonSerialized] public int Life;                 //残りライフ
    [NonSerialized] public int Attack;                //攻撃力
    [NonSerialized] public int Defense;                  //防御力

    [NonSerialized] public State stateNow = State.Normal;                 //現在の状態
    [NonSerialized] public ExistField existField = ExistField.Battle;         //存在するフィールド（MAPかバトルフィールドか）

    private HPBarController hPBarController;  
    public enum State
    {
        Normal, Attack, Guard, Down, Die
    }

    public enum ExistField
    {
        Map, Battle
    }
    public virtual void OnDie(StatusBase status)
    {
        status.stateNow = State.Die;
        status.GetComponent<Animator>().SetTrigger("Die");
    }

    public virtual void OnDown(StatusBase status)
    {
        status.GetComponent<PlayerStatus>().GuardFinished();
        status.stateNow = State.Down;
        status.GetComponent<Animator>().SetTrigger("Down");
        status.transform.Translate(0, 0, -1, Space.Self);
        status.Invoke(nameof(OnNormal), 1f);
    }
    public virtual void OnGuard()
    {
        stateNow = State.Guard;
        GetComponent<Animator>().SetFloat("Guard", 1.0f);
    }

    public virtual void OnAttack()
    {
        stateNow = State.Attack;
        GetComponentInChildren<Animator>().SetTrigger("Attack");
    }

    public virtual void OnNormal()
    {
        stateNow = State.Normal;
    }

    public virtual void OnDamage(StatusBase status)
    {
        if (status.stateNow == State.Die) return;  //死んでいたらダメージ処理は行わない

        if (status.stateNow == State.Guard)
        {
            var guardPointDistanceV = transform.position - status.gameObject.transform.Find("GuardPoint").transform.position;
            var playerDistanceV = transform.position - status.transform.position;
            var guardPointDistance = MathF.Abs(guardPointDistanceV.x) + MathF.Abs(guardPointDistanceV.z);
            var playerDistance = MathF.Abs(playerDistanceV.x) + MathF.Abs(playerDistanceV.z);
            var guard = GuardCheck(status, guardPointDistance <= playerDistance);
            if (status.gameObject.tag == "Player" && !guard) OnDown(status);
        }
        else
        {
            var guard= GuardCheck(status, false);
            if (status.gameObject.tag == "Player" && !guard) OnDown(status);
        }

        hPBarController = GetHPBar(status);
        hPBarController.RefreshHPBar();

        if (status.Life > 0) return;  //ライフチェック
        OnDie(status);
        CheckDieObject(status);
    }
    private bool GuardCheck(StatusBase status,bool checkGuard)
    {
        var num=checkGuard?  2: 1;
        var se = checkGuard ? "guard" : "damage";
        var damage = Attack / num - status.Defense;  //とりあえずダメージ半減
        if (damage < 0) damage = 0; 
        status.Life -= damage;
        if(status.GetComponent<PlayerStatus>()) PlayerDatabase.Instance.playerStatusData.Life -= damage;  //とりあえず
        AudioManager.Instance.Play(se, AudioManager.EclipType.SE);
        return checkGuard;
    }

    private void CheckDieObject(StatusBase status)  //死んだオブジェクトによって処理を分岐
    {
        var battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

        if (status.gameObject.tag == "Player")
        {
            OnDie(status);
            Destroy(gameObject);
            battleManager.PlayerLose();
        }
        else
        {
            battleManager.PlayerWin(status.gameObject);
            if (EventDatabase.Instance.isQuest) Destroy(status.gameObject);
            else DeleteEnemyData(status);
        }
    }

    private void DeleteEnemyData(StatusBase status)
    {
        var instance = EventDatabase.Instance;  //消したいオブジェクトのListでのindexを取得
        var index = instance.mapInEnemies.FindIndex(x => x.enemyName == EventDatabase.Instance.battleObjectName);
        instance.mapInEnemies.RemoveAt(index);
        Destroy(status.gameObject);
    }

    private HPBarController GetHPBar(StatusBase status)
    {
        hPBarController = status.GetComponentInChildren<HPBarController>();
        if (hPBarController == null) hPBarController =
                GameObject.Find("PlayerHPBar").GetComponent<HPBarController>();
        return hPBarController;
    }
}
