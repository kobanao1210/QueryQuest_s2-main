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
        if (status.stateNow == StatusBase.State.Normal)  //statusがノーマルなら追跡処理 その他なら追跡ストップ
        {
            agent.isStopped = false;
            agent.destination = other.transform.position;
            return;
        }
        agent.isStopped = true;
    }

    private void OnTriggerExit(Collider other)　　//プレイヤーが索敵範囲から出ると、初期位置に戻る
    {
        agent.destination = basePosition;
    }
}
