using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Codice.CM.Client.Differences;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    private Board board;
    private List<int> scores = new() { 100, 300, 10000, 900, 300 ,500 };
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (board.Turn == true && board.AI == true)
        {
            board.AI = false;
            MinMaxRoot(2, board, true);
        }
    }

    public void MakeAIMove()
    {
        int max_score = -1000000;
        List<int> bestmove = new() { 0, 0, 0, 0, 0 };
        List<List<int>> allMoves = GetMoves(board);

        foreach (List<int> move in allMoves)
        {
            // ѕрименение хода к временной копии доски
            Board boardCopy = board.CloneBoard();
            boardCopy.SelectTile(new Vector2Int(move[0], move[1]));
            boardCopy.SelectTile(new Vector2Int(move[2], move[3]));
            //boardCopy.MovePiece(new Vector2Int(move[0], move[1]), new Vector2Int(move[2], move[3]), false);
            boardCopy.pieces[move[2]][move[3]] = boardCopy.pieces[move[0]][move[1]];
            boardCopy.pieces[move[2]][move[3]].x = move[2];
            boardCopy.pieces[move[2]][move[3]].y = move[3];
            boardCopy.pieces[move[0]][move[1]] = null;

            int evaluation = EvaluatePosition(boardCopy.pieces);
            
            move[4] = evaluation;
            if (move[4] > max_score)
            {
                max_score = move[4];
                bestmove[0] = move[0];
                bestmove[1] = move[1];
                bestmove[2] = move[2];
                bestmove[3] = move[3];
            }

            DestroyBoard(boardCopy);
        }
        board.SelectTile(new Vector2Int(bestmove[0], bestmove[1]));
        board.SelectTile(new Vector2Int(bestmove[2], bestmove[3]));
        Button yourButton = GameObject.Find("Yes")?.GetComponent<Button>();
        if ( yourButton != null )
        {
            EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = yourButton.gameObject.AddComponent<EventTrigger>();
            }
            yourButton.onClick.Invoke();
        }
        
        board.AI = true;
    }

    public List<int> MinMaxRoot(int depth, Board board1, bool isMaximisingPlayer)
    {
        List<List<int>> Moves = GetMoves(board1);
        int maxscore = -100000;
        List<int> bestmove = new() { 0, 0, 0, 0, 0 };

        foreach (List<int> move in Moves)
        {
            Board boardCopy = board1.CloneBoard();
            //boardCopy.SelectTile(new Vector2Int(move[0], move[1]));
            //boardCopy.SelectTile(new Vector2Int(move[2], move[3]));
            boardCopy.pieces[move[2]][move[3]] = boardCopy.pieces[move[0]][move[1]];
            boardCopy.pieces[move[2]][move[3]].x = move[2];
            boardCopy.pieces[move[2]][move[3]].y = move[3];
            boardCopy.pieces[move[0]][move[1]] = null;
            boardCopy.Turn = !boardCopy.Turn;
            int eval = MinMax(depth - 1, boardCopy, -10000, 10000, !isMaximisingPlayer);

            DestroyBoard(boardCopy);
            if (eval > maxscore)
            {
                maxscore = eval;
                bestmove = move;
            }
        }
        board1.SelectTile(new Vector2Int(bestmove[0], bestmove[1]));
        board1.SelectTile(new Vector2Int(bestmove[2], bestmove[3]));
        board1.AI = true;
        return bestmove;
    }

    public int MinMax(int depth, Board boardC, int alpha, int beta, bool isMaximisingPlayer)
    {
        if (depth == 0)
        {
            return EvaluatePosition(boardC.pieces);
        }

        List<List<int>> Moves = GetMoves(boardC);

        if (isMaximisingPlayer)
        {
            int max_score = -100000;
            foreach (List<int> move in Moves)
            {
                Board boardCop = boardC.CloneBoard();
                //boardCop.SelectTile(new Vector2Int(move[0], move[1]));
                //boardCop.SelectTile(new Vector2Int(move[2], move[3]));
                boardCop.pieces[move[2]][move[3]] = boardCop.pieces[move[0]][move[1]];
                boardCop.pieces[move[2]][move[3]].x = move[2];
                boardCop.pieces[move[2]][move[3]].y = move[3];
                boardCop.pieces[move[0]][move[1]] = null;
                boardCop.Turn = !boardCop.Turn;
                max_score = Math.Max(max_score, MinMax(depth - 1, boardCop, alpha, beta, !isMaximisingPlayer));
                DestroyBoard(boardCop);
                alpha = Math.Max(alpha, max_score);
                if (beta <= alpha)
                {
                    return max_score;
                }
            }
            return max_score;
        }
        else
        {
            int max_score = 100000;
            foreach (List<int> move in Moves)
            {
                Board boardCop = boardC.CloneBoard();
                boardCop.SelectTile(new Vector2Int(move[0], move[1]));
                boardCop.SelectTile(new Vector2Int(move[2], move[3]));
                boardCop.pieces[move[2]][move[3]] = boardCop.pieces[move[0]][move[1]];
                boardCop.pieces[move[2]][move[3]].x = move[2];
                boardCop.pieces[move[2]][move[3]].y = move[3];
                boardCop.pieces[move[0]][move[1]] = null;
                boardCop.Turn = !boardCop.Turn;
                max_score = Math.Min(max_score, MinMax(depth - 1, boardCop, alpha, beta, !isMaximisingPlayer));
                DestroyBoard(boardCop);
                beta = Math.Min(beta, max_score);
                if (beta <= alpha)
                {
                    return max_score;
                }
            }
            return max_score;
        }
    }

    public int EvaluatePosition(Piece[][] pieces)
    {
        int whiteEvaluation = 0;
        int blackEvaluation = 0;

        for (int i = 0; i < pieces.Length; i++)
        {
            for (int j = 0; j < pieces[i].Length; j++)
            {
                Piece currentPiece = pieces[i][j];
                if (currentPiece != null)
                {
                    int pieceValue = GetPieceValue(currentPiece.type);
                    if (currentPiece.team)
                    {
                        whiteEvaluation += pieceValue;
                    }
                    else
                    {
                        blackEvaluation += pieceValue;
                    }
                }
            }
        }
        return whiteEvaluation - blackEvaluation;
    }


    private int GetPieceValue(PieceType type)
    {
        return scores[(int)type];
    }

    private void DestroyBoard(Board boardCopy)
    {
        foreach (Piece[] row in boardCopy.pieces)
        {
            foreach (Piece piece in row)
            {
                if (piece != null)
                {
                    Destroy(piece.gameObject);
                }
            }
        }

        foreach (GameObject[] row in boardCopy.tiles)
        {
            foreach (GameObject tile in row)
            {
                if (tile != null)
                {
                    Destroy(tile);
                }
            }
        }
        Destroy(boardCopy.gameObject);
    }

    private List<List<int>> GetMoves(Board board)
    {
        List<List<int>> allMoves = new List<List<int>>();

        foreach (Piece[] piece in board.pieces)
        {
            foreach (Piece piece1 in piece)
            {
                if (piece1 is not null && piece1.team == true)
                {
                    List<Vector2Int> availableMoves = piece1.GetAvaibleMooves();
                    foreach (Vector2Int move in availableMoves)
                    {
                        List<int> currentmove = new() { piece1.x, piece1.y, move.x, move.y, 0 };
                        allMoves.Add(currentmove);
                    }
                }
            }
        }
        return allMoves;
    }
}
