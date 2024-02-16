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

public class NetworkGamesScene : MonoBehaviour
{
    [SerializeField] GameObject NetworkGamePrefab;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject GameInfoWindow;
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] public Color DefaultColor;
    [SerializeField] public Color SelectedColor;

    List<GameObject> Items = new List<GameObject>();
    public uint selected_id;

    void Start()
    {
        ReadGamesFromServer();
    }

    public void ReadGamesFromServer()
    {
        const byte games_list_request = 100;
        ClearItems();

        try
        {
            Settings settings = Settings.Load();
            TcpClient server = new TcpClient(Settings.server_IP, Settings.server_port);
            NetworkStream stream = server.GetStream();

            SendString(password, stream);
            SendCode(games_list_request, stream);
            if (!CheckVersion(stream)) return;
            int GameCount = RecvInt(stream);

            for (int i = 0; i < GameCount; i++)
            {
                GameInfo game = RecvGameInfo(stream);
                DisplayGame(game);
            }
            server.Close();
        }

        catch (Exception e)
        {
            Debug.LogException(e);
            ErrorWindow.SetActive(true);
        }

        bool CheckVersion(NetworkStream stream)
        {
            SendInt(Settings.version, stream);
            int server_version = RecvInt(stream);
            if (Settings.version != server_version)
            {
                ErrorWindow.SetActive(true);
                ErrorWindow.GetComponentInChildren<Text>().text = $"Неподходящая версия\nВерсия сервера - {VerToStr(server_version)}\nИспользуемая версия клиента - {VerToStr(Settings.version)}\n";
            }
            return Settings.version == server_version;

            string VerToStr(int ver) => $"{ver/10}.{ver%10}";
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
        StartNetworkGame(0);
    }

    public void JoinSelectedGame()
    {
        if(selected_id != 0)
            StartNetworkGame(selected_id);
    }

    void StartNetworkGame(uint ID)
    {
        Settings settings = Settings.Load();
        settings.NetworkGame = true;
        settings.AIGame = false;
        settings.Game_ID_To_Connect = ID;
        settings.Save();

        if (ID == 0)
        {
            Content.SetActive(false);
            GameInfoWindow.SetActive(true);
        }
            
        else SceneManager.LoadScene(1);
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
    public void ConfirmClick()
    {
        GameInfo gameInfo = GameInfoWindow.GetComponent<GameInfoWindow>().GetGameInfo();
        if (gameInfo != null)
        {
            gameInfo.Save();
            SceneManager.LoadScene(1);
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
