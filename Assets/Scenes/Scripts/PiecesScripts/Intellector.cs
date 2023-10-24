using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intellector : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = this.x - 1; i <= this.x + 1; i++)
        {
            if (i < 0) continue;                                                                                //левая граница
            if (i > 8) continue;                                                                                //правая граница

            for (int j = this.y - 1; j <= this.y + 1; j++)
            {
                if (j < 0) continue;                                                                            //нижняя граница
                if (j >= board.pieces[i].Length) continue;                                                      //верхняя граница

                if (this.x == i && this.y == j) continue;                                                       //клетка с фигурой
                if (this.x % 2 == 0 && this.y + 1 == j && this.x != i) continue;                                //две лишние клетки сверху
                if (this.x % 2 == 1 && this.y - 1 == j && this.x != i) continue;                                //две лишние клетки снизу

                if (board.pieces[i][j] != null)                                                                 //есть фигура
                    if (board.pieces[i][j].team != this.team || board.pieces[i][j].type != PieceType.defensor)  //не дефенсор своей команды
                        continue;

                result.Add(new Vector2Int(i, j));
            }
        }

        return result;
    }
}
