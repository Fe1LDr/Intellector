using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaluator
{
    private static Dictionary<PieceType, int> PieceValues = new() {
        { PieceType.progressor, 10 },
        { PieceType.liberator, 30},
        { PieceType.defensor, 20},
        { PieceType.agressor, 40 },
        { PieceType.dominator, 50 },
        { PieceType.intellector, 1000} };

    private static int[][] default_positions = new int[][]
    {
        new int [] {0,0,0,0,0,0,0,0,0 },
        new int [] { 0,0,1,2,2,1,0,0 },
        new int [] {0,1,1,2,3,2,1,1,0 },
        new int [] { 2,3,4,5,4,3,2},
        new int [] {1,2,3,4,5,4,3,2,1 },
        new int [] { 2,3,4,5,4,3,2},
        new int [] {0,1,1,2,3,2,1,1,0 },
        new int [] { 0,0,1,2,2,1,0,0 },
        new int [] {0,0,0,0,0,0,0,0,0 }
    };

    private static int[][] white_intellector_positions = new int[][]
    {
        new int [] {4,2,0,0,0,0,0,0,0 },
        new int [] { 2,0,0,0,0,0,0,0 },
        new int [] {4,2,0,0,0,0,0,0,0 },
        new int [] { 2,0,0,0,0,0,0,0 },
        new int [] {4,2,0,0,0,0,0,0,0 },
        new int [] { 2,0,0,0,0,0,0,0 },
        new int [] {4,2,0,0,0,0,0,0,0 },
        new int [] { 2,0,0,0,0,0,0,0 },
        new int [] {4,2,0,0,0,0,0,0,0 }
    };

    private static int[][] black_intellector_positions = new int[][]
    {
        new int [] {0,0,0,0,0,0,0,2,4 },
        new int [] { 0,0,0,0,0,0,0,2 },
        new int [] {0,0,0,0,0,0,0,2,4 },
        new int [] { 0,0,0,0,0,0,0,2 },
        new int [] {0,0,0,0,0,0,0,2,4 },
        new int [] { 0,0,0,0,0,0,0,2 },
        new int [] {0,0,0,0,0,0,0,2,4 },
        new int [] { 0,0,0,0,0,0,0,2 },
        new int [] {0,0,0,0,0,0,0,2,4 },
    };

    private static int[][] white_progressor_positions = new int[][]
    {
        new int [] {0,0,0,1,2,3,4,6,8 },
        new int [] { 0,0,0,1,2,3,4,6 },
        new int [] {0,0,0,1,2,3,4,6,8 },
        new int [] { 0,0,0,1,2,3,4,6 },
        new int [] {0,0,0,1,2,3,4,6,8 },
        new int [] { 0,0,0,1,2,3,4,6 },
        new int [] {0,0,0,1,2,3,4,6,8 },
        new int [] { 0,0,0,1,2,3,4,6 },
        new int [] {0,0,0,1,2,3,4,6,8 },
    };

    private static int[][] black_progressor_positions = new int[][]
{
        new int [] {8,6,4,3,2,1,0,0,0 },
        new int [] { 6,4,3,2,1,0,0,0 },
        new int [] {8,6,4,3,2,1,0,0,0 },
        new int [] { 6,4,3,2,1,0,0,0 },
        new int [] {8,6,4,3,2,1,0,0,0 },
        new int [] { 6,4,3,2,1,0,0,0 },
        new int [] {8,6,4,3,2,1,0,0,0 },
        new int [] { 6,4,3,2,1,0,0,0 },
        new int [] {8,6,4,3,2,1,0,0,0 },
};

    private static int GetPositionValue(VirtualPiece piece)
    {
        switch (piece.type)
        {
            case PieceType.intellector: return piece.team ? black_intellector_positions[piece.x][piece.y] : white_intellector_positions[piece.x][piece.y];
            case PieceType.progressor: return piece.team? black_progressor_positions[piece.x][piece.y] : white_progressor_positions[piece.x][piece .y];
            default: return default_positions[piece.x][piece.y];
        }
    }

    public static int GetValue(VirtualPiece piece)
    {
        if (piece == null) return 0;
        if (piece.type == PieceType.intellector) return (piece.team == false) ? 1000 : -10000;
        return (piece.team == false) ? PieceValues[piece.type] + GetPositionValue(piece) : -PieceValues[piece.type] - GetPositionValue(piece);
    }



}
