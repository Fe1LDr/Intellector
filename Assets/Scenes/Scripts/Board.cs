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

    [Header("UI")]
    [SerializeField] GameObject Progressor_end;
    [SerializeField] GameObject Around_Intellector;

    public delegate void MoveDelegate(Vector2Int start, Vector2Int end);
    public event MoveDelegate MoveEvent;

    [NonSerialized] public bool Turn;
    [NonSerialized] public bool game_over;
    public Piece[][] pieces;
    public GameObject[][] tiles;

    private List<Vector2Int> AvaibleMoves;

    private Vector2Int currentHover  = -Vector2Int.one;
    private Vector2Int currentSelect = -Vector2Int.one;

    private static float x_offset;
    private static float y_offset;

    private Vector2Int startAsk;
    private Vector2Int endAsk;


    // Start is called before the first frame update
    public void Start()
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


    //Создание поля и фигур
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


    //операции с полями и слоями
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
        //Debug.Log($"{coordinates.x} {coordinates.y}");
        currentHover = coordinates;
    }
    public void RemoveHover()
    {
        currentHover = -Vector2Int.one;
    }

    //перемещение фигур
    public void SelectTile(Vector2Int coordinates)
    {
        if (NetworkGame && (PlayerTeam != Turn)) return; //не трогаем чужие фигуры

        //если не выбрана никакая фигура
        if (currentSelect == -Vector2Int.one)
            if((pieces[coordinates.x][coordinates.y] != null) && (pieces[coordinates.x][coordinates.y].team == Turn)) //если нажали на фигуру выбираем её
            {
                currentSelect = coordinates;
                AvaibleMoves = pieces[coordinates.x][coordinates.y].GetAvaibleMooves();
                return;
            }
            else { return; }

        //если уже выбрана
        else
        {
            if (currentSelect == coordinates) //если нажали на ту же фигуру сбрасываем выделение
            {
                currentSelect = -Vector2Int.one;
                AvaibleMoves = null;
            }
            else //а если нажали на другое поле
            {
                if (AvaibleMoves.Contains(coordinates)) // и туда можно пойти, то идём туда
                {
                    MovePiece(currentSelect, coordinates, false);
                    // и сбрасываем выделение
                    currentSelect = -Vector2Int.one;
                    AvaibleMoves = null;
                }
            }
        }
    }
    public void MovePiece(Vector2Int start, Vector2Int end, bool received_move, int transform_info = -1)
    {
        //проверка очерёдности хода
        if (pieces[start.x][start.y].team != Turn || game_over) return;

        //Переключение очерёдности хода
        Turn = !Turn;

        if ((pieces[start.x][start.y].type == PieceType.progressor) && // если ходил прогрессор
            ((pieces[start.x][start.y].team == false && (end.y == 6)) || (pieces[start.x][start.y].team == true && (end.y == 0) && (end.x % 2 == 0)))) //и он дошёл до поля превращения
        {
            Progressor_end.SetActive(true);

            startAsk = start;
            endAsk = end;

            StartCoroutine(WaitForPieceType());
            game_over = true;

            return;
        }

        //едим если занято вражеской фигурой
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].team != pieces[start.x][start.y].team))
        {
            if (pieces[start.x][start.y].HasIntellectorNearby() && (pieces[end.x][end.y].type != PieceType.intellector)) //если рядом есть свой интеллектор
            {   // то можно превратиться в съеденную фигуру
                Around_Intellector.SetActive(true);

                startAsk = start;
                endAsk = end;

                StartCoroutine(WaitForTransformation());
                game_over = true;

                return;
            }

            Destroy(pieces[end.x][end.y].GetComponent<MeshRenderer>());
            if(pieces[end.x][end.y].type == PieceType.intellector)
            {
                GameOver(pieces[start.x][start.y].team);
            }
        }

        //перемещение в пространстве
        pieces[start.x][start.y].transform.position = TransformCoordinates(end.x, end.y);

        //при ходе на свою фигуру
        if (pieces[end.x][end.y] != null && (pieces[end.x][end.y].team == pieces[start.x][start.y].team))
        {
            if ( //если это дефенсор и интеллектор
                (pieces[start.x][start.y].type == PieceType.intellector && pieces[end.x][end.y].type == PieceType.defensor) ||
                (pieces[start.x][start.y].type == PieceType.defensor && pieces[end.x][end.y].type == PieceType.intellector)
               )
            {   //то меняем их местами
                Castling();
            }
            else
                throw new InvalidOperationException("Невозможный ход: ход на свою фигуру");
        }
        else
        {
            //изменение ссылок
            pieces[end.x][end.y] = pieces[start.x][start.y];
            pieces[end.x][end.y].x = end.x;
            pieces[end.x][end.y].y = end.y;
            pieces[start.x][start.y] = null;
        }


        MoveEvent?.Invoke(start , end);

        //"рокировка"
        void Castling()
        {
            pieces[end.x][end.y].transform.position = TransformCoordinates(start.x, start.y); 
            (pieces[start.x][start.y], pieces[end.x][end.y]) = (pieces[end.x][end.y], pieces[start.x][start.y]);
            pieces[start.x][start.y].x = start.x;
            pieces[start.x][start.y].y = start.y;
            pieces[end.x][end.y].x = end.x;
            pieces[end.x][end.y].y = end.y;
        }

        //превращение прогрессора
        void ProgressorTransformation()
        {
            PieceType new_type = (PieceType)Progressor_end.GetComponent<Progressor_end>().answer;

            Destroy(pieces[startAsk.x][startAsk.y].GetComponent<MeshRenderer>());

            if(pieces[endAsk.x][endAsk.y] != null)
                Destroy(pieces[endAsk.x][endAsk.y].GetComponent<MeshRenderer>());
             

            // works, but looks weird

            pieces[endAsk.x][endAsk.y] = GenerateSinglePiece(new_type, pieces[startAsk.x][startAsk.y].team, endAsk.x, endAsk.y);
            pieces[startAsk.x][startAsk.y] = null;
        }    
         
        //превращение в съеденную фигуру
        void TransformToEaten()
        {
            if (Around_Intellector.GetComponent<Around_Intellector>().answer == true)
            {
                PieceType new_type = pieces[endAsk.x][endAsk.y].type;

                Destroy(pieces[startAsk.x][startAsk.y].GetComponent<MeshRenderer>());
                Destroy(pieces[endAsk.x][endAsk.y].GetComponent<MeshRenderer>());

                pieces[endAsk.x][endAsk.y] = GenerateSinglePiece(new_type, pieces[startAsk.x][startAsk.y].team, endAsk.x, endAsk.y);
                pieces[startAsk.x][startAsk.y] = null;
            }
            else
            {
                Destroy(pieces[endAsk.x][endAsk.y].GetComponent<MeshRenderer>());

                //перемещение в пространстве
                pieces[startAsk.x][startAsk.y].transform.position = TransformCoordinates(endAsk.x, endAsk.y);

                //изменение ссылок
                pieces[endAsk.x][endAsk.y] = pieces[startAsk.x][startAsk.y];
                pieces[endAsk.x][endAsk.y].x = endAsk.x;
                pieces[endAsk.x][endAsk.y].y = endAsk.y;
                pieces[startAsk.x][startAsk.y] = null;
            }
        }

        IEnumerator WaitForTransformation()
        {
            yield return new WaitUntil(() => !Around_Intellector.activeInHierarchy);

            TransformToEaten();

            Around_Intellector.GetComponent<Around_Intellector>().answer = null;
            game_over = false;
        }

        IEnumerator WaitForPieceType()
        {
            yield return new WaitUntil(() => !Progressor_end.activeInHierarchy);

            ProgressorTransformation();

            Progressor_end.GetComponent<Progressor_end>().answer = null;
            game_over = false;
        }
    }

    void GameOver(bool winner)
    {
        Debug.Log((winner) ? "ПОБЕДИЛИ НЕ БЕЛЫЕ" : "ПОБЕДИЛИ БЕЛЫЕ");
        AvaibleMoves = null;
        currentHover = -Vector2Int.one;
        currentSelect = -Vector2Int.one;
        game_over = true;
    }
}
