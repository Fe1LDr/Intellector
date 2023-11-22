using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

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
}
