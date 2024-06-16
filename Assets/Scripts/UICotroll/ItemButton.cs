using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item item;
    public GameUIManager gameUIManager;
    public GameObject itemInfoPanel;
    public List<Button> buttons = new List<Button>();
    public void OnItemButton()
    {
        RemoveButtonsLister();
        gameUIManager.SetItemInfo(item);
        itemInfoPanel.SetActive(true);
        buttons[0].onClick.AddListener(OnUseButton);
        buttons[1].onClick.AddListener(OnDisposeButton);
    }
    public void OnUseButton()
    {
        itemInfoPanel.SetActive(false);
        InventryManager.UseItem(item);
    }

    public void OnDisposeButton()
    {
        itemInfoPanel.SetActive(false);
        InventryManager.DisPoseItem(item);
    }

    private void RemoveButtonsLister()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
