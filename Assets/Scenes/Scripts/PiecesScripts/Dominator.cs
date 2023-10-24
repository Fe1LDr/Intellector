using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dominator : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //ходы вверх
        for (int j = this.y + 1; j <= this.x + 6; j++)
        {
            if (j >= board.pieces[x].Length) break;                                             //верхняя граница
            if (board.pieces[x][j] != null && board.pieces[x][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(this.x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != this.team) break;      //есть фигура и она вражеская
        }
        //ходы вниз
        for (int j = this.y - 1; j >= this.x - 6; j--)
        {
            if (j < 0) break;                                                                   //нижняя граница
            if (board.pieces[x][j] != null && board.pieces[x][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(this.x, j));
            if (board.pieces[x][j] != null && board.pieces[x][j].team != this.team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх вправо
        for (int i = this.x + 1, j = this.y; i <= this.x + 8; i++)
        {
            if (i % 2 == 0) j++;

            if (i > 8) break;                                                                   //правая граница
            if (j >= board.pieces[i].Length) break;                                             //верхняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //есть фигура и она вражеская
        }
        //ходы по диагонали вниз вправо
        for (int i = this.x + 1, j = this.y; i <= this.x + 8; i++)
        {
            if (i % 2 == 1) j--;

            if (i > 8) break;                                                                   //правая граница
            if (j < 0) break;                                                                   //нижняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //есть фигура и она вражеская
        }

        //ходы по диагонали вверх влево
        for (int i = this.x - 1, j = this.y; i >= this.x - 8; i--)
        {
            if (i % 2 == 0) j++;

            if (i < 0) break;                                                                   //левая граница
            if (j >= board.pieces[i].Length) break;                                             //верхняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //есть фигура и она вражеская
        }
        //ходы по диагонали вниз влево
        for (int i = this.x - 1, j = this.y; i >= this.x - 8; i--)
        {
            if (i % 2 == 1) j--;

            if (i < 0) break;                                                                   //левая граница
            if (j < 0) break;                                                                   //нижняя граница

            if (board.pieces[i][j] != null && board.pieces[i][j].team == this.team) break;      //есть фигура и она союзная

            result.Add(new Vector2Int(i, j));
            if (board.pieces[i][j] != null && board.pieces[i][j].team != this.team) break;      //есть фигура и она вражеская
        }


        return result;
    }
}
