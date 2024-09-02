using System;
using System.Collections.Generic;
using UnityEngine;
public class ContextGame : MonoBehaviour 
{
    public static ContextGame contextGame;
    void Awake()
    {
        if(contextGame == null)
        {
            contextGame = this;
        }
        else if(contextGame != this)
        {
            Destroy(gameObject);
        }
    }
    //Referencia a las listas del tablero:
    public GameObject playerFairies;
    public GameObject playerDemons;
    // public GameObject WeatherZone;

    #region Context Methods
    // retorna el jugador q esta jugando
    public Player TriggerPlayer
    {
        get
        {
            if(GameManager.Instance.CurrentPlayer == false) return playerFairies.GetComponent<Player>();
            else return playerDemons.GetComponent<Player>();
        }
    } 
    //Retorna el jugador enemigo
    public Player EnemyPlayer
    {
        get
        {
            if(GameManager.Instance.CurrentPlayer == true) return playerFairies.GetComponent<Player>();
            else return playerDemons.GetComponent<Player>();
        }
    }
    public List<Card> Board{ get{return GetCardsInBoard();}private set{}} //retorna todas las listas del campo
    private List<Card> GetCardsInBoard()
    {
        List<Card> cards = new List<Card>();
        cards.AddRange(FieldOfPlayer(playerFairies.GetComponent<Player>()));
        //cards.AddRange(FieldOfPlayer(playerDemons.GetComponent<Player>()));
        //poner tamb las clima
        return cards;
    }
    public Player GetPlayer(int ID)
    {
        if (ID == 1) return playerFairies.GetComponent<Player>();
        else if(ID == 2) return playerDemons.GetComponent<Player>();
        else throw new Exception("No hay ningun jugador q contenga el ID asignado");
    }
    public List<Card> Hand => HandOfPlayer(TriggerPlayer);
    public List<Card> Deck => DeckOfPlayer(TriggerPlayer);
    public List<Card> Field => FieldOfPlayer(TriggerPlayer);
    public List<Card> Graveyard => GraveyardOfPlayer(TriggerPlayer);
    public List<Card> HandOfPlayer(Player player)
    {
        return player.GetComponent<Player>().HandZone.GetComponent<Zone>().Cards;
    }
    public List<Card> FieldOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Field();
    }
    public List<Card> GraveyardOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Cementery;
    }
    public List<Card> DeckOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Deck;
    }
    #endregion

    #region Methods
    //Falta Find
    public void Push(Card item, List<Card> list) => list.Add(item);
    public void SendBottom(Card item, List<Card> list) => list.Insert(0, item);
    public Card Pop(List<Card> list)
    {
        Card card = list[list.Count -1];
        list.Remove(card);
        return card;
    }
    static System.Random random = new System.Random();
    public void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int index = random.Next(0, list.Count - 1);
            //Change
            Card Temp = list[i];
            list[i] = list[index];
            list[index] = Temp;
        }
    }
    // //Metodo q permite robar una carta del deck
    public void Stole(Player player) 
    {
        Card card = player.GetComponent<Player>().Deck[0];
        player.GetComponent<Player>().HandZone.GetComponent<Zone>().Cards.Add(card);
        player.GetComponent<Player>().Deck.Remove(card);
    }  

    public void UpdateFront()
    {
        playerFairies.GetComponent<Player>().HandZone.GetComponent<Zone>().UpdateZone();
        playerFairies.GetComponent<Player>().MeleeZone.GetComponent<Zone>().UpdateZone();
        playerFairies.GetComponent<Player>().SiegeZone.GetComponent<Zone>().UpdateZone();
        playerFairies.GetComponent<Player>().RangedZone.GetComponent<Zone>().UpdateZone();
    }
    #endregion
}
