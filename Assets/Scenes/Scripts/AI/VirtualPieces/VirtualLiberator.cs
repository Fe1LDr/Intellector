using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualLiberator : VirtualPiece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //������� ����
        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0) continue;                                                                        //����� �������
            if (i > 8) continue;                                                                        //������ �������

            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0) continue;                                                                    //������ �������
                if (j >= board[i].Length) continue;                                              //������� �������

                if (x == i && y == j) continue;                                                         //������ � �������
                if (x % 2 == 0 && y + 1 == j && x != i) continue;                                       //��� ������ ������ ������
                if (x % 2 == 1 && y - 1 == j && x != i) continue;                                       //��� ������ ������ �����

                if (board[i][j] != null) continue;                                               //���� ������

                result.Add(new Vector2Int(i, j));
            }
        }

        //������� ����
        if(y + 2 < board[x].Length)                                                              //�����
            if (board[x][y + 2] == null || board[x][y + 2].team != team)
                result.Add(new Vector2Int(x, y + 2));
        if (y - 2 >= 0)                                                                                 //����
            if (board[x][y - 2] == null || board[x][y - 2].team != team)
                result.Add(new Vector2Int(x, y - 2));

        if (y + 1 < board[x].Length && x + 2 <= 8)                                               //����� ������
            if (board[x + 2][y + 1] == null || board[x + 2][y + 1].team != team)
                result.Add(new Vector2Int(x + 2, y + 1));
        if (y - 1 >= 0 && x + 2 <= 8)                                                                   //���� ������
            if (board[x + 2][y - 1] == null || board[x + 2][y - 1].team != team)
                result.Add(new Vector2Int(x + 2, y - 1));

        if (y + 1 < board[x].Length && x - 2 >= 0)                                               //����� �����
            if (board[x - 2][y + 1] == null || board[x - 2][y + 1].team != team)
                result.Add(new Vector2Int(x - 2, y + 1));
        if (y - 1 >= 0 && x - 2 >= 0)                                                                   //���� �����
            if (board[x - 2][y - 1] == null || board[x - 2][y - 1].team != team)
                result.Add(new Vector2Int(x - 2, y - 1));

        return result;
    }
}
