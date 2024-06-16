using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float sensitiveRotate = 3; //���_���x

    private Animator playerAnimator;
    private Rigidbody myRigidbody;   //�y�ʉ��̂��߃L���b�V��
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
            playerStatus.stateNow == StatusBase.State.Die) return;  //�_�E���E���S��Ԃł���Ύ��_�ύX�E�ړ��͂ł��Ȃ�
        if (Input.GetMouseButtonUp(1)) playerStatus.GuardFinished();
        VisionCotroll();
        if (playerStatus.stateNow != StatusBase.State.Normal) return;  //�m�[�}����ԂłȂ���ΐ퓬�A�N�V�����͍s���Ȃ�
        Move();
        if (playerStatus.existField != StatusBase.ExistField.Battle) return;�@�@//�o�g���V�[���łȂ���ΐ퓬�A�N�V�����͍s���Ȃ�
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 1) playerAttack.AttackStart();
        else if (Input.GetMouseButtonDown(1) && Time.timeScale == 1) playerStatus.OnGuard();
    }

    private void Move()
    {
        playerAnimator.SetFloat("Run", 0f);
        moveVelocity = GetMoveVelocity();
        CheckRun(Input.GetKey(KeyCode.LeftShift));  //Shift�L�[������Ă���΃X�s�[�h�A�b�v
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
        // �J�����̌�������O�㍶�E�ւ̈ړ��x�N�g�����v�Z
        Vector3 moveDirection = Vector3.zero;
        moveDirection += myTransform.forward * Input.GetAxis("Vertical"); // �O�����
        moveDirection += myTransform.right * Input.GetAxis("Horizontal"); // ���E����

        // �ړ������Ɉړ��X�s�[�h��������
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

    private void CheckHitForAnimator()  //�v����
    {
        playerAttack.CheckHit();
    }

    private void AttackFinishedForAnimator()  //�v����
    {
        playerAttack.AttackFinished();
    }
}
