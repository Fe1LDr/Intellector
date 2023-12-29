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
    [SerializeField] GameObject AskForNameWindow;
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] Color DefaultColor;
    [SerializeField] public Color SelectedColor;

    List<GameObject> Items = new List<GameObject>();
    List<Button> Buttons = new List<Button>();
    public uint selected_id;

    private const string password = "a3P1>8]џ-/й„€Ё975?:$qcDы‘9&e@1a<c{a/";

    void Start()
    {
        ReadGamesFromServer();
    }

    public void ReadGamesFromServer()
    {
        ClearItems();
        NetworkStream stream;
        try
        {
            Settings settings = Settings.Load();
            TcpClient server = new TcpClient(settings.ServerIP, 7001);
            stream = server.GetStream();

            byte[] password_bytes = Encoding.Default.GetBytes(password);
            stream.Write(password_bytes, 0, password_bytes.Length);

            byte[] SentIdBytes = new byte[1] { 100 };
            stream.Write(SentIdBytes, 0, 1);

            byte[] GetFromServerBytes = new byte[1];
            stream.Read(GetFromServerBytes, 0, 1);

            uint GameCount = GetFromServerBytes[0];
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
            byte[] id_bytes = new byte[4];
            byte[] name_bytes = new byte[20];
            stream.Read(id_bytes, 0, 4);
            stream.Read(name_bytes, 0, 20);
            uint id = BitConverter.ToUInt32(id_bytes);
            string name = Encoding.Default.GetString(name_bytes).TrimEnd('\0');
            DisplayGame(id, name);
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

    void DisplayGame(uint id, string name)
    {
        Transform content_transform = Content.GetComponent<Transform>();
        GameObject net_game_obj = Instantiate(NetworkGamePrefab);
        NetworkGameItem net_game = net_game_obj.GetComponent<NetworkGameItem>();

        net_game.ID = id;
        net_game.NetworkGameScene = this;
        net_game_obj.transform.SetParent(content_transform, transform);
        net_game_obj.GetComponentInChildren<Image>().color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 1f);

        Text text = net_game_obj.GetComponentInChildren<Text>();
        text.text = name;
        Items.Add(net_game_obj);
        Buttons.Add(net_game_obj.GetComponent<Button>());
    }

    void TestFeel(uint id)
    {
        Transform content_transform = Content.GetComponent<Transform>();
        GameObject net_game_obj = Instantiate(NetworkGamePrefab);
        NetworkGameItem net_game = net_game_obj.GetComponent<NetworkGameItem>();

        net_game.ID = id;
        net_game.NetworkGameScene = this;
        net_game_obj.transform.SetParent(content_transform, transform);
        net_game_obj.GetComponentInChildren<Image>().color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, 1f);

        Text text = net_game_obj.GetComponentInChildren<Text>();
        text.text = $"Test {id}";
        Items.Add(net_game_obj);
        Buttons.Add(net_game_obj.GetComponent<Button>());
    }

    public void ConfirmNameInput()
    {
        string NameInput = AskForNameWindow.GetComponentInChildren<InputField>().text;
        Text text = AskForNameWindow.GetComponentInChildren<Text>();

        if (String.IsNullOrEmpty(NameInput))
        {
            text.text = "»м€ не должно быть пустым";
            return;
        }  
        if (Encoding.Default.GetBytes(NameInput).Length > 20)
        {
            text.text = "»м€ не должно быть длинне 20 символов";
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
