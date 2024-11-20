using System.Collections.Generic;
using UnityEngine;

public class VirtualAgressor : VirtualPiece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //ходы вправо
        for (int i = x + 2; i <= 8; i += 2)
        {
            if (board[i][y] != null && board[i][y].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, y));
            if (board[i][y] != null && board[i][y].team != team) break;      //есть фигура и она вражеская
        }

        //ходы влево
        for (int i = x - 2; i >= 0; i -= 2)
        {
            if (board[i][y] != null && board[i][y].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, y));
            if (board[i][y] != null && board[i][y].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх вправо
        for (int i = x + 1, j = y + 1; i <= 8; i++, j++)
        {
            if (i % 2 == 0) j++;
            if (j >= board[i].Length) break;                                             //верхняя граница

            if (board[i][j] != null && board[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вниз вправо
        for (int i = x + 1, j = y - 1; i <= 8; i++, j--)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //нижняя граница

            if (board[i][j] != null && board[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх влево
        for (int i = x - 1, j = y + 1; i >= 0; i--, j++)
        {
            if (i % 2 == 0) j++;
            if (j >= board[i].Length) break;                                             //верхняя граница

            if (board[i][j] != null && board[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вниз влево
        for (int i = x - 1, j = y - 1; i >= 0; i--, j--)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //нижняя граница

            if (board[i][j] != null && board[i][j].team == team) break;      //есть фигура и она союзная
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //есть фигура и она вражеская
        }

        return result;
    }
}
