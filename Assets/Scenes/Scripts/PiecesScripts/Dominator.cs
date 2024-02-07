using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dominator : Piece
{
    public override List<Vector2Int> GetAvaibleMooves()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        //õîäû ââåðõ
        for (int j = y + 1; j < board[x].Length; j++)
        {
            if (board[x][j] != null && board[x][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(x, j));
            if (board[x][j] != null && board[x][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        //õîäû âíèç
        for (int j = y - 1; j >= 0; j--)
        {
            if (board[x][j] != null && board[x][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(x, j));
            if (board[x][j] != null && board[x][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        //õîäû ïî äèàãîíàëè ââåðõ âïðàâî
        for (int i = x + 1, j = y; i <= 8; i++)
        {
            if (i % 2 == 0) j++;
            if (j >= board[i].Length) break;                                             //âåðõíÿÿ ãðàíèöà

            if (board[i][j] != null && board[i][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        //õîäû ïî äèàãîíàëè âíèç âïðàâî
        for (int i = x + 1, j = y; i <= 8; i++)
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //íèæíÿÿ ãðàíèöà

            if (board[i][j] != null && board[i][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        //õîäû ïî äèàãîíàëè ââåðõ âëåâî
        for (int i = x - 1, j = y; i >= 0; i--)
        {
            if (i % 2 == 0) j++;
            if (j >= board[i].Length) break;                                             //âåðõíÿÿ ãðàíèöà

            if (board[i][j] != null && board[i][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        //õîäû ïî äèàãîíàëè âíèç âëåâî
        for (int i = x - 1, j = y; i >= 0; i--) 
        {
            if (i % 2 == 1) j--;
            if (j < 0) break;                                                                   //íèæíÿÿ ãðàíèöà

            if (board[i][j] != null && board[i][j].team == team) break;      //åñòü ôèãóðà è îíà ñîþçíàÿ
            result.Add(new Vector2Int(i, j));
            if (board[i][j] != null && board[i][j].team != team) break;      //åñòü ôèãóðà è îíà âðàæåñêàÿ
        }

        return result;
    }
}
