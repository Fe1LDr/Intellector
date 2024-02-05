using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirtualBoard
{
    VirtualPiece[][] pieces;
    public int valuation;
    bool turn;

    public VirtualBoard(Piece[][] board, int valuation)
    {
        pieces = new VirtualPiece[9][];
        for (int i = 0; i < 9; i++)
        {
            pieces[i] = new VirtualPiece[7 - (i % 2)];
            for(int j = 0; j < pieces[i].Length; j++)
            {
                pieces[i][j] = MakeVirtualCopy(board[i][j]);
            }
        }

        this.valuation = valuation;
        turn = AI.AI_team;
    }

    public void MakeMove(Move move)
    {
        VirtualPiece start_piece = pieces[move.start_x][move.start_y];
        VirtualPiece end_piece = pieces[move.end_x][move.end_y];
        if (start_piece == null)
            throw new System.NullReferenceException("Ќј„јЋ№Ќјя ‘»√”–ј NULL");
        DecreaseValuation(move);

        //рокировка
        if(move.castling)
        {
            (pieces[move.start_x][move.start_y], pieces[move.end_x][move.end_y]) = (pieces[move.end_x][move.end_y], pieces[move.start_x][move.start_y]);
        }
        //любой другой ход
        else
        {
            pieces[move.end_x][move.end_y] = start_piece;
            if (move.end_type != start_piece.type)
                ChangeType(ref pieces[move.end_x][move.end_y], move.end_type);

            pieces[move.start_x][move.start_y] = null;
        }

        SynchronizeCoordinates(move);
        IncreaseValuation(move);
        turn = !turn;
    }

    public void CancelMove(Move move)
    {
        VirtualPiece start_piece = pieces[move.start_x][move.start_y];
        VirtualPiece end_piece = pieces[move.end_x][move.end_y];

        if (end_piece == null)
            throw new System.NullReferenceException(" ќЌ≈„Ќјя ‘»√”–ј NULL");

        DecreaseValuation(move);

        //рокировка
        if (move.castling)
        {
            (pieces[move.start_x][move.start_y], pieces[move.end_x][move.end_y]) = (pieces[move.end_x][move.end_y], pieces[move.start_x][move.start_y]);
        }
        //любой другой ход
        else
        {
            pieces[move.start_x][move.start_y] = end_piece;
            if (move.start_type != end_piece.type)
                ChangeType(ref pieces[move.start_x][move.start_y], move.start_type);

            pieces[move.end_x][move.end_y] = move.previous_piece;
        }
              
        SynchronizeCoordinates(move);
        IncreaseValuation(move);
        turn = !turn;
    }

    public List<Move> GetAllMoves()
    {
        List<Move> moves = new();
        foreach (VirtualPiece[] row in pieces)
            foreach (VirtualPiece piece in row)
            {
                if (piece != null && piece.team == turn)
                {
                    foreach (Vector2Int coor in piece.GetAvaibleMooves())
                    {
                        if (piece.HasIntellectorNearby())
                        {
                            moves.AddRange(Move.MoveWithIntellector(piece, coor));
                        }
                        else
                        {
                            moves.Add(new Move(piece, coor));
                        }
                    }
                }
            }
        return moves;
    }

    private void SynchronizeCoordinates(Move move)
    {
        if (pieces[move.start_x][move.start_y] != null)
        {
            pieces[move.start_x][move.start_y].x = move.start_x;
            pieces[move.start_x][move.start_y].y = move.start_y;
        }
        if(pieces[move.end_x][move.end_y] != null)
        {
            pieces[move.end_x][move.end_y].x = move.end_x; 
            pieces[move.end_x][move.end_y].y = move.end_y;
        }
        
    }

    private void DecreaseValuation(Move move)
    {
        valuation -= Evaluator.GetValue(pieces[move.start_x][move.start_y]);
        valuation -= Evaluator.GetValue(pieces[move.end_x][move.end_y]);
    }

    private void IncreaseValuation(Move move)
    {
        valuation += Evaluator.GetValue(pieces[move.start_x][move.start_y]);
        valuation += Evaluator.GetValue(pieces[move.end_x][move.end_y]);
    }

    private VirtualPiece MakePiece(PieceType type)
    {
        switch (type)
        {
            case PieceType.intellector: return new VirtualIntellector();
            case PieceType.progressor: return new VirtualProgressor();
            case PieceType.liberator: return new VirtualLiberator();
            case PieceType.dominator: return new VirtualDominator();
            case PieceType.agressor: return new VirtualAgressor();
            case PieceType.defensor: return new VirtualDefensor();
            default: throw new System.Exception("Ќеизвестный тип фигуры");
        }
    }

    private void ChangeType(ref VirtualPiece piece, PieceType new_type)
    {
        VirtualPiece new_piece = MakePiece(new_type);
        new_piece.type = new_type;
        new_piece.x = piece.x;
        new_piece.y = piece.y;
        new_piece.team = piece.team;
        new_piece.board = this.pieces;

        piece = new_piece;
    }

    private VirtualPiece MakeVirtualCopy(Piece piece)
    {
        if(piece == null ) return null;
        VirtualPiece pieceCopy = MakePiece(piece.type);
        pieceCopy.x = piece.x;
        pieceCopy.y = piece.y;
        pieceCopy.team = piece.team;
        pieceCopy.type = piece.type;
        pieceCopy.board = this.pieces;

        return pieceCopy;
    }
}
