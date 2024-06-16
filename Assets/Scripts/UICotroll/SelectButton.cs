using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button leftButton;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowSelectPanel(string text,UnityAction action)�@�@//����1�ɑI����e�@2�ɂ͍��{�^���̃��\�b�h�A�N�V����
    {
        gameObject.SetActive(true);
        leftButton.onClick.RemoveAllListeners();
        leftButton.onClick.AddListener(action);
        questionText.text = text;
    }

    public void CloseSelectPanel()
    {
        GameUIManager.canShowMenu = true;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
