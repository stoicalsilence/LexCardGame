using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // Maybe split into PlayerGrid & EnemyGrid
    public enum FIELDTYPE { NORMAL, MEADOW, FOREST, SEA, DARKNESS, WASTELAND, MOUNTAIN }
    public FIELDTYPE fieldType;
    public enum TURNSTATE { DRAWPHASE, MAINPHASE, BATTLEPHASE }
    public TURNSTATE turnstate;

    public CameraMovement cameraMovement;

    public List<Tile> lightTiles;
    public List<Tile> darkTiles;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

}

