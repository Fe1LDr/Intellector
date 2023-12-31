using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.Reflection;

public class Computer : MonoBehaviour
{
    private Board board;
    private List<int> scores = new()
                    {
                        100, 300, 10000, 900, 300 ,500
                    };
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameGameObject = GameObject.Find("Board");
        board = gameGameObject.GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (board.Turn == true)
        {
            MakeAIMove();
        }
    }

    public void MakeAIMove()
    {
        int max_score = -1000000;
        List<int> bestmove = new()
                    {
                        0,
                        0,
                        0,
                        0,
                        0
                    };
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
                        List<int> currentmove = new()
                    {
                        piece1.x,
                        piece1.y,
                        move.x,
                        move.y,
                        0
                    };
                        allMoves.Add(currentmove);
                    }
                }
            }
        }

        foreach (List<int> move in allMoves)
        {
            // ѕрименение хода к временной копии доски
            Board boardCopy = board.CloneBoard();
            boardCopy.SelectTile(new Vector2Int(move[0], move[1]));
            boardCopy.SelectTile(new Vector2Int(move[2], move[3]));
            //boardCopy.MovePiece(new Vector2Int(move[0], move[1]), new Vector2Int(move[2], move[3]), false);
            boardCopy.pieces[move[2]][move[3]] = boardCopy.pieces[move[0]][move[1]];
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
        board.SelectTile(new Vector2Int(bestmove[0], bestmove[1]));
        board.SelectTile(new Vector2Int(bestmove[2], bestmove[3]));
        

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
                    // ”читываем стоимость фигур в зависимости от команды (белые или черные)
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

        // ¬озвращаем разницу между оценками команд
        return whiteEvaluation - blackEvaluation;
    }


    private int GetPieceValue(PieceType type)
    {
        // »спользуем ваш массив scores дл€ получени€ стоимости по типу фигуры
        return scores[(int)type];
    }
}
