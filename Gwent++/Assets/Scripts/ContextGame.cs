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
    public Player TriggerPlayer
    {
        get
        {
            if(GameManager.Instance.CurrentPlayer == false) return playerFairies.GetComponent<Player>();
            else return playerDemons.GetComponent<Player>();
        }
    } // retorna el jugador q esta jugando
    public List<GameObject> Board{ get{return GetCardsInBoard();}private set{}} //retorna todas las listas del campo
    private List<GameObject> GetCardsInBoard()
    {
        List<GameObject> cards = new List<GameObject>();
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
    public List<GameObject> Hand => HandOfPlayer(TriggerPlayer);
    public List<GameObject> Deck => DeckOfPlayer(TriggerPlayer);
    public List<GameObject> Field => FieldOfPlayer(TriggerPlayer);
    public List<GameObject> Graveyard => GraveyardOfPlayer(TriggerPlayer);
    public List<GameObject> HandOfPlayer(Player player)
    {
        return player.GetComponent<Player>().HandZone.GetComponent<Zone>().Cards;
    }
    public List<GameObject> FieldOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Field();
    }
    public List<GameObject> GraveyardOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Cementery;
    }
    public List<GameObject> DeckOfPlayer(Player player)
    {
        return player.GetComponent<Player>().Deck;
    }
    #endregion

    #region Methods
    //Falta Find
    public void Push(GameObject item, List<GameObject> list) => list.Add(item);
    public void SendBottom(GameObject item, List<GameObject> list) => list.Insert(0, item);
    public GameObject Pop(List<GameObject> list)
    {
        GameObject card = list[list.Count -1];
        list.Remove(card);
        return card;
    }
    static System.Random random = new System.Random();
    public void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int index = random.Next(0, list.Count - 1);
            //Change
            GameObject Temp = list[i];
            list[i] = list[index];
            list[index] = Temp;
        }
    }
    //Metodo q permite robar una carta del deck
    public void Stole(Player player) 
    {
        GameObject drawCard = Instantiate(DeckOfPlayer(player)[0], new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log(drawCard.name);
        drawCard.transform.SetParent(player.HandZone.transform, false);
        player.Deck.Remove(DeckOfPlayer(player)[0]);
    }  
    #endregion
}
