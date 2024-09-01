using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 startPos;
    private float deltaX, deltaY;

    private void Start()
    {
        startPos = Input.mousePosition;
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        deltaX = startPos.x - transform.position.x;
        deltaY = startPos.y - transform.position.y;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = new Vector3(mousePos.x - deltaX ,mousePos.y - deltaY);

        Vector3Int cellPos = BuildingSystem.current.gridLayout.WorldToCell(pos);
        transform.position = BuildingSystem.current.gridLayout.CellToLocalInterpolated(cellPos);
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        { 
            gameObject.GetComponent<PlaceableObject>().CheckPlacement();
            Destroy(this);
        }
    }
}
