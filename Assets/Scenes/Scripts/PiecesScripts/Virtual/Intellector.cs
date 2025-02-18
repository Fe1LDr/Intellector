using System.Collections.Generic;
using UnityEngine;

public class Intellector : Piece
{
    public override PieceType Type => PieceType.intellector;
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = X - 1; i <= X + 1; i++)
        {
            if (i < 0) continue;                                                                                //����� �������
            if (i > 8) continue;                                                                                //������ �������

            for (int j = Y - 1; j <= Y + 1; j++)
            {
                if (j < 0) continue;                                                                            //������ �������
                if (j >= Board[i].Length) continue;                                                      //������� �������

                if (X == i && Y == j) continue;                                                       //������ � �������
                if (X % 2 == 0 && Y + 1 == j && X != i) continue;                                //��� ������ ������ ������
                if (X % 2 == 1 && Y - 1 == j && X != i) continue;                                //��� ������ ������ �����

                if (Board[i][j] != null)                                                                 //���� ������
                    if (Board[i][j].Team != Team || Board[i][j].Type != PieceType.defensor)  //�� �������� ����� �������
                        continue;

                result.Add(new Vector2Int(i, j));
            }
        }

        return result;
    }
}
