using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;

public class NetworkGamesScene : MonoBehaviour
{
    [SerializeField] GameObject NetworkGamePrefab;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject GameInfoWindow;
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] GameObject WaitingWindow;
    [SerializeField] public Color DefaultColor;
    [SerializeField] public Color SelectedColor;
    [SerializeField] GameObject[] Buttons;

    
    List<GameObject> Items = new List<GameObject>();
    public uint selected_id;


    void Start()
    {
        ShowGamesList();
    }

    public void ShowGamesList()
    {
        ClearItems();
        try
        {
            var games = ServerManager.GetInstance().ReadGames();
            foreach(var game in games)
            {
                DisplayGame(game);
            }
        }
        catch(VersionException e)
        {
            ErrorWindow.SetActive(true);
            ErrorWindow.GetComponentInChildren<Text>().text = e.Message;
            DeactivateButtons();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            ErrorWindow.SetActive(true);
            DeactivateButtons();
        }

        void DeactivateButtons() { foreach (var button in Buttons) button.SetActive(false); }
    }
    void DisplayGame(GameInfo game)
    {
        GameObject net_game_obj = Instantiate(NetworkGamePrefab);
        NetworkGameItem net_game = net_game_obj.GetComponent<NetworkGameItem>();

        net_game.GameInfo = game;
        net_game.NetworkGameScene = this;
        net_game_obj.transform.SetParent(Content.GetComponent<Transform>(), transform);
        net_game.DisplayGameInfo();
        net_game.SetDefaultColor();
        Items.Add(net_game_obj);
    }
    private void ClearItems()
    {
        foreach (GameObject item in Items)
        {
            Destroy(item);
        }
        Items.Clear();
    }
    public void ShowGameInfoWindow()
    {
        GameInfoWindow.SetActive(true);
    }
    public void SetDefaultColors()
    {
        foreach (GameObject game_obj in Items)
        {
            game_obj.GetComponent<NetworkGameItem>().SetDefaultColor();
        }
    }


    public void JoinSelectedGame()
    {
        if(selected_id != 0)
        {
            (bool connect, GameInfo gameInfo) = ServerManager.GetInstance().JoinGame(selected_id);
            if (!connect)
            {
                ErrorWindow.SetActive(true);
                ErrorWindow.GetComponentInChildren<Text>().text = "Игра уже не существует";
                return;
            }
            gameInfo.Save();
            GoToGameScene();
        }
    }
    public void CreateGameConfirmClick()
    {
        GameInfo gameInfo = GameInfoWindow.GetComponent<GameInfoWindow>().GetGameInfo();
        if (gameInfo != null)
        {
            WaitingWindow.SetActive(true);
            ServerManager.GetInstance().CreateGame(gameInfo,GoToGameScene);
        }
    }
    public void CancelWaiting()
    {
        ServerManager.GetInstance().CancelGameCreate();
        WaitingWindow.SetActive(false);
    }



    private void GoToGameScene()
    {
        Settings settings = Settings.Load();
        settings.GameMode = GameMode.Network;
        settings.Save();
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
