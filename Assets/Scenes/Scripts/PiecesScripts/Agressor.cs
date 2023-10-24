using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agressor : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //���� ������
        for (int i = this.x + 2; i <= this.x + 8; i += 2)
        {
            if (i > 8) break;                                                                   //������ �������
            if (board.pieces[i][y] != null && board.pieces[i][y].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, y));
            if (board.pieces[i][y] != null && board.pieces[i][y].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� �����
        for (int i = this.x - 2; i >= this.x - 8; i -= 2)
        {
            if (i < 0) break;                                                                   //������ �������
            if (board.pieces[i][y] != null && board.pieces[i][y].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, y));
            if (board.pieces[i][y] != null && board.pieces[i][y].team != this.team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� ������
        for (int i = this.x + 1, j = this.y + 1; i <= this.x + 8; i++, j++)
        {
            if (i % 2 == 0) j++;

            if (i > 8) break;                                                                   //������ �������
            if (j >= board.pieces[i].Length) break;                                             //������� �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� �� ��������� ���� ������
        for (int i = this.x + 1, j = this.y - 1; i <= this.x + 8; i++, j--)
        {
            if (i % 2 == 1) j--;

            if (i > 8) break;                                                                   //������ �������
            if (j < 0) break;                                                                   //������ �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� �����
        for (int i = this.x - 1, j = this.y + 1; i >= this.x - 8; i--, j++)
        {
            if (i % 2 == 0) j++;

            if (i < 0) break;                                                                   //����� �������
            if (j >= board.pieces[i].Length) break;                                             //������� �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� �� ��������� ���� �����
        for (int i = this.x - 1, j = this.y - 1; i >= this.x - 8; i--, j--)
        {
            if (i % 2 == 1) j--;

            if (i < 0) break;                                                                   //����� �������
            if (j < 0) break;                                                                   //������ �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }

        return result;
    }
}
