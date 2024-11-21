using System.Collections.Generic;
using UnityEngine;

public class Liberator : Piece
{
    public override PieceType Type => PieceType.liberator;
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //������� ����
        for (int i = X - 1; i <= X + 1; i++)
        {
            if (i < 0) continue;                                                                        //����� �������
            if (i > 8) continue;                                                                        //������ �������

            for (int j = Y - 1; j <= Y + 1; j++)
            {
                if (j < 0) continue;                                                                    //������ �������
                if (j >= Board[i].Length) continue;                                              //������� �������

                if (X == i && Y == j) continue;                                                         //������ � �������
                if (X % 2 == 0 && Y + 1 == j && X != i) continue;                                       //��� ������ ������ ������
                if (X % 2 == 1 && Y - 1 == j && X != i) continue;                                       //��� ������ ������ �����

                if (Board[i][j] != null) continue;                                               //���� ������

                result.Add(new Vector2Int(i, j));
            }
        }

        //������� ����
        if(Y + 2 < Board[X].Length)                                                              //�����
            if (Board[X][Y + 2] == null || Board[X][Y + 2].Team != Team)
                result.Add(new Vector2Int(X, Y + 2));
        if (Y - 2 >= 0)                                                                                 //����
            if (Board[X][Y - 2] == null || Board[X][Y - 2].Team != Team)
                result.Add(new Vector2Int(X, Y - 2));

        if (Y + 1 < Board[X].Length && X + 2 <= 8)                                               //����� ������
            if (Board[X + 2][Y + 1] == null || Board[X + 2][Y + 1].Team != Team)
                result.Add(new Vector2Int(X + 2, Y + 1));
        if (Y - 1 >= 0 && X + 2 <= 8)                                                                   //���� ������
            if (Board[X + 2][Y - 1] == null || Board[X + 2][Y - 1].Team != Team)
                result.Add(new Vector2Int(X + 2, Y - 1));

        if (Y + 1 < Board[X].Length && X - 2 >= 0)                                               //����� �����
            if (Board[X - 2][Y + 1] == null || Board[X - 2][Y + 1].Team != Team)
                result.Add(new Vector2Int(X - 2, Y + 1));
        if (Y - 1 >= 0 && X - 2 >= 0)                                                                   //���� �����
            if (Board[X - 2][Y - 1] == null || Board[X - 2][Y - 1].Team != Team)
                result.Add(new Vector2Int(X - 2, Y - 1));

        return result;
    }
}
