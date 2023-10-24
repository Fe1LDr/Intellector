using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progressor : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        if (this.y + 1 < board.pieces[x].Length )           //если впереди не край доски
            if (board.pieces[x][y + 1] == null              //если там нет фигуры
                || board.pieces[x][y + 1].team != this.team)//или эта фигура вражеская
                result.Add(new Vector2Int(x, y + 1));       // то туда можно ходить
        return result;              
    }
}
