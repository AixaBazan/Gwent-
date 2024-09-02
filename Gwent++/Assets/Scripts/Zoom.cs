using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script q se encarga de hacerle zoom a las cartas 
public class Zoom : MonoBehaviour
{
    GameObject UbZoom;
    private GameObject zoomCard;
    private Vector2 zoomScale = new Vector2(3, 3);
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
}

