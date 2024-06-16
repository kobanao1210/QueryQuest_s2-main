using TMPro;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyNameText;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Util.LoadBattleScene();
    }

    public void ShowMapPanel(string enemyName)
    {
        Util.PauseGame();
        gameObject.SetActive(true);
        enemyNameText.text = enemyName + "‚ª";
    }
}
