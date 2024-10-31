using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class UIDrag : MonoBehaviour ,IBeginDragHandler , IEndDragHandler , IDragHandler
{
    public static Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private UnityEngine.UI.Image img;

    private Vector3 originPos;
    private bool drag;

    protected PlaceableObject source;

    public void Initialize(PlaceableObject src)
    {
        source = src;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        img = transform.Find("Icon").GetComponent<UnityEngine.UI.Image>();
        img.maskable = false;
        originPos = rectTransform.anchoredPosition;
    }

    private void FixedUpdate()
    {
        if (drag)
        { 
           Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           RaycastHit2D hit = Physics2D.Raycast(touchPos , Vector2.positiveInfinity);
            if (hit.collider != null)
            {
                PlaceableObject selected = hit.transform.GetComponent<PlaceableObject>();
                if (selected.GetType() == source.GetType())
                {
                    OnCollide(selected);
                }
            }

        }
    }
    protected virtual void OnCollide(PlaceableObject collidedSource) 
    {
    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = true;
        canvasGroup.blocksRaycasts = false;
        PanZoom.current.enabled = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnDrag(PointerEventData eventData)
    {
        drag = false;
        canvasGroup.blocksRaycasts = true;
        PanZoom.current.enabled = true;

        img.maskable = true;
        rectTransform.anchoredPosition = originPos;

    }
}
