using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualLiberator : VirtualPiece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //ближний круг
        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0) continue;                                                                        //левая граница
            if (i > 8) continue;                                                                        //правая граница

            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0) continue;                                                                    //нижняя граница
                if (j >= board[i].Length) continue;                                              //верхняя граница

                if (x == i && y == j) continue;                                                         //клетка с фигурой
                if (x % 2 == 0 && y + 1 == j && x != i) continue;                                       //две лишние клетки сверху
                if (x % 2 == 1 && y - 1 == j && x != i) continue;                                       //две лишние клетки снизу

                if (board[i][j] != null) continue;                                               //есть фигура

                result.Add(new Vector2Int(i, j));
            }
        }

        //дальний круг
        if(y + 2 < board[x].Length)                                                              //вверх
            if (board[x][y + 2] == null || board[x][y + 2].team != team)
                result.Add(new Vector2Int(x, y + 2));
        if (y - 2 >= 0)                                                                                 //вниз
            if (board[x][y - 2] == null || board[x][y - 2].team != team)
                result.Add(new Vector2Int(x, y - 2));

        if (y + 1 < board[x].Length && x + 2 <= 8)                                               //вверх вправо
            if (board[x + 2][y + 1] == null || board[x + 2][y + 1].team != team)
                result.Add(new Vector2Int(x + 2, y + 1));
        if (y - 1 >= 0 && x + 2 <= 8)                                                                   //вниз вправо
            if (board[x + 2][y - 1] == null || board[x + 2][y - 1].team != team)
                result.Add(new Vector2Int(x + 2, y - 1));

        if (y + 1 < board[x].Length && x - 2 >= 0)                                               //вверх влево
            if (board[x - 2][y + 1] == null || board[x - 2][y + 1].team != team)
                result.Add(new Vector2Int(x - 2, y + 1));
        if (y - 1 >= 0 && x - 2 >= 0)                                                                   //вниз влево
            if (board[x - 2][y - 1] == null || board[x - 2][y - 1].team != team)
                result.Add(new Vector2Int(x - 2, y - 1));

        return result;
    }
}
