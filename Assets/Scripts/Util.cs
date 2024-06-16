using UnityEngine;
using UnityEngine.SceneManagement;

public  class Util
{
    public static void LoadTownScene()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.Play("town", AudioManager.EclipType.BGM);
        GameUIManager.canShowMenu = true;
        SceneManager.LoadScene("TownScene");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public static void LoadMapScene()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.Play("field", AudioManager.EclipType.BGM);
        GameUIManager.canShowMenu = true;
        SceneManager.LoadScene("MapScene");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public static void LoadBattleScene()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.Play("battle", AudioManager.EclipType.BGM);
        GameUIManager.canShowMenu = true;
        SceneManager.LoadScene("BattleScene");
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public static void LoadTitleScene()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.Play("title", AudioManager.EclipType.BGM);
        GameUIManager.canShowMenu = false;
        SceneManager.LoadScene("TitleScene");
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        GameUIManager.canShowMenu = false;
    }
}
