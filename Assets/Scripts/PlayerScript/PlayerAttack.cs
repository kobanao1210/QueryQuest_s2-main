using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown = 0.5f;   //�U���\�܂ł̎���
    [SerializeField] private Animator animator;

    private StatusBase playerStatus;
    private StatusBase enemyStatus;
    private bool enemyInRange;

    private void Start()
    {
        playerStatus = GetComponentInParent<StatusBase>();
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy") return;
        enemyInRange = true;
        enemyStatus = other.GetComponent<StatusBase>();
    }

    private void OnTriggerExit(Collider other)
    {
        enemyInRange = false;
    }

    public void AttackStart()
    {
        playerStatus.OnAttack();
        animator.speed = 0.5f;
    }

    public void AttackFinished()
    {
        Invoke(nameof(ChangeNormal), attackCoolDown);
        animator.speed = 1f;
    }

    private void ChangeNormal()
    {
        playerStatus.OnNormal();
    }

    public void CheckHit()
    {
        AudioManager.Instance.Play("slash", AudioManager.EclipType.SE);
        if (!enemyInRange) return;
        playerStatus.OnDamage(enemyStatus);
    }

    //player��stateNow��\��
    /*
   private void OnGUI()
   {
       // GUIStyle�̃C���X�^���X���쐬
       GUIStyle myStyle = new GUIStyle
       {
           // �t�H���g�T�C�Y��ݒ�
           fontSize = 36
       };

       // �e�L�X�g�̐F��ݒ�i�I�v�V�����j
       myStyle.normal.textColor = Color.white;

       GUI.Label(new Rect(0, 0, 100, 50), playerStatus.stateNow.ToString(), myStyle);
   }
    */
}
