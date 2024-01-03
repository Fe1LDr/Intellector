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
    private List<int> scores = new() { 10, 30, 1000, 90, 40, 50 };
    private int[,] whiteProgressor = { { 0, 1, 0, 1, 2, 5, 0 }, 
                                       { 0, 2, 0, 1, 2, 5, -1},
                                       { 5, 1, 0, 1, 2, 5, 0},
                                       { 2, 3, 0, 1, 2, 5, -1 },
                                       { 0, 1, 0, 1, 2, 5, 0},
                                       { 2, 3, 0, 1, 2, 5, -1},
                                       { 5, 1, 0, 1, 2, 5, 0 },
                                       { 0, 2, 0, 1, 2, 5, -1},
                                       { 0, 1, 0, 1, 2, 5, 0}, };
    private int[,] blackProgressor = { { 0, 5, 2, 1, 0, 1, 0 },
                                       { 5, 2, 1, 0, 2, 5, -1},
                                       { -5, 5, 2, 1, 0, 1, 0},
                                       { 5, 2, 1, 0, 2, 3, -1},
                                       { 0, 5, 2, 1, 0, 1, 0},
                                       { 5, 2, 1, 0, 2, 3, -1},
                                       { -5, 5, 2, 1, 0, 1, 0},
                                       { 5, 2, 1, 0, 2, 5, -1},
                                       { 0, 5, 2, 1, 0, 1, 0}, };
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
            MinMaxRoot(4, board.pieces, true);
        }
    }

    public void MakeAIMove()
    {
        int max_score = -1000000;
        List<int> bestmove = new() { 0, 0, 0, 0 };
        List<List<int>> allMoves = GetMoves(board.pieces, true);

        foreach (List<int> move in allMoves)
        {
            // ѕрименение хода к временной копии доски
            Board boardCopy = board.CloneBoard(board);
            boardCopy.SelectTile(new Vector2Int(move[0], move[1]));
            boardCopy.SelectTile(new Vector2Int(move[2], move[3]));
            //boardCopy.MovePiece(new Vector2Int(move[0], move[1]), new Vector2Int(move[2], move[3]), false);
            boardCopy.pieces[move[2]][move[3]] = boardCopy.pieces[move[0]][move[1]];
            boardCopy.pieces[move[2]][move[3]].x = move[2];
            boardCopy.pieces[move[2]][move[3]].y = move[3];
            boardCopy.pieces[move[0]][move[1]] = null;

            int evaluation = EvaluatePosition(boardCopy.pieces);
            
            if (evaluation > max_score)
            {
                max_score = evaluation;
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

    public List<int> MinMaxRoot(int depth, Piece[][] pieces, bool isMaximisingPlayer)
    {
        List<List<int>> Moves = GetMoves(pieces, isMaximisingPlayer);
        int maxscore = -100000;
        List<int> bestmove = new() { 0, 0, 0, 0 };
        int ev = EvaluatePosition(pieces);

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

            int eval = MinMax(depth - 1, clone, -10000, 10000, !isMaximisingPlayer, move);

            if (eval > maxscore)
            {
                maxscore = eval;
                bestmove = move;
            }
        }
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

    public int MinMax(int depth, Piece[][] pieces, int alpha, int beta, bool isMaximisingPlayer, List<int> moveLast)
    {
        if (depth == 0)
        {
            return newEval(pieces, moveLast);
        }
        List<List<int>> Moves = GetMoves(pieces, isMaximisingPlayer);

        if (isMaximisingPlayer)
        {
            int max_score = -100000;
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
                max_score = Math.Max(max_score, MinMax(depth - 1, pieces, alpha, beta, !isMaximisingPlayer, move));
                
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
                max_score = Math.Min(max_score, MinMax(depth - 1, clone, alpha, beta, !isMaximisingPlayer, move));
                
                beta = Math.Min(beta, max_score);
                if (beta <= alpha)
                {
                    return max_score;
                }
            }
            return max_score;
        }
    }

    public int newEval(Piece[][] pieces, List<int> move)
    {
        

        return EvaluatePosition(pieces);
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
                    int pieceValue = GetPieceValue(currentPiece.type) + ((currentPiece.team) ? blackProgressor[i, j] : whiteProgressor[i, j]);
                    if (currentPiece.team)
                    {
                        blackEvaluation += pieceValue;
                    }
                    else
                    {
                        whiteEvaluation += pieceValue;
                    }
                }
            }
        }
        return blackEvaluation - whiteEvaluation;
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
