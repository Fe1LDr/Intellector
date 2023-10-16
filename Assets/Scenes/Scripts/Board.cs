using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileSize;

    private GameObject[][] tiles;
    private Vector2Int? currentHover = null;

    private static float x_offset;
    private static float y_offset;

    // Start is called before the first frame update
    void Start()
    {
        x_offset = tileSize / Mathf.Sqrt(3) * 1.5f;
        y_offset = tileSize;

        GenerateAllTiles();
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
