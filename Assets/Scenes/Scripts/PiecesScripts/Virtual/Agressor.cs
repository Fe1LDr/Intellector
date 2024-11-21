using System.Collections.Generic;
using UnityEngine;

public class Agressor : Piece
{
    public override PieceType Type => PieceType.agressor;
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //���� ������
        for (int i = X + 2; i <= 8; i += 2)
        {
            if (Board[i][Y] != null && Board[i][Y].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, Y));
            if (Board[i][Y] != null && Board[i][Y].Team != Team) break;      //���� ������ � ��� ���������
        }

        //���� �����
        for (int i = X - 2; i >= 0; i -= 2)
        {
            if (Board[i][Y] != null && Board[i][Y].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, Y));
            if (Board[i][Y] != null && Board[i][Y].Team != Team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� ������
        for (int i = X + 1, j = Y + 1; i <= 8; i++, j++)
        {
            if (i % 2 == 0) j++;
            if (j >= Board[i].Length) break;                                             //������� �������

            if (Board[i][j] != null && Board[i][j].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, j));
            if (Board[i][j] != null && Board[i][j].Team != Team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ���� ������
        for (int i = X + 1, j = Y - 1; i <= 8; i++, j--)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //������ �������

            if (Board[i][j] != null && Board[i][j].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, j));
            if (Board[i][j] != null && Board[i][j].Team != Team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� �����
        for (int i = X - 1, j = Y + 1; i >= 0; i--, j++)
        {
            if (i % 2 == 0) j++;
            if (j >= Board[i].Length) break;                                             //������� �������

            if (Board[i][j] != null && Board[i][j].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, j));
            if (Board[i][j] != null && Board[i][j].Team != Team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ���� �����
        for (int i = X - 1, j = Y - 1; i >= 0; i--, j--)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //������ �������

            if (Board[i][j] != null && Board[i][j].Team == Team) break;      //���� ������ � ��� �������
            result.Add(new Vector2Int(i, j));
            if (Board[i][j] != null && Board[i][j].Team != Team) break;      //���� ������ � ��� ���������
        }

        return result;
    }
}
