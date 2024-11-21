using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITests
{
    private Board board;

    [UnityTest]
    public IEnumerator MenuOpensCorrectly() // Проверка запуска сцены Menu
    {
        SceneManager.LoadScene("Menu");

        yield return null;

        GameObject menu = GameObject.Find("Menu");
        Assert.IsNotNull(menu);
        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator StartGameAndBackToMenu()
    {
        SceneManager.LoadScene("Menu");
        yield return null;

        Button yourButton = GameObject.Find("LocalGame").GetComponent<Button>();
        Assert.IsNotNull(yourButton, "Button not found");
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return null;
        yield return new WaitForSeconds(1f);
        Button yourButton2 = GameObject.Find("ExitMenu").GetComponent<Button>();
        Assert.IsNotNull(yourButton2, "Button not found");
        EventTrigger trigger2 = yourButton2.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger2 = yourButton2.gameObject.AddComponent<EventTrigger>();
        }
        yourButton2.onClick.Invoke();
        yield return null;
    }

    [UnityTest]
    public IEnumerator ClickUIProgressor() // Ход Progressor
    {
        SceneManager.LoadScene("SampleScene");

        yield return null;

        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 1), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(1, 5), new Vector2Int(1, 4), false);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator ClickUIIntellectorAround() // Проверка метода Around_intellector 
    {
        SceneManager.LoadScene("SampleScene");

        yield return null;

        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(2, 0), new Vector2Int(6, 6), false);
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(5, 5));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(6, 6));
        yield return new WaitForSeconds(0.1f);
        Button yourButton = GameObject.Find("Yes").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return null;
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(board.pieces[6][6].Type, PieceType.agressor);
    }

    [UnityTest]
    public IEnumerator ClickUIEndGame() // Корректность завершения игры 
    {
        SceneManager.LoadScene("SampleScene");

        yield return null;

        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(2, 0), new Vector2Int(6, 6), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(1, 5), new Vector2Int(1, 4), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(6, 6), new Vector2Int(4, 6), false);
        yield return new WaitForSeconds(0.1f);
        Button yourButton = GameObject.Find("Exit").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return null;
        yield return new WaitForSeconds(2f);
    }

    [UnityTest]
    public IEnumerator ClickUIProgressorEnd() // Пешка доходит до конца
    {
        SceneManager.LoadScene("SampleScene");

        yield return null;

        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();

        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 1), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 5), new Vector2Int(0, 4), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 2), new Vector2Int(0, 3), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 4), new Vector2Int(1, 3), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 3), new Vector2Int(0, 4), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(1, 3), new Vector2Int(0, 3), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 4), new Vector2Int(0, 5), false);
        yield return new WaitForSeconds(0.1f);
        board.MovePiece(new Vector2Int(0, 3), new Vector2Int(0, 2), false);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 5));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0, 6));
        yield return new WaitForSeconds(0.1f);
        Button yourButton = GameObject.Find("Defensor").GetComponent<Button>();
        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }
        yourButton.onClick.Invoke();
        yield return null;
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(board.pieces[0][6].Type, PieceType.defensor);
    }

    [UnityTest]
    public IEnumerator ClickUIStart() // Запуск игры из Menu
    {
        SceneManager.LoadScene("Menu");

        yield return null;

        Button yourButton = GameObject.Find("LocalGame").GetComponent<Button>();

        Assert.IsNotNull(yourButton, "Button not found");


        EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            // Если компонента EventTrigger отсутствует, добавляем его
            trigger = yourButton.gameObject.AddComponent<EventTrigger>();
        }

        // Создаем событие для нажатия кнопки
        yourButton.onClick.Invoke();
        yield return null;
        yield return new WaitForSeconds(2f);
    }
}
