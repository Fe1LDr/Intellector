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
    private Vector2Int? currentHover = null;

    private static float x_offset;
    private static float y_offset;

    // Start is called before the first frame update
    void Start()
    {
        x_offset = tileSize / Mathf.Sqrt(3) * 1.5f;
        y_offset = tileSize;

        

        GenerateAllTiles();
        GenerateAllPieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        for(int i = 0; i < 8 ; i += 2)
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
        for (int i = 0; i < 8; i += 2)
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


    public Vector3 TransformCoordinates(int x, int y)
        => new Vector3(x * x_offset, 0, y * y_offset + (y_offset / 2) * (x % 2));



    public Vector2Int LookUpTileIndex(GameObject hitInfo)
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < tiles[i].Length; j++)
                if(tiles[i][j] == hitInfo)
                    return new Vector2Int(i,j);

        throw new Exception("Не найден тайл на который указывал курсор");
    }
    public void HoverTile(Vector2Int coordinates)
    {
        if (currentHover == coordinates) return;

        if (currentHover != null)
            tiles[currentHover.Value.x][currentHover.Value.y].layer = LayerMask.NameToLayer("Tile");

        tiles[coordinates.x][coordinates.y].layer = LayerMask.NameToLayer("HoverTile");
        Debug.Log($"{coordinates.x} {coordinates.y}");
        currentHover = coordinates;
    }
    internal void RemoveHover()
    {
        if (currentHover != null)
            tiles[currentHover.Value.x][currentHover.Value.y].layer = LayerMask.NameToLayer("Tile");
    }


}
