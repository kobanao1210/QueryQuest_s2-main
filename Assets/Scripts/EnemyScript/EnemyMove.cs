using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour  
{
    [SerializeField] private SphereCollider searchCollider;
    [SerializeField] private EnemyAttack enemyAttack;

    private EnemyStatus status;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        status = GetComponent<EnemyStatus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
        CheckMoveType(status.existField);
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);
    }

    public void CheckMoveType(EnemyStatus.ExistField existField)  //現在のフィールドによって、索敵範囲を変動
    { 
        if (existField == StatusBase.ExistField.Map) searchCollider.radius = 4;
        else searchCollider.radius = 30;
    }

    public void StepIn()　　//攻撃時に向いている方向へ小前進
    {
        transform.AddComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    private void CheckHitForAnimator()
    {
        enemyAttack.CheckHit();
        StepIn();
    }

    private void AttackFinishedForAnimator()
    {
        enemyAttack.AttackFinished();
        Destroy(GetComponent<Rigidbody>());
    }
}
