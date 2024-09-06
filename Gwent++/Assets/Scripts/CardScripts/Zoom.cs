using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Script q se encarga de hacerle zoom a las cartas 
public class Zoom : MonoBehaviour, IPointerClickHandler
{
    GameObject UbZoom;
    private GameObject zoomCard;
    private Vector2 zoomScale = new Vector2(2.5f, 2.5f);
    public void Awake()
    {
        UbZoom = GameObject.Find("UbicarZoom");
    }
    public void OnMouseEnter()
    {
        zoomCard = Instantiate(gameObject, new Vector2(160, 890), Quaternion.identity);

        zoomCard.transform.SetParent(UbZoom.transform);

        zoomCard.transform.localScale = zoomScale;
    }
    public void OnMouseExit()
    {
       Destroy(zoomCard);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UbZoom.transform.childCount > 0)
        {
            Transform child = UbZoom.transform.GetChild(0);
            Destroy(child.gameObject);
        }
    }
}

