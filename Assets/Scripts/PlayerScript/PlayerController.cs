using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float sensitiveRotate = 3; //視点感度

    private Animator playerAnimator;
    private Rigidbody myRigidbody;   //軽量化のためキャッシュ
    private Transform myTransform;
    private PlayerStatus playerStatus;

    private Vector3 moveVelocity;

    private void Start()
    {

        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (playerStatus.stateNow == StatusBase.State.Down ||
            playerStatus.stateNow == StatusBase.State.Die) return;  //ダウン・死亡状態であれば視点変更・移動はできない
        if (Input.GetMouseButtonUp(1)) playerStatus.GuardFinished();
        VisionCotroll();
        if (playerStatus.stateNow != StatusBase.State.Normal) return;  //ノーマル状態でなければ戦闘アクションは行えない
        Move();
        if (playerStatus.existField != StatusBase.ExistField.Battle) return;　　//バトルシーンでなければ戦闘アクションは行えない
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 1) playerAttack.AttackStart();
        else if (Input.GetMouseButtonDown(1) && Time.timeScale == 1) playerStatus.OnGuard();
    }

    private void Move()
    {
        playerAnimator.SetFloat("Run", 0f);
        moveVelocity = GetMoveVelocity();
        CheckRun(Input.GetKey(KeyCode.LeftShift));  //Shiftキー押されていればスピードアップ
        myRigidbody.velocity = moveVelocity;

        playerAnimator.SetFloat("MoveSpeed", new Vector3(
            moveVelocity.x, 0, moveVelocity.z).magnitude);
    }

    private void VisionCotroll()
    {
        if (Time.deltaTime == 0) return;
        var mouseMoveX = Input.GetAxis("Mouse X") * sensitiveRotate;
        var mouseMoveY = Input.GetAxis("Mouse Y") * sensitiveRotate;

        myTransform.Rotate(0, mouseMoveX, 0);
    }

    private Vector3 GetMoveVelocity()
    {
        // カメラの向きから前後左右への移動ベクトルを計算
        Vector3 moveDirection = Vector3.zero;
        moveDirection += myTransform.forward * Input.GetAxis("Vertical"); // 前後方向
        moveDirection += myTransform.right * Input.GetAxis("Horizontal"); // 左右方向

        // 移動方向に移動スピードをかける
        moveDirection.Normalize();
        moveVelocity.x = moveDirection.x * moveSpeed;
        moveVelocity.z = moveDirection.z * moveSpeed;
        return moveVelocity;
    }

    private void CheckRun(bool run)
    {
        if (!run) return;
        moveVelocity *= 1.5f;
        playerAnimator.SetFloat("Run", 1.0f);
    }

    private void CheckHitForAnimator()  //要調整
    {
        playerAttack.CheckHit();
    }

    private void AttackFinishedForAnimator()  //要調整
    {
        playerAttack.AttackFinished();
    }
}
