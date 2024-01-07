using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameItem : MonoBehaviour
{
    public NetworkGamesScene NetworkGameScene;
    public GameInfo GameInfo { get; set; }

    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SetSelectedNumber);
    }

    void SetSelectedNumber()
    {
        NetworkGameScene.selected_id = GameInfo.ID;
        Button button = GetComponent<Button>();
        NetworkGameScene.RemoveSelection();
        button.GetComponent<Image>().color = new Color(NetworkGameScene.SelectedColor.r, NetworkGameScene.SelectedColor.g, NetworkGameScene.SelectedColor.b, 1f);
    }


}