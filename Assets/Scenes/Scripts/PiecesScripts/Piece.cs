using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public int x;
    public int y;
    public bool team;
    public Board board;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    abstract public List<Vector2Int> GetAvaibleMooves();
}
