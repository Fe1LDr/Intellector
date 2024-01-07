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
    [SerializeField] GameObject AskForNameWindow;
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] Color DefaultColor;
    [SerializeField] public Color SelectedColor;

    List<GameObject> Items = new List<GameObject>();
    List<Button> Buttons = new List<Button>();
    public uint selected_id;

    void Start()
    {
        ReadGamesFromServer();
    }

    public void ReadGamesFromServer()
    {
        ClearItems();
        NetworkStream stream;
        const byte games_list_request = 100;

        try
        {
            Settings settings = Settings.Load();
            TcpClient server = new TcpClient(settings.ServerIP, 7001);
            stream = server.GetStream();

            SendString(password, stream);
            Networking.SendMessage(games_list_request, stream);
            uint GameCount = RecvMessage(stream);

            for (int i = 0; i < GameCount; i++)
            {
                ReadGame();
            }
            server.Close();
        }
        catch (Exception)
        {
            ErrorWindow.SetActive(true);
        }

        void ReadGame()
        {
            GameInfo game = RecvGameInfo(stream);
            DisplayGame(game);
        }

        void ClearItems()
        {
            foreach(GameObject item in Items)
            {
                Destroy(item);
            }
            Items.Clear();
            Buttons.Clear();
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
        settings.Game_ID_To_Connect = ID;

        settings.Save();
        if (String.IsNullOrEmpty(settings.UserName))
        {
            AskForNameWindow.SetActive(true);
            return;
        }
        SceneManager.LoadScene(1);
    }

    void DisplayGame(GameInfo game)
    {
        Transform content_transform = Content.GetComponent<Transform>();
        GameObject net_game_obj = Instantiate(NetworkGamePrefab);
        NetworkGameItem net_game = net_game_obj.GetComponent<NetworkGameItem>();

        net_game.GameInfo = game;
        net_game.NetworkGameScene = this;
        net_game_obj.transform.SetParent(content_transform, transform);
        net_game_obj.GetComponentInChildren<Image>().color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 1f);

        Text text = net_game_obj.GetComponentInChildren<Text>();
        text.text = game.Name;
        Items.Add(net_game_obj);
        Buttons.Add(net_game_obj.GetComponent<Button>());
    }

    void TestFeel(uint id) => DisplayGame(new GameInfo(id, $"Test {id}"));
    public void ConfirmNameInput()
    {
        string NameInput = AskForNameWindow.GetComponentInChildren<InputField>().text;
        Text text = AskForNameWindow.GetComponentInChildren<Text>();

        string error_mes;
        if(!Settings.CheckName(NameInput, out error_mes))
        {
            text.text = error_mes;
            return;
        }
            
        Settings settings = Settings.Load();
        settings.UserName = NameInput; 
        settings.Save();
        SceneManager.LoadScene(1);
    }

    public void RemoveSelection()
    {
        foreach(Button button in Buttons)
        {
            button.GetComponent<Image>().color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 1f);
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
