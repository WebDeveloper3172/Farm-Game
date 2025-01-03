﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTileMap;
    public TileBase takenTile;


    private void Awake()
    {
        current = this;
    }

    #region Tilemap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileBase tileBase, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        FillTiles(tileArray, tileBase);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileBase tileBase)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBase;
        }
    }

    public void ClearArea(BoundsInt area, Tilemap tilemap)
    {
        SetTilesBlock(area, null, tilemap);
    }
    #endregion

    #region  Building Placement

    public GameObject InitializeWithObject(GameObject building, Vector3 pos)
    {
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        GameObject obj = Instantiate(building, position, Quaternion.identity);
        PlaceableObject temp = obj.transform.GetComponent<PlaceableObject>();
        temp.gameObject.AddComponent<ObjectDrag>();

        PanZoom.current.FollowObject(obj.transform);

        return obj;
    }
    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTileMap);
        foreach (var b in baseArray)
        {
            if (b == takenTile)
            {
                return false;
            }
        }
        return true;
    }


    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, takenTile, MainTileMap);
    }

    public void UnlockTerritory(BoundsInt area, TileBase territoryTile, Tilemap territoryTileMap)
    {
        Debug.LogError("Territoriul Deblocat");
        // Logare pentru a verifica începutul metodei
        Debug.LogError($"Încercăm să deblocăm teritoriul pentru zona: {area}");

        // Creează un array pentru tile-uri
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];

        // Umplem array-ul cu tile-ul specificat
        //FillTiles(tileArray, territoryTile);

        // Logare pentru a verifica tile-urile care vor fi setate
        Debug.LogError($"Setăm tile-uri pentru zona {area} cu tile: {territoryTile}");

        // Setează toate tile-urile în Tilemap pentru zona specificată
        territoryTileMap.SetTilesBlock(area, tileArray);

        // Logare pentru a confirma finalizarea operației
        Debug.LogError($"Teritoriul deblocat cu succes pentru zona: {area}");

        #endregion
    }

}
