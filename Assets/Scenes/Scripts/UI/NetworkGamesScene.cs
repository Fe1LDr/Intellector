using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using static Networking;
using System.Net.Http;
using Assets.Scenes.Scripts;


public class NetworkGamesScene : MonoBehaviour
{
    [SerializeField] GameObject NetworkGamePrefab;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject GameInfoWindow;
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] public Color DefaultColor;
    [SerializeField] public Color SelectedColor;
    [SerializeField] GameObject[] Buttons;


    [SerializeField] ConnectionWaiter connectionWaiter;
    List<GameObject> Items = new List<GameObject>();
    public uint selected_id;

    void Start()
    {
        ReadGamesFromServer();
    }

    public async void ReadGamesFromServer()
    {
        ClearItems();

        try
        {
            Settings settings = Settings.Load();
            List<WaitingGameInfo> gameInfos = await ServerManager.GetRequest<List<WaitingGameInfo>>("WaitingGames");
            foreach (WaitingGameInfo waitingGameInfo in gameInfos)
            {
                DisplayGame(waitingGameInfo.ToGameInfo());
            }
        }

        catch (Exception e)
        {
            Debug.LogException(e);
            ErrorWindow.SetActive(true);
            foreach (var button in Buttons) button.SetActive(false);
        }

        void ClearItems()
        {
            foreach(GameObject item in Items)
            {
                Destroy(item);
            }
            Items.Clear();
        }
    }
        
    public void CreateNewGame()
    {
        SetNetworkSettings(0);
        ShowCreateGameWindow(); 
    }

    public async void JoinSelectedGame()
    {
        if(selected_id != 0)
        {
            SetNetworkSettings(selected_id);
            JoinInfo joinInfo = await ServerManager.GetRequest<JoinInfo>($"JoinGame/{selected_id}");
            GameStarter.StartGame(GameInfo.Load(), joinInfo);
        }
    }

    private void SetNetworkSettings(uint ID)
    {
        Settings settings = Settings.Load();
        settings.NetworkGame = true;
        settings.AIGame = false;
        settings.Game_ID_To_Connect = ID;
        settings.Save();
    }

    private void ShowCreateGameWindow()
    {
        Content.SetActive(false);
        GameInfoWindow.SetActive(true);
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

    void TestFeel(uint id) => DisplayGame(new GameInfo { ID = id, Color = ColorChoice.random, Name = $"Test{id}", TimeContol = new(0,0)});
    public async void ConfirmClick()
    {
        GameInfo gameInfo = GameInfoWindow.GetComponent<GameInfoWindow>().GetGameInfo();
        if (gameInfo != null)
        {
            gameInfo.Save();

            bool? team = gameInfo.Color switch
            {
                ColorChoice.random => null,
                ColorChoice.white => false,
                ColorChoice.black => true,
                _ => null,
            };

            string request = $"CreateGame?Name={gameInfo.Name}&MainTime={gameInfo.TimeContol.MaxMinutes}&AddedTime={gameInfo.TimeContol.AddedSeconds}&Color={team}";
            await ServerManager.PostRequest(request);

            connectionWaiter.StartWaiting(gameInfo);
        }
    }

    public void SetDefaultColors()
    {
        foreach(GameObject game_obj in Items)
        {
            game_obj.GetComponent<NetworkGameItem>().SetDefaultColor();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
