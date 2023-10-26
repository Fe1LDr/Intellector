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
            if (i < 0) continue;                                                                        //левая граница
            if (i > 8) continue;                                                                        //правая граница

            for (int j = this.y - 1; j <= this.y + 1; j++)
            {
                if (j < 0) continue;                                                                    //нижняя граница
                if (j >= board.pieces[i].Length) continue;                                              //верхняя граница

                if (this.x == i && this.y == j) continue;                                               //клетка с фигурой
                if (this.x % 2 == 0 && this.y + 1 == j && this.x != i) continue;                        //две лишние клетки сверху
                if (this.x % 2 == 1 && this.y - 1 == j && this.x != i) continue;                        //две лишние клетки снизу

                if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team)                 //есть фигура и она союзная
                    if(board.pieces[i][j].type == PieceType.intellector)                                //эта фигура интелектор
                        return true;       
            }
        }

        return false;
    }

    abstract public List<Vector2Int> GetAvaibleMooves();
}
