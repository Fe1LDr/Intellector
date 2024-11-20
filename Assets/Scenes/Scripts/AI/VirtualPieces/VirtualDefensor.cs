using System.Collections.Generic;
using UnityEngine;

public class VirtualDefensor : VirtualPiece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0) continue;                                                                        //левая граница
            if (i > 8) continue;                                                                        //правая граница

            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0) continue;                                                                    //нижняя граница
                if (j >= board[i].Length) continue;                                              //верхняя граница

                if (x == i && y == j) continue;                                               //клетка с фигурой
                if (x % 2 == 0 && y + 1 == j && x != i) continue;                        //две лишние клетки сверху
                if (x % 2 == 1 && y - 1 == j && x != i) continue;                        //две лишние клетки снизу

                if (board[i][j] != null && board[i][j].team == team) continue;       //есть фигура и она союзная

                result.Add(new Vector2Int(i, j));
            }
        }

        return result;
    }
}
