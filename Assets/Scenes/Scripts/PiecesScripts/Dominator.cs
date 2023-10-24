using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dominator : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //���� �����
        for (int j = this.y + 1; j <= this.x + 6; j++)
        {
            if (j >= board.pieces[x].Length) break;                                             //������� �������
            if (board.pieces[x][j] != null && board.pieces[x][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(this.x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� ����
        for (int j = this.y - 1; j >= this.x - 6; j--)
        {
            if (j < 0) break;                                                                   //������ �������
            if (board.pieces[x][j] != null && board.pieces[x][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(this.x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != this.team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� ������
        for (int i = this.x + 1, j = this.y; i <= this.x + 8; i++)
        {
            if (i % 2 == 0) j++;

            if (i > 8) break;                                                                   //������ �������
            if (j >= board.pieces[i].Length) break;                                             //������� �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� �� ��������� ���� ������
        for (int i = this.x + 1, j = this.y; i <= this.x + 8; i++)
        {
            if (i % 2 == 1) j--;

            if (i > 8) break;                                                                   //������ �������
            if (j < 0) break;                                                                   //������ �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }

        //���� �� ��������� ����� �����
        for (int i = this.x - 1, j = this.y; i >= this.x - 8; i--)
        {
            if (i % 2 == 0) j++;

            if (i < 0) break;                                                                   //����� �������
            if (j >= board.pieces[i].Length) break;                                             //������� �������

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //���� ������ � ��� �������

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //���� ������ � ��� ���������
        }
        //���� �� ��������� ���� �����
        for (int i = this.x - 1, j = this.y; i >= this.x - 8; i--)
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
