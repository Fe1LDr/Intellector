using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private float tileSize;


    [Header("Materials")]
    [SerializeField] private Material WhiteTeamMaterial;
    [SerializeField] private Material BlackTeamMaterial;

    private GameObject[][] tiles;
    private Piece[][] pieces;

    List<Vector2Int> AvaibleMoves;

    private Vector2Int currentHover  = -Vector2Int.one;
    private Vector2Int currentSelect = -Vector2Int.one;

    private static float x_offset;
    private static float y_offset;

    // Start is called before the first frame update
    void Start()
    {
        x_offset = tileSize / Mathf.Sqrt(3) * 1.51f;
        y_offset = tileSize;

        GenerateAllTiles();
        GenerateAllPieces();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelect != -Vector2Int.one)
            tiles[currentSelect.x][currentSelect.y].layer = LayerMask.NameToLayer("SelectedTile");
    }
    public Vector3 TransformCoordinates(int x, int y)
        => new Vector3(x * x_offset, 0, y * y_offset + (y_offset / 2) * (x % 2));


    //—оздание пол€ и фигур
    public void GenerateAllTiles()
    {
        tiles = new GameObject[9][];
        for(int i = 0; i < 9; i++)
        {
            tiles[i] = new GameObject[7 - (i % 2)];
            for (int j = 0; j < tiles[i].Length; j++)
                tiles[i][j] = GenerateOneTile(i, j);
        }
    }
    public GameObject GenerateOneTile(int x, int y)
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        tile.name = $"tile {x} {y}";
        tile.transform.position = TransformCoordinates(x,y);
        tile.layer = LayerMask.NameToLayer("Tile");
        tile.AddComponent<BoxCollider>();
        return tile;
    }
    public void GenerateAllPieces()
    {
        pieces = new Piece[9][];
        for (int i = 0; i < 9; i++)
            pieces[i] = new Piece[7 - (i % 2)];

        pieces[0][0] = GenerateSinglePiece(PieceType.dominator, false, 0, 0);
        pieces[1][0] = GenerateSinglePiece(PieceType.liberator, false, 1, 0);
        pieces[2][0] = GenerateSinglePiece(PieceType.agressor, false, 2, 0);
        pieces[3][0] = GenerateSinglePiece(PieceType.defensor, false, 3, 0);
        pieces[4][0] = GenerateSinglePiece(PieceType.intellector, false, 4, 0);
        pieces[5][0] = GenerateSinglePiece(PieceType.defensor, false, 5, 0);
        pieces[6][0] = GenerateSinglePiece(PieceType.agressor, false, 6, 0);
        pieces[7][0] = GenerateSinglePiece(PieceType.liberator, false, 7, 0);
        pieces[8][0] = GenerateSinglePiece(PieceType.dominator, false, 8, 0);
        for(int i = 0; i < 9 ; i += 2)
            pieces[i][1] = GenerateSinglePiece(PieceType.progressor, false, i, 1);

        pieces[0][6] = GenerateSinglePiece(PieceType.dominator, true, 0, 6);
        pieces[1][5] = GenerateSinglePiece(PieceType.liberator, true, 1, 5);
        pieces[2][6] = GenerateSinglePiece(PieceType.agressor, true, 2, 6);
        pieces[3][5] = GenerateSinglePiece(PieceType.defensor, true, 3, 5);
        pieces[4][6] = GenerateSinglePiece(PieceType.intellector, true, 4, 6);
        pieces[5][5] = GenerateSinglePiece(PieceType.defensor, true, 5, 5);
        pieces[6][6] = GenerateSinglePiece(PieceType.agressor, true, 6, 6);
        pieces[7][5] = GenerateSinglePiece(PieceType.liberator, true, 7, 5);
        pieces[8][6] = GenerateSinglePiece(PieceType.dominator, true, 8, 6);
        for (int i = 0; i < 9; i += 2)
            pieces[i][5] = GenerateSinglePiece(PieceType.progressor, true, i, 5);

    }
    public Piece GenerateSinglePiece(PieceType type, bool team, int x , int y)
    {
        Piece piece;
        piece = Instantiate(piecesPrefabs[(int)type], transform).GetComponent<Piece>();
        piece.type = type;
        piece.team = team;
        piece.board = this;
        piece.transform.position = TransformCoordinates(x, y);
        piece.GetComponent<MeshRenderer>().materials = (team == true) ? new Material[]{ BlackTeamMaterial} : new Material[] { WhiteTeamMaterial };
        return piece;
    }


    //операции с пол€ми и сло€ми
    public Vector2Int LookUpTileIndex(GameObject hitInfo)
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < tiles[i].Length; j++)
                if(tiles[i][j] == hitInfo)
                    return new Vector2Int(i,j);

        throw new Exception("Ќе найден тайл на который указывал курсор");
    }
    public void HoverTile(Vector2Int coordinates)
    {
        if (currentHover == coordinates) return;

        if (currentHover != -Vector2Int.one)
             tiles[currentHover.x][currentHover.y].layer = LayerMask.NameToLayer("Tile");

        tiles[coordinates.x][coordinates.y].layer = LayerMask.NameToLayer("HoverTile");
        Debug.Log($"{coordinates.x} {coordinates.y}");
        currentHover = coordinates;
    }
    public void RemoveHover()
    {
        if (currentHover != -Vector2Int.one)
            tiles[currentHover.x][currentHover.y].layer = LayerMask.NameToLayer("Tile");
    }
    public void HighlightAvaibleTiles(List<Vector2Int> coordinates)
    {
        foreach (Vector2Int tile in coordinates)
            tiles[tile.x][tile.y].layer = LayerMask.NameToLayer("Avaible");
    }
    public void RemoveHighlight()
    {
        foreach (GameObject[] x_tiles in tiles)
            foreach (GameObject tile in x_tiles)
                tile.layer = LayerMask.NameToLayer("Tile");
    }


    //перемещение фигур
    public void SelectTile(Vector2Int coordinates)
    {
        //если не выбрана никака€ фигура
        if (currentSelect == -Vector2Int.one)
            if(pieces[coordinates.x][coordinates.y] != null) //если нажали на фигуру выбираем еЄ
            {
                currentSelect = coordinates;
                try
                {
                    AvaibleMoves = pieces[coordinates.x][coordinates.y].GetAvaibleMooves();
                }
                catch (NotImplementedException)
                {
                    AvaibleMoves = new List<Vector2Int>();
                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < tiles[i].Length; j++)
                            if(pieces[i][j] == null || pieces[i][j].team != pieces[currentSelect.x][currentSelect.y].team)
                                AvaibleMoves.Add(new Vector2Int(i, j));
                }
            
                HighlightAvaibleTiles(AvaibleMoves);
                return;
            }
            else { return; }

        //если уже выбрана
        else
        {
            if (currentSelect == coordinates) //если нажали на ту же фигуру сбрасываем выделение
            {
                currentSelect = -Vector2Int.one;
                RemoveHighlight();
            }
            else //а если нажали на другое поле
            {
                if (CanMove()) // и туда можно пойти, то идЄм туда
                {
                    MovePiece(currentSelect, coordinates);
                    //событие

                    // и сбрасываем выделение
                    currentSelect = -Vector2Int.one;
                    RemoveHighlight();
                }
            }
        }

        bool CanMove()
        {
            bool avaible = false;
            if (AvaibleMoves != null)
                foreach (Vector2Int move in AvaibleMoves)
                    if (coordinates == move)
                        avaible = true;
            return avaible;
        }
    }
    public void MovePiece(Vector2Int start, Vector2Int end)
    {
        //перемещение в пространстве
        pieces[start.x][start.y].transform.position = TransformCoordinates(end.x, end.y);

        //едим если зан€то вражеской фигурой
        if(pieces[end.x][end.y] != null && (pieces[end.x][end.y].team != pieces[start.x][start.y].team))
            Destroy(pieces[end.x][end.y].GetComponent<MeshRenderer>());

        //на свои фигуры пока не ходим
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].team == pieces[start.x][start.y].team))
            throw new InvalidOperationException("Ќевозможный ход: ход на свою фигуру");

        //изменение ссылок
        pieces[end.x][end.y] = pieces[start.x][start.y];
        pieces[start.x][start.y] = null;
    }
}
