﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoom : MonoBehaviour
{
    public static PanZoom current;

    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float upperLimit;
    [SerializeField] private float bottomLimit;

    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;


    private Camera cam;

    private bool moveAllowed;
    private Vector3 touchPos;

    private Transform objectToFollow;
    private Bounds objectBounts;
    private Vector3 prevPos;

    public bool canMoveAndZoom = true;


    private void Awake()
    {
        cam = GetComponent<Camera>();
        current = this;
    }

    private void Update()
    {
        if (!canMoveAndZoom) return;

        if (objectToFollow != null)
        {
            Vector3 objPos = cam.WorldToViewportPoint(objectToFollow.position + objectBounts.max);
            if (objPos.x >= 0.7f || objPos.x <= 0.3f || objPos.y >= 0.7f || objPos.y <= 0.3f)
            {
                Vector3 pos = cam.ScreenToWorldPoint(objectToFollow.position);
                Vector3 direction = pos - prevPos;
                cam.transform.position += direction;
                prevPos = pos;

                transform.position = new Vector3
                           (
                           Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                           Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                           transform.position.z
                           );
            }
            else
            {
                Vector3 pos = cam.ScreenToWorldPoint(objectToFollow.position);
                prevPos = pos;
            }
            return;
        }
       
        // Pentru Android
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId) ||
                    EventSystem.current.IsPointerOverGameObject(touchZero.fingerId))
                { 
                    return;
                }

                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                float difference = currentDistTouch - distTouch;

                Zoom(difference * 0.01f);
            }
            else
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            moveAllowed = false;
                        }
                        else
                        {
                            moveAllowed = true;
                        }
                        touchPos = cam.ScreenToWorldPoint(touch.position);
                        break;

                    case TouchPhase.Moved:
                        if (moveAllowed)
                        {
                            Vector3 direction = touchPos - cam.ScreenToWorldPoint(touch.position);
                            cam.transform.position += direction;

                            //transform.position = new Vector3
                            //(
                            //Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                            //Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
                            //transform.position.z
                            //);
                        }
                        break;
                }
            }
        }

        // === Logica pentru PC (mișcare cu mouse-ul) ===
        if (Input.GetMouseButtonDown(0))  // Detectăm apăsarea butonului stâng al mouse-ului
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                moveAllowed = true;  // Permitem mișcarea
                touchPos = cam.ScreenToWorldPoint(Input.mousePosition);  // Salvăm poziția mouse-ului
            }
            else
            {
                moveAllowed = false;  // Nu permitem mișcarea dacă mouse-ul este peste un element UI
            }
        }

        if (Input.GetMouseButton(0) && moveAllowed)  // Detectăm mișcarea mouse-ului în timp ce butonul stâng este apăsat
        {
            Vector3 direction = touchPos - cam.ScreenToWorldPoint(Input.mousePosition);  // Calculăm direcția de mișcare
            cam.transform.position += direction;  // Mutăm camera

            // Limităm mișcarea în limitele setate
            //transform.position = new Vector3
            //(
            //    Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
            //    Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
            //    transform.position.z
            //);
        }


        // Pentru PC (zoom cu roata mouse-ului)
        if (Input.mouseScrollDelta.y != 0)
        {
            float scroll = Input.mouseScrollDelta.y;
            Zoom(scroll * 0.5f);  // Ajustăm factorul de zoom pentru PC (poți modifica 0.5f dacă este prea rapid sau prea lent)
        }
    }

    private void Zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment , zoomMin , zoomMax );
    }

    public void FollowObject(Transform objToFollow)
    {
        objectToFollow = objToFollow;
        objectBounts = objectToFollow.GetComponent<PolygonCollider2D>().bounds;
        prevPos = cam.ScreenToWorldPoint(Vector3.zero);
    }

    public void UnfollowObject()
    {
        objectToFollow = null;
    }

    public void Focus(Vector3 position)
    {
        Vector3 newPos = new Vector3(position.x , position.y , transform.position.z);
        LeanTween.move(gameObject , newPos , 0.2f);

        //transform.position = new Vector3
        //                   (
        //                   Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
        //                   Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
        //                   transform.position.z
        //                   );

        touchPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
        new Vector3((rightLimit + leftLimit) / 2.0f, (upperLimit + bottomLimit) / 2.0f, 0),
        new Vector3(rightLimit - leftLimit, upperLimit - bottomLimit, 0)
        );
    }
}
 