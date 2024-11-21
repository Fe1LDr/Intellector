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
    [SerializeField] private MaterialSelector materialSelector;
    private Material WhiteTeamMaterial;
    private Material BlackTeamMaterial;


    [Header("UI")]
    [SerializeField] GameObject Progressor_end;
    [SerializeField] GameObject Around_Intellector;
    [SerializeField] EndGame EndGame;

    [NonSerialized] public bool NetworkGame;
    [NonSerialized] public bool PlayerTeam;

    public delegate void MoveDelegate(Vector2Int start, Vector2Int end, int transform_info);
    public event MoveDelegate MoveEvent;
    public event Action RestartEvent;
    public event Action EndGameEvent;

    [NonSerialized] public bool Turn;
    [NonSerialized] public bool game_over;
    [NonSerialized] public bool wait_for_transformation;
    public IPiece[][] pieces;
    private readonly Dictionary<IPiece, GameObject> piecesObjects = new();
    public GameObject[][] tiles;

    private List<Vector2Int> AvaibleMoves;

    private Vector2Int currentHover  = -Vector2Int.one;
    private Vector2Int currentSelect = -Vector2Int.one;
    private Vector2Int lustMove1 = -Vector2Int.one;
    private Vector2Int lustMove2 = -Vector2Int.one;

    private static float x_offset;
    private static float y_offset;

    public void Awake()
    {
        x_offset = tileSize / Mathf.Sqrt(3) * 1.51f;
        y_offset = tileSize;

        Settings settings = Settings.Load();
        PlayerTeam = GameInfo.Load().Team;
        LogWriter.WriteLog($"����������� ����: {PlayerTeam}");
        NetworkGame = settings.GameMode == GameMode.Network;
        (WhiteTeamMaterial, BlackTeamMaterial) = materialSelector.GetCurrentMaterials(settings.Material);

        GenerateAllTiles();
        GenerateAllPieces();

        Turn = false;
        game_over = false;
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Vector2Int coor = new Vector2Int(i, j);
                string layer;
                if (coor == currentSelect)
                {
                    if (coor == currentHover) layer = "HoverSelected";
                    else layer = "SelectedTile";
                }
                else if(AvaibleMoves != null && AvaibleMoves.Contains(coor))
                {
                    if (coor == currentHover)   layer = "HoverAvaible";
                    else layer = "Avaible";
                }
                else if(coor == currentHover)
                    layer = "HoverTile";
                else if (coor == lustMove1 || coor == lustMove2)
                    layer = "SelectedTile";
                else
                    layer = "Tile";

                tiles[coor.x][coor.y].layer = LayerMask.NameToLayer(layer);
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
        pieces = new IPiece[9][];
        for (int i = 0; i < 9; i++)
            pieces[i] = new IPiece[7 - (i % 2)];

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
    public IPiece GenerateSinglePiece(PieceType type, bool team, int x , int y)
    {
        GameObject gameObject = Instantiate(piecesPrefabs[(int)type], transform);
        IPiece piece = gameObject.GetComponent<IPiece>();
        piecesObjects[piece] = gameObject;
        piece.Team = team;
        piece.Board = pieces;
        piece.X = x;
        piece.Y = y;

        gameObject.transform.position = TransformCoordinates(x, y);
        if((type == PieceType.agressor) && (team == true))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        gameObject.GetComponent<MeshRenderer>().materials = (team == true) ? new Material[]{ BlackTeamMaterial} : new Material[] { WhiteTeamMaterial };
        return piece;
    }

    //�������
    public void Restart()
    {
        DeleteAllPieces();
        GenerateAllPieces();
        
        PlayerTeam = !PlayerTeam;
        Turn = false;
        game_over = false;
        EndGame.Hide();
        RestartEvent?.Invoke();
    }
    void DeleteAllPieces()
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < pieces[i].Length; j++)
                if (pieces[i][j] != null)
                {
                    Destroy(piecesObjects[pieces[i][j]].GetComponent<MeshRenderer>());
                    pieces[i][j] = null;
                }
    }
    void DeleteAllHighlights()
    {
        AvaibleMoves = null;
        currentHover = -Vector2Int.one;
        currentSelect = -Vector2Int.one;
        lustMove1 = -Vector2Int.one;
        lustMove2 = -Vector2Int.one;
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
            if((pieces[coordinates.x][coordinates.y] != null) && (pieces[coordinates.x][coordinates.y].Team == Turn)) //���� ������ �� ������ �������� �
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
                    // � ���������� ���������
                    var select_buff = currentSelect;
                    currentSelect = -Vector2Int.one;
                    AvaibleMoves = null;

                    if(!ChekAndAskForTransformaton(select_buff, coordinates)) //���� ��� ������� ����������� �� ����� ������ �����
                        MovePiece(select_buff, coordinates, false);
                }
            }
        }
    }

    bool ChekAndAskForTransformaton(Vector2Int start, Vector2Int end)
    {
        if ((pieces[start.x][start.y].Type == PieceType.progressor) && // ���� ����� ����������
        ((pieces[start.x][start.y].Team == false && (end.y == 6)) || (pieces[start.x][start.y].Team == true && (end.y == 0) && (end.x % 2 == 0))) //� �� ����� �� ���� �����������
        && ((pieces[end.x][end.y] == null) || (pieces[end.x][end.y].Type != PieceType.intellector))) //� �� �� ����� ������������
        {
            Progressor_end.SetActive(true);
            StartCoroutine(WaitForPieceType(start, end));
            wait_for_transformation = true;
            return true;
        }

        else if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].Team != pieces[start.x][start.y].Team) //���� ���� ��������� ������
            && (pieces[start.x][start.y].HasIntellectorNearby()) //� ����� ���� �����������
            && (pieces[start.x][start.y].Type!=PieceType.progressor) //� ����� �� ����������
            && (pieces[end.x][end.y].Type != PieceType.intellector)) //� �� ����� �� ������������
        {
            // �� ����� ������������ � ��������� ������
            Around_Intellector.SetActive(true);
            StartCoroutine(WaitForTransformation(start, end));
            wait_for_transformation = true;
            return true;
        }

        return false;
    }

    public void MovePiece(Vector2Int start, Vector2Int end, bool received_move, int transform_info = 200)
    {
        //�������� ���������� ����
        if (pieces[start.x][start.y].Team != Turn) return;

        //������������ ���������� ����
        Turn = !Turn;

        //���������� ���������� ����
        lustMove1 = start;
        lustMove2 = end;

        //���� ���� ������ ��������� �������
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].Team != pieces[start.x][start.y].Team))
        {         
            Destroy(piecesObjects[pieces[end.x][end.y]].GetComponent<MeshRenderer>());
            if(pieces[end.x][end.y].Type == PieceType.intellector)
            {
                GameOver(pieces[start.x][start.y].Team);
            }
        }

        //����������� � ������������
        piecesObjects[pieces[start.x][start.y]].transform.position = TransformCoordinates(end.x, end.y);

        //��� ���� �� ���� ������
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].Team == pieces[start.x][start.y].Team))
        {
            if ( //���� ��� �������� � �����������
                (pieces[start.x][start.y].Type == PieceType.intellector && pieces[end.x][end.y].Type == PieceType.defensor) ||
                (pieces[start.x][start.y].Type == PieceType.defensor && pieces[end.x][end.y].Type == PieceType.intellector)
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
            pieces[end.x][end.y].X = end.x;
            pieces[end.x][end.y].Y = end.y;
            pieces[start.x][start.y] = null;
        }

        //����������� ���� ����
        if(transform_info != 200)
        {
            Destroy(piecesObjects[pieces[end.x][end.y]].GetComponent<MeshRenderer>());
            pieces[end.x][end.y] = GenerateSinglePiece((PieceType)transform_info, pieces[end.x][end.y].Team, end.x, end.y);
        }

        //����� ������� ����
        if (!received_move)
        {
            MoveEvent?.Invoke(start, end, transform_info);
        }

        //"���������"
        void Castling()
        {
            piecesObjects[pieces[end.x][end.y]].transform.position = TransformCoordinates(start.x, start.y); 
            (pieces[start.x][start.y], pieces[end.x][end.y]) = (pieces[end.x][end.y], pieces[start.x][start.y]);
            pieces[start.x][start.y].X = start.x;
            pieces[start.x][start.y].Y = start.y;
            pieces[end.x][end.y].X = end.x;
            pieces[end.x][end.y].Y = end.y;
        }

    }

    //�����������
    IEnumerator WaitForTransformation(Vector2Int start, Vector2Int end)
    {
        yield return new WaitUntil(() => !Around_Intellector.activeInHierarchy);

        TransformToEaten(start, end);

        Around_Intellector.GetComponent<Around_Intellector>().answer = null;
        wait_for_transformation = false;
    }
        
    IEnumerator WaitForPieceType(Vector2Int start, Vector2Int end)
    {
        yield return new WaitUntil(() => !Progressor_end.activeInHierarchy);

        ProgressorTransformation(start, end);

        Progressor_end.GetComponent<Progressor_end>().answer = null;
        wait_for_transformation = false;
    }

    void ProgressorTransformation(Vector2Int start, Vector2Int end)
    {
       PieceType new_type = (PieceType)Progressor_end.GetComponent<Progressor_end>().answer;
       MovePiece(start, end, false, (int)new_type);
    }

    void TransformToEaten(Vector2Int start, Vector2Int end)
    {
        if (Around_Intellector.GetComponent<Around_Intellector>().answer == true)
        {
            PieceType new_type = pieces[end.x][end.y].Type;
            MovePiece(start, end, false, (int)new_type);
        }
        else
        {
            MovePiece(start, end, false);
        }
    }

    //����� ����
    public void GameOver(bool winner, bool ByExit = false)
    {
        if (game_over) return;
        game_over = true;
        DeleteAllHighlights();
        EndGame.DisplayResult(NetworkGame, winner, PlayerTeam, ByExit);
        EndGameEvent?.Invoke();
    }
}
