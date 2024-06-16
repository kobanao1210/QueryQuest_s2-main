using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button LoadButton;

    private void Awake()
    {
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }
    private void Start()
    {
        JsonManager.LoadItemData();
        JsonManager.LoadQuestData();
        AudioManager.Instance.Play("title", AudioManager.EclipType.BGM);
        if (!File.Exists(JsonManager.playerDataPath)) LoadButton.enabled = false;
    }
    public void OnStartButton()
    {
        SetPlayerData();
        Util.LoadTownScene();
    }

    public void OnLoadButton()
    {
        JsonManager.LoadPlayerData();
        Util.LoadTownScene();
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetPlayerData()
    {
        var playerDatabase = PlayerDatabase.Instance;
        playerDatabase.playerStatusData.ResetPlayerStatusData();
        playerDatabase.haveItems.Clear();
    }
}
