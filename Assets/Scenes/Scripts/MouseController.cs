using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Board board;
    private Camera currentCamera;

    private static string[] layers_names = {"Tile","HoverTile","SelectedTile","Avaible","HoverAvaible","HoverSelected"};

    void Update()
    {
        if (board.game_over || board.wait_for_transformation) return;
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out info, 500, LayerMask.GetMask(layers_names)))
        {
            //��������� ����
            Vector2Int hitPosition = board.LookUpTileIndex(info.transform.gameObject);
            board.HoverTile(hitPosition);
        }
        else board.RemoveHover();

        //��������� ������� ���
        if (Input.GetMouseButtonDown(0) && (Physics.Raycast(ray, out info, 500, LayerMask.GetMask(layers_names))))
        {
            Vector2Int hitPosition = board.LookUpTileIndex(info.transform.gameObject);
            board.SelectTile(hitPosition);
        }
    }
}
