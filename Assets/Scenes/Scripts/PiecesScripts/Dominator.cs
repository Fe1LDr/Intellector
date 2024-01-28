using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dominator : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //ходы вверх
        for (int j = y + 1; j < board.pieces[x].Length; j++)
        {
            if (board.pieces[x][j] != null && board.pieces[x][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы вниз
        for (int j = y - 1; j >= 0; j--)
        {
            if (board.pieces[x][j] != null && board.pieces[x][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх вправо
        for (int i = x + 1, j = y; i <= 8; i++)
        {
            if (i % 2 == 0) j++;
            if (j >= board.pieces[i].Length) break;                                             //верхняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вниз вправо
        for (int i = x + 1, j = y; i <= 8; i++)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //нижняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх влево
        for (int i = x - 1, j = y; i >= 0; i--)
        {
            if (i % 2 == 0) j++;
            if (j >= board.pieces[i].Length) break;                                             //верхняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вниз влево
        for (int i = x - 1, j = y; i >= 0; i--) 
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //нижняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != team) break;      //есть фигура и она вражеская
        }

        return result;
    }
}
