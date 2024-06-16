using UnityEngine;

public class InnPanel : MonoBehaviour
{
    private GameUIManager gameUIManager;
    private AudioManager audioManager;
    private PlayerStatusData playerStatusData;
    private void Start()
    {
        gameUIManager = GameObject.Find("GameUIManager").GetComponent<GameUIManager>();
        playerStatusData = PlayerDatabase.Instance.playerStatusData;
        audioManager = AudioManager.Instance;
    }

    public void OnRestButton()
    {
        if (InventryManager.UseMoney(50)) RestPlayer();
        else return;
    }
    public void RestPlayer()
    {
        playerStatusData.Life = playerStatusData.LifeMax;
        gameUIManager.FadeOutPanel(4);
        audioManager.ToggleBGM(4f);
        audioManager.Play("rest", AudioManager.EclipType.SE);
    }
}
