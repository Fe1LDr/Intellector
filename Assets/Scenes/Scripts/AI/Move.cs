using System;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public int start_x;
    public int start_y;
    public PieceType start_type;

    public int end_x;
    public int end_y;
    public PieceType end_type;

    public VirtualPiece previous_piece;//опасно!
    public bool castling;
    public bool taking;

    public Move(VirtualPiece piece, Vector2Int coor)
    {
        start_x = piece.x;
        start_y = piece.y;
        start_type = piece.type;
        end_x = coor.x;
        end_y = coor.y;
        end_type = piece.type;

        VirtualPiece start_piece = piece.board[start_x][start_y];
        VirtualPiece end_piece = piece.board[end_x][end_y];
        castling = (start_piece != null && start_piece.type == PieceType.intellector && end_piece != null && end_piece.type == PieceType.defensor && start_piece.team == end_piece.team);

        taking = (end_piece != null) && (!castling);

        if (taking) previous_piece = end_piece;
        else previous_piece = null;
    }

    public static List<Move> MoveWithIntellector(VirtualPiece piece, Vector2Int coor)
    {
        List<Move> moves = new();
        Move standard = new Move(piece, coor);
        moves.Add(standard);

        if (standard.taking)
        {
            moves.Add(new Move(standard, piece.board[coor.x][coor.y].type));
        }

        return moves;
    }

    public List<Move> ProgressorMoveWithTransform()
    {
        throw new NotImplementedException();
    }

    private Move(Move standard, PieceType taken_type)
    {
        start_x = standard.start_x;
        start_y = standard.start_y;
        start_type = standard.start_type;
        end_x = standard.end_x;
        end_y = standard.end_y;
        end_type = taken_type;
        castling = standard.castling;
        taking = standard.taking;
        previous_piece = standard.previous_piece;
    }
}
