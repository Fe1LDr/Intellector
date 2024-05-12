
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private Camera TheCamera;
    public CinemachineFreeLook FreeCamera;
    public CinemachineVirtualCamera VirtCamera2;

    [SerializeField] private Board board;
    void Start()
    {
        TheCamera = GetComponent<Camera>();
        TheCamera = Camera.main;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            FreeCamera.gameObject.SetActive(!FreeCamera.gameObject.activeSelf);
            VirtCamera2.gameObject.SetActive(!VirtCamera2.gameObject.activeSelf);
            if (board.PlayerTeam == false)
            {
                VirtCamera2.transform.Rotate(0, 0, 0);
            }
            else
            {
                VirtCamera2.transform.Rotate(0, 0, 180);
            }
        }
    }
}
