using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class Zone : MonoBehaviour
{
    public List<GameObject> Cards;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject card = collision.gameObject;
        Debug.Log("colisiono" + card.name);
        Player owner = ContextGame.contextGame.GetPlayer(card.GetComponent<CardDisplay>().card.Owner);
        Cards.Add(card);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject card = collision.gameObject;
        Debug.Log("se fue d la zona" + card.name);
        Player owner = ContextGame.contextGame.GetPlayer(card.GetComponent<CardDisplay>().card.Owner);
        card.GetComponent<CardDisplay>().card.Power = card.GetComponent<CardDisplay>().card.OriginalPower;
        owner.Cementery.Add(card);
        Cards.Remove(card);
    }
}