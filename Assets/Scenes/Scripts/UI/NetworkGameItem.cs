using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameItem : MonoBehaviour
{
    public NetworkGamesScene NetworkGameScene { get; set; }
    public GameInfo GameInfo { get; set; }

    void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(SetSelectedNumber);
    }

    public void DisplayGameInfo()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        texts[0].text = GameInfo.Name;
        texts[1].text = GameInfo.TimeContol.ToString();
        SetColor();
    }

    public void SetSelectedColor()
    {
        GetComponent<Button>().GetComponent<Image>().color = new Color(NetworkGameScene.SelectedColor.r, NetworkGameScene.SelectedColor.g, NetworkGameScene.SelectedColor.b, 1f);
    }

    public void SetDefaultColor()
    {
        GetComponent<Button>().GetComponent<Image>().color = new Color(NetworkGameScene.DefaultColor.r, NetworkGameScene.DefaultColor.g, NetworkGameScene.DefaultColor.b, 1f);
    }

    private void SetSelectedNumber()
    {
        NetworkGameScene.selected_id = GameInfo.ID;
        NetworkGameScene.SetDefaultColors();
        SetSelectedColor();
    }

    private void SetColor()
    {
        GameObject colors = transform.Find("Color").gameObject;
        GameObject white = colors.transform.Find("WhiteColor").gameObject;
        GameObject black = colors.transform.Find("BlackColor").gameObject;
        GameObject random = colors.transform.Find("RandomColor").gameObject;
        switch (GameInfo.Color)
        {
            case ColorChoice.white: white.SetActive(true); break;
            case ColorChoice.black: black.SetActive(true); break;
            case ColorChoice.random: random.SetActive(true); break;
        }
    }
}
