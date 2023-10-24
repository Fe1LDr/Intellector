using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progressor : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        if (this.y + 1 < board.pieces[x].Length )           //���� ������� �� ���� �����
            if (board.pieces[x][y + 1] == null              //���� ��� ��� ������
                || board.pieces[x][y + 1].team != this.team)//��� ��� ������ ���������
                result.Add(new Vector2Int(x, y + 1));       // �� ���� ����� ������
        return result;              
    }
}
