using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    private Vector3 origin;

    public BoundsInt area;

    public bool CanBePlaced()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //if (BuildingSystem.current.CanTakeArea(areaTemp))
        //{
        //    return true;
        //}

        //return false;
        return BuildingSystem.current.CanTakeArea(areaTemp);
    }

    public virtual void Place()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;
        //origin = transform.position;
        BuildingSystem.current.TakeArea(areaTemp);

        PanZoom.current.UnfollowObject();


    }

    public void CheckPlacement()
    {
        if(!Placed)
        {
            if (CanBePlaced())
            {
                Place();
                origin = transform.position;
            }
            else
            {
                Destroy(transform.gameObject);
            }

            //ShopManager.current.ShopButton_Click();
        }
        else
        {
            if (CanBePlaced())
            {
                Place();
                origin = transform.position;
            }
            else
            {
                transform.position = origin;
                Place();

            }
        }

   
    }

    private float time = 0f;
    private bool touching;

    private void Update()
    {
        if (!touching && Placed)
        { 
            if(Input.GetMouseButtonDown(0))
            {
                time = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                time += Time.deltaTime;

                if (time > 3f)
                {
                    touching = true;
                    gameObject.AddComponent<ObjectDrag>();

                    Vector3Int positionInt = BuildingSystem.current.gridLayout.WorldToCell(transform.position);
                    BoundsInt areaTemp = area;
                    areaTemp.position = positionInt;

                    BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTileMap);
                }
            }
        }

        if (touching && Input.GetMouseButton(0))
        {
            touching = false;
        }
    }


}
