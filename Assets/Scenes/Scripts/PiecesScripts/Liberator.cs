using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liberator : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //������� ����
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

                if (board.pieces[i][j] != null) continue;                                               //���� ������

                result.Add(new Vector2Int(i, j));
            }
        }

        //������� ����
        if(this.y + 2 < board.pieces[x].Length)                                                         //�����
            if (board.pieces[x][this.y + 2] == null || board.pieces[x][this.y + 2].team != this.team)
                result.Add(new Vector2Int(x, this.y + 2));
        if (this.y - 2 >= 0)                                                                            //����
            if (board.pieces[x][this.y - 2] == null || board.pieces[x][this.y - 2].team != this.team)
                result.Add(new Vector2Int(x, this.y - 2));

        if (this.y + 1 < board.pieces[x].Length && this.x + 2 <= 8)                                     //����� ������
            if (board.pieces[this.x + 2][this.y + 1] == null || board.pieces[this.x + 2][this.y + 1].team != this.team)
                result.Add(new Vector2Int(this.x + 2, this.y + 1));
        if (this.y - 1 >= 0 && this.x + 2 <= 8)                                                         //���� ������
            if (board.pieces[this.x + 2][this.y - 1] == null || board.pieces[this.x + 2][this.y - 1].team != this.team)
                result.Add(new Vector2Int(this.x + 2, this.y - 1));

        if (this.y + 1 < board.pieces[x].Length && this.x - 2 >= 0)                                     //����� �����
            if (board.pieces[this.x - 2][this.y + 1] == null || board.pieces[this.x - 2][this.y + 1].team != this.team)
                result.Add(new Vector2Int(this.x - 2, this.y + 1));
        if (this.y - 1 >= 0 && this.x - 2 >= 0)                                                         //���� �����
            if (board.pieces[this.x - 2][this.y - 1] == null || board.pieces[this.x - 2][this.y - 1].team != this.team)
                result.Add(new Vector2Int(this.x - 2, this.y - 1));

        return result;
    }
}
