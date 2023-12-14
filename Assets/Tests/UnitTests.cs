using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;
using UnityEditor;
using NUnit;

public class UnitTests {
    private Board board;

    private void SetUp()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();
        board.Awake();
    }

    [UnityTest]
    public IEnumerator SelectTileTest()
    {
        SetUp();
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(0,0));
        yield return new WaitForSeconds(0.1f);

        Assert.That(board.tiles[0][0].layer == 8);
    }

    [UnityTest]
    public IEnumerator ProgressorAvailibaleMovesTest() 
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
    public IEnumerator DominatorNoMovesTest()
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
    public IEnumerator IntellectorCastlingTest()
    {
        SetUp();

        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(4, 0));
        yield return new WaitForSeconds(0.1f);
        board.SelectTile(new Vector2Int(3, 0));
        yield return new WaitForSeconds(0.1f);

        PieceType result = board.pieces[3][0].type;
        Assert.AreEqual(result, PieceType.intellector);
    }

    [UnityTest]
    public IEnumerator Test()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();
        yield return null;
        Piece piece = board.GenerateSinglePiece(PieceType.intellector, false, 1000, -1000);
        yield return null;

        PieceType result = piece.type;
        Assert.AreEqual(result, PieceType.intellector);
    }

    [UnityTest]
    public IEnumerator GenerateAllTilesTest()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();

        yield return null;
        board.GenerateAllTiles();
        yield return null;

        int result = 0;
        for (int i = 0; i < board.tiles.Length; i++)
        {
            result += board.tiles[i].Length;
        }
        Assert.AreEqual(result, 59);
    }

    [UnityTest]
    public IEnumerator GenerateOneTileTest()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();

        yield return null;
        int i = 4;
        int j = 5;
        GameObject tile = board.GenerateOneTile(i, j);
        yield return null;

        //Assert.IsNotNull(tile);
        Assert.That(tile.name == $"tile {i} {j}");
    }

    [UnityTest]
    public IEnumerator GenerateAllPiecesTest()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();

        yield return null;
        board.GenerateAllPieces();
        yield return null;

        int result = 0;
        for (int i = 0; i < board.pieces.Length; i++)
        {
            for (int j = 0; j < board.pieces[i].Length; j++)
            {
                if (board.pieces[i][j] != null)
                {
                    result += 1;
                }
            }
        }
        Assert.AreEqual(result, 28);
    }

    [UnityTest]
    public IEnumerator GenerateOnePieceTest()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();

        yield return null;
        Piece piece = board.GenerateSinglePiece(PieceType.dominator, false, 8, 0);
        yield return null;

        PieceType result = piece.type;
        Assert.AreEqual(result, PieceType.dominator);
    }

    [UnityTest]
    public IEnumerator GenerateNotOnBoardTest()
    {
        GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Board"));
        board = gameGameObject.GetComponent<Board>();
        yield return null;
        Piece piece = board.GenerateSinglePiece(PieceType.intellector, false, 1000, -1000);
        yield return null;

        PieceType result = piece.type;
        Assert.AreEqual(result, PieceType.intellector);
    }
}
