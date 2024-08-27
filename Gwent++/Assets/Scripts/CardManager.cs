using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void MoveCard(GameObject card)
    {
        Debug.Log(card.name + "nombre d la carta");
        Debug.Log("Entro al metodo move card");
        Card cardProperty = card.GetComponent<CardDisplay>().card;
        if(cardProperty.GameZone.Contains("Melee"))
        {
            Debug.Log(cardProperty.Owner);
            Player owner = ContextGame.contextGame.GetPlayer(cardProperty.Owner);
            Debug.Log(owner.HandZone.GetComponent<Zone>().Cards.Count + " count d owner del contexto");
            Debug.Log(owner.ID);
            owner.MeleeZone.GetComponent<Zone>().Cards.Add(card);
            List<GameObject> Hand = owner.HandZone.GetComponent<Zone>().Cards;
            if(!Hand.Contains(card))
            {
                Debug.Log("No tiene el gameobject" + card.name);
            }
            else
            {
                Debug.Log(Hand.Count + "este es el count d la hand y la contiene");
               bool isRemove = Hand.Remove(card);
               Debug.Log("ver si la quito: " + isRemove);
            }
            // bool isRemove = owner.HandZone.GetComponent<Zone>().Cards.Remove(card);
            // Debug.Log("ver si la quito: " + isRemove);
            GameManager.Instance.ChangePlayerTurn();
        }
    }
}
