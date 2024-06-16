using UnityEngine;
using UnityEngine.AI;

public class CollisionDetecter : MonoBehaviour
{
    private StatusBase status;
    private NavMeshAgent agent;
    private Vector3 basePosition;

    private void Awake()
    {
        basePosition = transform.position;
        agent = GetComponentInParent<NavMeshAgent>();
        status = GetComponentInParent<StatusBase>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (status.stateNow == StatusBase.State.Normal)  //status���m�[�}���Ȃ�ǐՏ��� ���̑��Ȃ�ǐՃX�g�b�v
        {
            agent.isStopped = false;
            agent.destination = other.transform.position;
            return;
        }
        agent.isStopped = true;
    }

    private void OnTriggerExit(Collider other)�@�@//�v���C���[�����G�͈͂���o��ƁA�����ʒu�ɖ߂�
    {
        agent.destination = basePosition;
    }
}
