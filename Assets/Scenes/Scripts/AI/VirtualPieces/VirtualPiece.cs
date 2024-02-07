using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualPiece
{
    public PieceType type;
    public int x;
    public int y;
    public bool team;
    public VirtualPiece[][] board;

    public bool HasIntellectorNearby()
    {
        for (int i = x - 1; i <= x + 1; i++)
        {
            if (i < 0) continue;                                                                        
            if (i > 8) continue;                                                                        

            for (int j = y - 1; j <= y + 1; j++)
            {
                if (j < 0) continue;                                                                    
                if (j >= board[i].Length) continue;                                             

                if (x == i && y == j) continue;                                               
                if (x % 2 == 0 && y + 1 == j && x != i) continue;                       
                if (x % 2 == 1 && y - 1 == j && x != i) continue;                        

                if (board[i][j] != null && board[i][j].team == team)                 
                    if (board[i][j].type == PieceType.intellector)                              
                        return true;
            }
        }

        return false;
    }

    abstract public List<Vector2Int> GetAvaibleMooves();
}
