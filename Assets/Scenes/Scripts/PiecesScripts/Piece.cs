using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public int x;
    public int y;
    public bool team;
    public Board board;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public bool HasIntellectorNearby()
    {
        for (int i = this.x - 1; i <= this.x + 1; i++)
        {
            if (i < 0) continue;                                                                        
            if (i > 8) continue;                                                                        

            for (int j = this.y - 1; j <= this.y + 1; j++)
            {
                if (j < 0) continue;                                                                    
                if (j >= board.pieces[i].Length) continue;                                             

                if (this.x == i && this.y == j) continue;                                               
                if (this.x % 2 == 0 && this.y + 1 == j && this.x != i) continue;                       
                if (this.x % 2 == 1 && this.y - 1 == j && this.x != i) continue;                        

                if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team)                 
                    if (board.pieces[i][j].type == PieceType.intellector)                              
                        return true;
            }
        }

        return false;
    }

    abstract public List<Vector2Int> GetAvaibleMooves();
}
