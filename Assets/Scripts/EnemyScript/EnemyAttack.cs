using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 1.0f;   //�U���\�܂ł̎���
    [SerializeField] private Animator animator;

    private EnemyStatus status;�@�@�@�@�@�@�@�@�@�@�@�@�@�@ //������status�X�N���v�g
    private PlayerStatus playerStatus;                      //�U���Ώۂ�status�X�N���v�g
    private bool playerInRange;�@�@�@�@�@�@�@�@�@�@�@�@�@�@ //�U���͈͓��ɑΏۂ����݂��邩�ǂ���
    private void Start()
    {
        status = GetComponentInParent<EnemyStatus>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)  //�˒��͈͂ɓ����Ă�����t���O�I���ɂ��đ���̏����擾
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

    private void OnTriggerExit(Collider other)  //�˒��͈͂���ł���t���O���I�t
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

    private void ChangeNormal()   //Invoke�g�����߂Ɏ����Ă����@���P�ł�����
    {
        status.OnNormal();
    }

    public void CheckHit()
    {
        if (!playerInRange) return;
        status.OnDamage(playerStatus);
    }
}
