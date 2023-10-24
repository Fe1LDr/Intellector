using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defensor: Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = this.x - 1; i <= this.x + 1; i++)
        {
            if (i < 0) continue;                                                                        //����� �������
            if (i > 8) continue;                                                                        //������ �������

            for (int j = this.y - 1; j <= this.y + 1; j++)
            {
                if (j < 0) continue;                                                                    //������ �������
                if (j >= board.pieces[i].Length) continue;                                              //������� �������

                if (this.x == i && this.y == j) continue;                                               //������ � �������
                if (this.x % 2 == 0 && this.y + 1 == j && this.x != i) continue;                        //��� ������ ������ ������
                if (this.x % 2 == 1 && this.y - 1 == j && this.x != i) continue;                        //��� ������ ������ �����

                if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) continue;       //���� ������ � ��� �������

                result.Add(new Vector2Int(i, j));
            }
        }

        return result;
    }
}
