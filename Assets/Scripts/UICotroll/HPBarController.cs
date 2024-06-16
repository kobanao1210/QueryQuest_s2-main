using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    [SerializeField] private Image HPBarImage;

    private PlayerStatusData playerStatusData;
    private StatusBase enemyStatus;

    private void Start()
    {
        playerStatusData = PlayerDatabase.Instance.playerStatusData;
        enemyStatus = GetComponentInParent<StatusBase>();
        RefreshHPBar();
    }
    public void RefreshHPBar()  //HP�̕ϓ��ƃ��C�t�o�[��A������,�cHP�ɂ���ĐF��ς���
    {
        if (enemyStatus == null)
        {
            HPBarImage.fillAmount = (float)playerStatusData.Life / playerStatusData.LifeMax;
            if (HPBarImage.fillAmount < 0.2f) HPBarImage.color = Color.red;
            else if (HPBarImage.fillAmount < 0.5f) HPBarImage.color = Color.yellow;
            else HPBarImage.color = Color.green;
        }
        else
        {
            HPBarImage.fillAmount = (float)enemyStatus.Life / enemyStatus.LifeMax;
            if (HPBarImage.fillAmount < 0.2f) HPBarImage.color = Color.red;
            else if (HPBarImage.fillAmount < 0.5f) HPBarImage.color = Color.yellow;
            else HPBarImage.color = Color.green;
        }
    }
}
