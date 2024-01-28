using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intellector : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0) continue;                                                                                //����� �������
            if (i > 8) continue;                                                                                //������ �������

            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0) continue;                                                                            //������ �������
                if (j >= board.pieces[i].Length) continue;                                                      //������� �������

                if (x == i && y == j) continue;                                                       //������ � �������
                if (x % 2 == 0 && y + 1 == j && x != i) continue;                                //��� ������ ������ ������
                if (x % 2 == 1 && y - 1 == j && x != i) continue;                                //��� ������ ������ �����

                if (board.pieces[i][j] != null)                                                                 //���� ������
                    if (board.pieces[i][j].team != team || board.pieces[i][j].type != PieceType.defensor)  //�� �������� ����� �������
                        continue;

                result.Add(new Vector2Int(i, j));
            }
        }

        return result;
    }
}
