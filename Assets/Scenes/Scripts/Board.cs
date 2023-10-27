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

    [Header("Network")]
    [SerializeField] public bool NetworkGame;
    [SerializeField] public bool PlayerTeam;

    public delegate void MoveDelegate(Vector2Int start, Vector2Int end);
    public event MoveDelegate MoveEvent;

    [NonSerialized] public bool Turn;
    [NonSerialized] public bool game_over;
    public Piece[][] pieces;
    private GameObject[][] tiles;

    private List<Vector2Int> AvaibleMoves;

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

        Turn = false;
        game_over = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Vector2Int coor = new Vector2Int(i, j);
                if (coor == currentSelect) tiles[coor.x][coor.y].layer = LayerMask.NameToLayer("SelectedTile");
                else if(AvaibleMoves != null && AvaibleMoves.Contains(coor))
                {
                    if(coor == currentHover)
                        tiles[coor.x][coor.y].layer = LayerMask.NameToLayer("HoverAvaible");
                    else
                        tiles[coor.x][coor.y].layer = LayerMask.NameToLayer("Avaible");
                }
                else if(coor == currentHover)
                    tiles[coor.x][coor.y].layer = LayerMask.NameToLayer("HoverTile");
                else
                    tiles[coor.x][coor.y].layer = LayerMask.NameToLayer("Tile");

            }
    }
    public Vector3 TransformCoordinates(int x, int y)
        => new Vector3(x * x_offset, 0, y * y_offset + (y_offset / 2) * (x % 2));


    //�������� ���� � �����
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
        piece.x = x;
        piece.y = y;
        piece.transform.position = TransformCoordinates(x, y);
        if((type == PieceType.agressor) && (team == true))
        {
            piece.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        piece.GetComponent<MeshRenderer>().materials = (team == true) ? new Material[]{ BlackTeamMaterial} : new Material[] { WhiteTeamMaterial };
        return piece;
    }


    //�������� � ������ � ������
    public Vector2Int LookUpTileIndex(GameObject hitInfo)
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < tiles[i].Length; j++)
                if(tiles[i][j] == hitInfo)
                    return new Vector2Int(i,j);

        throw new Exception("�� ������ ���� �� ������� �������� ������");
    }
    public void HoverTile(Vector2Int coordinates)
    {
        //Debug.Log($"{coordinates.x} {coordinates.y}");
        currentHover = coordinates;
    }
    public void RemoveHover()
    {
        currentHover = -Vector2Int.one;
    }

    //����������� �����
    public void SelectTile(Vector2Int coordinates)
    {
        if (NetworkGame && (PlayerTeam != Turn)) return; //�� ������� ����� ������

        //���� �� ������� ������� ������
        if (currentSelect == -Vector2Int.one)
            if((pieces[coordinates.x][coordinates.y] != null) && (pieces[coordinates.x][coordinates.y].team == Turn)) //���� ������ �� ������ �������� �
            {
                currentSelect = coordinates;
                AvaibleMoves = pieces[coordinates.x][coordinates.y].GetAvaibleMooves();
                return;
            }
            else { return; }

        //���� ��� �������
        else
        {
            if (currentSelect == coordinates) //���� ������ �� �� �� ������ ���������� ���������
            {
                currentSelect = -Vector2Int.one;
                AvaibleMoves = null;
            }
            else //� ���� ������ �� ������ ����
            {
                if (AvaibleMoves.Contains(coordinates)) // � ���� ����� �����, �� ��� ����
                {
                    MovePiece(currentSelect, coordinates);
                    // � ���������� ���������
                    currentSelect = -Vector2Int.one;
                    AvaibleMoves = null;
                }
            }
        }
    }
    public void MovePiece(Vector2Int start, Vector2Int end)
    {
        //�������� ���������� ����
        if (pieces[start.x][start.y].team != Turn) return;

        //����������� � ������������
        pieces[start.x][start.y].transform.position = TransformCoordinates(end.x, end.y);

        //������������ ���������� ����
        Turn = !Turn;

        //���� ���� ������ ��������� �������
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].team != pieces[start.x][start.y].team))
        {
            Destroy(pieces[end.x][end.y].GetComponent<MeshRenderer>());
            if(pieces[end.x][end.y].type == PieceType.intellector)
            {
                GameOver(pieces[start.x][start.y].team);
            }

            if (pieces[start.x][start.y].HasIntellectorNearby() && !game_over) //���� ����� ���� ���� �����������
            {   // �� ����� ������������ � ��������� ������
                bool transformation;

                try
                { transformation = AskForTransformation();}
                catch(NotImplementedException)
                { transformation = true; }

                if (transformation) TransformToEaten();
            }
        }

        //��� ���� �� ���� ������
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].team == pieces[start.x][start.y].team))
        {
            if ( //���� ��� �������� � �����������
                (pieces[start.x][start.y].type == PieceType.intellector && pieces[end.x][end.y].type == PieceType.defensor) ||
                (pieces[start.x][start.y].type == PieceType.defensor && pieces[end.x][end.y].type == PieceType.intellector)
               )
            {   //�� ������ �� �������
                Castling();
            }
            else
                throw new InvalidOperationException("����������� ���: ��� �� ���� ������");
        }

        else
        {
            //��������� ������
            pieces[end.x][end.y] = pieces[start.x][start.y];
            pieces[end.x][end.y].x = end.x;
            pieces[end.x][end.y].y = end.y;
            pieces[start.x][start.y] = null;
        }

        if((pieces[end.x][end.y].type == PieceType.progressor) &&  // ���� ����� ����������
            ((pieces[end.x][end.y].team == false && (end.y == 6)) || (pieces[end.x][end.y].team == true && (end.y == 0) && (end.x%2 == 0)))) //� �� ����� �� ���� �����������
        {
            ProgressorTransformation(end.x, end.y, pieces[end.x][end.y].team); // �� ���������� ���
        }


        MoveEvent?.Invoke(start , end);

        //"���������"
        void Castling()
        {
            pieces[end.x][end.y].transform.position = TransformCoordinates(start.x, start.y); 
            (pieces[start.x][start.y], pieces[end.x][end.y]) = (pieces[end.x][end.y], pieces[start.x][start.y]);
            pieces[start.x][start.y].x = start.x;
            pieces[start.x][start.y].y = start.y;
            pieces[end.x][end.y].x = end.x;
            pieces[end.x][end.y].y = end.y;
        }

        //����������� �����������
        void ProgressorTransformation(int x, int y, bool team)
        {
            PieceType new_type;
            try
            {
                new_type = AskForPieceType();
            }
            catch (NotImplementedException)
            {
                new_type = PieceType.dominator;
            }
            Destroy(pieces[x][y].GetComponent<MeshRenderer>());
            pieces[x][y] = GenerateSinglePiece(new_type, team, x, y);
        }    
         
        //����������� � ��������� ������
        void TransformToEaten()
        {
            PieceType new_type = pieces[end.x][end.y].type;
            Destroy(pieces[start.x][start.y].GetComponent<MeshRenderer>());
            pieces[start.x][start.y] = GenerateSinglePiece(new_type, pieces[start.x][start.y].team, end.x, end.y);
        }
    }


    //��������� � UI
    PieceType AskForPieceType()
    {
        throw new NotImplementedException();
    }

    bool AskForTransformation()
    {
        throw new NotImplementedException();
    }

    void GameOver(bool winner)
    {
        Debug.Log((winner) ? "�������� �� �����" : "�������� �����");
        AvaibleMoves = null;
        currentHover = -Vector2Int.one;
        currentSelect = -Vector2Int.one;
        game_over = true;
    }
}
