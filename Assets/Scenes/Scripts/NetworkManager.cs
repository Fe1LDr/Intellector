using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] private Board board;
    // Start is called before the first frame update
    void Start()
    {
        if (board.NetworkGame)
        {
            board.MoveEvent += MoveEventHandler;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveEventHandler(Vector2Int start, Vector2Int end)
    {
        Debug.Log($"����� ������ ���� �������� ���� {start} ; {end}");
    }
}
