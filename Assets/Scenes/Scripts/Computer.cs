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
    private Weights w = new Weights();
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();
    }

    void Update()
    {
        if (board.Turn == true && board.AI == true)
        {
            board.AI = false;
            MinMaxRoot(3, board.pieces, true);
        }
    }

    public List<int> MinMaxRoot(int depth, Piece[][] pieces, bool isMaximisingPlayer)
    {
        List<List<int>> Moves = GetMoves(pieces, isMaximisingPlayer);
        int maxscore = -9999;
        List<int> bestmove = new() { 0, 0, 0, 0 };

        foreach (List<int> move in Moves)
        {
            Piece[][] clone = new Piece[pieces.Length][];
            for (int i = 0; i < pieces.Length; i++)
            {
                clone[i] = new Piece[pieces[i].Length];
                for (int j = 0; j < pieces[i].Length; j++)
                {
                    if (pieces[i][j] != null)
                    {
                        clone[i][j] = pieces[i][j];
                    }
                }
            }
            clone[move[2]][move[3]] = clone[move[0]][move[1]];
            clone[move[0]][move[1]] = null;

            int eval = MinMax(depth - 1, clone, -10000, 10000, !isMaximisingPlayer);

            if (eval > maxscore)
            {
                maxscore = eval;
                bestmove = move;
            }
            
        }
        Debug.LogFormat("{0}, {1}, {2}, {3}, {4}", maxscore, bestmove[0], bestmove[1], bestmove[2], bestmove[3]);
        board.SelectTile(new Vector2Int(bestmove[0], bestmove[1]));
        board.SelectTile(new Vector2Int(bestmove[2], bestmove[3]));
        Button yourButton = GameObject.Find("Yes")?.GetComponent<Button>();
        if (yourButton != null)
        {
            EventTrigger trigger = yourButton.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = yourButton.gameObject.AddComponent<EventTrigger>();
            }
            yourButton.onClick.Invoke();
        }
        board.AI = true;
        return bestmove;
    }

    public int MinMax(int depth, Piece[][] pieces, int alpha, int beta, bool isMaximisingPlayer)
    {
        if (depth == 0)
        {
            return EvaluatePosition(pieces);
        }
        List<List<int>> Moves = GetMoves(pieces, isMaximisingPlayer);

        if (isMaximisingPlayer)
        {
            int max_score = -9999;
            foreach (List<int> move in Moves)
            {
                Piece[][] clone = new Piece[pieces.Length][];
                for (int i = 0; i < pieces.Length; i++)
                {
                    clone[i] = new Piece[pieces[i].Length];
                    for (int j = 0; j < pieces[i].Length; j++)
                    {
                        if (pieces[i][j] != null)
                        {
                            clone[i][j] = pieces[i][j];
                        }
                    }
                }
                clone[move[2]][move[3]] = clone[move[0]][move[1]];
                clone[move[0]][move[1]] = null;
                max_score = Math.Max(max_score, MinMax(depth - 1, pieces, alpha, beta, !isMaximisingPlayer));
                
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
            int max_score = 9999;
            foreach (List<int> move in Moves)
            {
                Piece[][] clone = new Piece[pieces.Length][];
                for (int i = 0; i < pieces.Length; i++)
                {
                    clone[i] = new Piece[pieces[i].Length];
                    for (int j = 0; j < pieces[i].Length; j++)
                    {
                        if (pieces[i][j] != null)
                        {
                            clone[i][j] = pieces[i][j];
                        }
                    }
                }
                clone[move[2]][move[3]] = clone[move[0]][move[1]];
                clone[move[0]][move[1]] = null;
                max_score = Math.Min(max_score, MinMax(depth - 1, clone, alpha, beta, !isMaximisingPlayer));
                
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
        int total = 0;

        for (int i = 0; i < pieces.Length; i++)
        {
            for (int j = 0; j < pieces[i].Length; j++)
            {
                Piece currentPiece = pieces[i][j];
                if (currentPiece != null)
                {
                    int pieceValue = GetPieceValue(currentPiece, i, j);
                    if (currentPiece.team)
                    {
                        total += pieceValue;
                    }
                    else
                    {
                        total -= pieceValue;
                    }
                }
            }
        }
        return total;
    }

private int GetPieceValue(Piece currentPiece, int x, int y)
    {
        if ((int)currentPiece.type == 0)
        {
            return w.scores[(int)currentPiece.type] + ((currentPiece.team) ? w.blackProgressor[x, y] : w.whiteProgressor[x, y]);
        }
        else if ((int)currentPiece.type == 1)
        {
            return w.scores[(int)currentPiece.type] + ((currentPiece.team) ? w.blackLiberator[x, y] : w.whiteLiberator[x, y]);
        }
        else if ((int)currentPiece.type == 2)
        {
            return w.scores[(int)currentPiece.type] + w.intellector[x, y];
        }
        else if ((int)currentPiece.type == 3)
        {
            return w.scores[(int)currentPiece.type] + ((currentPiece.team) ? w.blackDominator[x, y] : w.whiteDominator[x, y]);
        }
        else if ((int)currentPiece.type == 4)
        {
            return w.scores[(int)currentPiece.type] + w.defensor[x, y];
        }
        else if ((int)currentPiece.type == 5)
        {
            return w.scores[(int)currentPiece.type] + w.argessor[x, y];
        }
        return w.scores[(int)currentPiece.type];
    }


    private List<List<int>> GetMoves(Piece[][] pieces, bool t)
    {
        List<List<int>> allMoves = new List<List<int>>();

        foreach (Piece[] piece in pieces)
        {
            foreach (Piece piece1 in piece)
            {
                if (piece1 is not null && piece1.team == t)
                {
                    List<Vector2Int> availableMoves = piece1.GetAvaibleMooves();
                    foreach (Vector2Int move in availableMoves)
                    {
                        List<int> currentmove = new() { piece1.x, piece1.y, move.x, move.y };
                        allMoves.Add(currentmove);
                    }
                }
            }
        }
        return allMoves;
    }
}
