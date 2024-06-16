using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 1.0f;   //攻撃可能までの時間
    [SerializeField] private Animator animator;

    private EnemyStatus status;　　　　　　　　　　　　　　 //自分のstatusスクリプト
    private PlayerStatus playerStatus;                      //攻撃対象のstatusスクリプト
    private bool playerInRange;　　　　　　　　　　　　　　 //攻撃範囲内に対象が存在するかどうか
    private void Start()
    {
        status = GetComponentInParent<EnemyStatus>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)  //射程範囲に入ってきたらフラグオンにして相手の情報を取得
    {
        if (other.tag != "Player") return;
        playerInRange = true;
        playerStatus = other.GetComponent<PlayerStatus>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (status.stateNow == StatusBase.State.Normal)
        {
            AttackStart();
        }
    }

    private void OnTriggerExit(Collider other)  //射程範囲からでたらフラグをオフ
    {
        playerInRange = false;
    }

    private void AttackStart()
    {
        if (status.existField != StatusBase.ExistField.Battle) return;
        status.OnAttack();
        animator.speed = 0.5f;
    }

    public void AttackFinished()
    {
        if (status.stateNow != StatusBase.State.Attack) return;
        Invoke(nameof(ChangeNormal), attackCoolDown);
        animator.speed = 1f;
    }

    private void ChangeNormal()   //Invoke使うために持ってきた　改善できたら
    {
        status.OnNormal();
    }

    public void CheckHit()
    {
        if (!playerInRange) return;
        status.OnDamage(playerStatus);
    }
}
