using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewTestScript {
    private Board board;

    private void SetUp()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();
        board.Awake();
    }

    [UnityTest]
    public IEnumerator SelectTile_Test()
    {
        SetUp();
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0,0));
        yield return new WaitForSeconds(0.1f);

        Assert.That(board.tiles[0][0].layer == 8);
    }

    [UnityTest]
    public IEnumerator Progressor_Test() 
    {
        SetUp();

        Progressor progressor = board.pieces[0][1] as Progressor;
        yield return new WaitForSeconds(0.1f);
        var moves = progressor.GetAvaibleMooves();
        yield return new WaitForSeconds(0.1f);
        // 0 2 and 1 1
        Assert.That(moves.Count == 2 && (moves[0].x == 0 && moves[0].y == 2 && moves[1].x == 1 && moves[1].y == 1));
    }


    [UnityTest]
    public IEnumerator Dominator_Test()
    {
        SetUp();

        Dominator dominator = board.pieces[0][0] as Dominator;
        yield return new WaitForSeconds(0.1f);
        var moves = dominator.GetAvaibleMooves();
        yield return new WaitForSeconds(0.1f);
        // empty
        Assert.That(moves.Count == 0);
    }

    [UnityTest]
    public IEnumerator MenuOpensCorrectly()
    {
        SceneManager.LoadScene("Menu"); // Загрузка сцены с меню

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Проверяем, что меню отображается на экране
        GameObject menu = GameObject.Find("Menu"); // Предположим, что Canvas, содержащий меню, имеет имя "MenuCanvas"
        Assert.IsNotNull(menu); // Проверяем, что объект меню не равен null
    }

    [UnityTest]
    public IEnumerator ClickUIProgressor()
    {
        // Загрузка сцены с вашим UI
        SceneManager.LoadScene("SampleScene");

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Находим кнопку в сцене (предположим, что кнопка имеет тег "UIButton")
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        /*yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 1));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 2));*/
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 1), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(1, 5), new Vector2Int(1, 4), false);
        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator ClickUIIntellectorAround()
    {
        // Загрузка сцены с вашим UI
        SceneManager.LoadScene("SampleScene");

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Находим кнопку в сцене (предположим, что кнопка имеет тег "UIButton")
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(2, 0), new Vector2Int(6, 6), false);
        yield return new WaitForSeconds(2f);
        board.SelectTile(new Vector2Int(5, 5));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(6, 6));
        yield return new WaitForSeconds(2f);
        Button yourButton = GameObject.Find("Yes").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator ClickUIEndGame()
    {
        // Загрузка сцены с вашим UI
        SceneManager.LoadScene("SampleScene");

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Находим кнопку в сцене (предположим, что кнопка имеет тег "UIButton")
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(2, 0), new Vector2Int(6, 6), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(1, 5), new Vector2Int(1, 4), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(6, 6), new Vector2Int(4, 6), false);
        yield return new WaitForSeconds(2f);
        Button yourButton = GameObject.Find("Exit").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return new WaitForSeconds(5f);
    }

    [UnityTest]
    public IEnumerator ClickUIProgressorEnd()
    {
        // Загрузка сцены с вашим UI
        SceneManager.LoadScene("SampleScene");

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Находим кнопку в сцене (предположим, что кнопка имеет тег "UIButton")
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 1), new Vector2Int(0, 5), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 5), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 2), new Vector2Int(0, 3), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 4), new Vector2Int(1, 3), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 3), new Vector2Int(0, 4), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(1, 3), new Vector2Int(0, 3), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 4), new Vector2Int(0, 5), false);
        yield return new WaitForSeconds(2f);
        board.MovePiece(new Vector2Int(0, 3), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 5));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 6));
        yield return new WaitForSeconds(2f);
        Button yourButton = GameObject.Find("Defensor").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return new WaitForSeconds(5f);
    }

    [UnityTest]
    public IEnumerator ClickUIStart()
    {
        // Загрузка сцены с вашим UI
        SceneManager.LoadScene("Menu");

        yield return null; // Ждем один кадр, чтобы сцена загрузилась полностью

        // Находим кнопку в сцене (предположим, что кнопка имеет тег "UIButton")
        Button yourButton = GameObject.Find("Start").GetComponent<Button>();

        Assert.IsNotNull(yourButton, "Button not found");

        // Создаем экземпляр EventTrigger для симуляции нажатия кнопки
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            // Если компонента EventTrigger отсутствует, добавляем его
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }

        // Создаем событие для нажатия кнопки
        yourButton.onClick.Invoke();

        // Ждем один кадр, чтобы событие успело обработаться
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(5f);
    }
}
