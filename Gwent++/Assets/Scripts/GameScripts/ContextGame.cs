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
    public GameObject WeatherZone;

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
    public Player GetPlayer(int ID)
    {
        if (ID == 1) return playerFairies.GetComponent<Player>();
        else if(ID == 2) return playerDemons.GetComponent<Player>();
        else throw new Exception("No hay ningun jugador q contenga el ID asignado");
    }
    public List<Card> Board{ get{return GetCardsInBoard();}private set{}} //retorna todas las listas del campo
    private List<Card> GetCardsInBoard()
    {
        List<Card> cards = new List<Card>();
        cards.AddRange(FieldOfPlayer(playerFairies.GetComponent<Player>()));
        cards.AddRange(FieldOfPlayer(playerDemons.GetComponent<Player>()));
        cards.AddRange(WeatherZone.GetComponent<Zone>().Cards);
        return cards;
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
        return player.GetComponent<Player>().GetField();
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

    #region List Methods
    //Push Method
    public void PushGame(Card item, List<Card> list)
    {
        if(CompareList(list, FieldOfPlayer(playerFairies.GetComponent<Player>())) || CompareList(list, FieldOfPlayer(playerDemons.GetComponent<Player>())) || CompareList(list, Board))
        {
            CardManager.Instance.MoveCard(item);
        }
        else
        {
            Push(item, list);
        }
    }
    public void Push(Card item, List<Card> list) => list.Add(item);

    //SendBottomMethod
    public void SendBottomGame(Card item, List<Card> list)
    {
        if(CompareList(list, FieldOfPlayer(playerFairies.GetComponent<Player>())) || CompareList(list, FieldOfPlayer(playerDemons.GetComponent<Player>())) || CompareList(list, Board))
        {
            CardManager.Instance.MoveCard(item);
            //Se cambia su posicion a las pos 0
            Player player = GetPlayer(item.Owner);
            if(player.MeleeZone.GetComponent<Zone>().Cards.Contains(item))
            {
                CambiarAlPrincipio(player.MeleeZone.GetComponent<Zone>().Cards);
            }
            else if(player.RangedZone.GetComponent<Zone>().Cards.Contains(item))
            {
                CambiarAlPrincipio(player.RangedZone.GetComponent<Zone>().Cards);
            }
            else if(player.SiegeZone.GetComponent<Zone>().Cards.Contains(item))
            {
                CambiarAlPrincipio(player.SiegeZone.GetComponent<Zone>().Cards);
            }
            else if(WeatherZone.GetComponent<Zone>().Cards.Contains(item))
            {
                CambiarAlPrincipio(WeatherZone.GetComponent<Zone>().Cards);
            }
        }
        else
        {
            SendBottom(item, list);
        }
    }
    public void SendBottom(Card item, List<Card> list) => list.Insert(0, item);
    private void CambiarAlPrincipio(List<Card> list)
    {
        int LastIndex = list.Count - 1;
        Card LastCard = list[LastIndex];
        list.RemoveAt(LastIndex);
        list.Insert(0, LastCard);
    }
    // RemoveMethod
    public void RemoveGame(Card item, List<Card> list)
    {
        if(CompareList(list, FieldOfPlayer(playerFairies.GetComponent<Player>())) || CompareList(list, FieldOfPlayer(playerDemons.GetComponent<Player>())) || CompareList(list, Board))
        {
            Player player = GetPlayer(item.Owner);
            if(player.MeleeZone.GetComponent<Zone>().Cards.Contains(item))
            {
                player.MeleeZone.GetComponent<Zone>().Cards.Remove(item);
            }
            else if(player.RangedZone.GetComponent<Zone>().Cards.Contains(item))
            {
                player.RangedZone.GetComponent<Zone>().Cards.Remove(item);
            }
            else if(player.SiegeZone.GetComponent<Zone>().Cards.Contains(item))
            {
                player.SiegeZone.GetComponent<Zone>().Cards.Remove(item);
            }
            else if(WeatherZone.GetComponent<Zone>().Cards.Contains(item))
            {
                WeatherZone.GetComponent<Zone>().Cards.Remove(item);
            }
        }
        else
        {
            list.Remove(item);
        }
    }
    private bool CompareList(List<Card> list1, List<Card> list2)
    {
        if(list1.Count != list2.Count) return false;
        for (int i = 0; i < list1.Count; i++)
        {
            if(list1[i] != list2[i]) return false;
        }
        return true;
    }
    public Card Pop(List<Card> list)
    {
        Card card = list[list.Count -1];
        RemoveGame(card, list);
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
    #endregion

    #region Game Methods
    //Metodo q permite robar una carta del deck
    public void Stole(Player player) 
    {
        Card card = player.GetComponent<Player>().Deck[0];
        player.GetComponent<Player>().HandZone.GetComponent<Zone>().Cards.Add(card);
        player.GetComponent<Player>().Deck.Remove(card);
    }  
    //Cambiar carta
    public void ChangeCard(Player player, Card card)
    {
        if(player.CanChange)
        {
            player.HandZone.GetComponent<Zone>().Cards.Remove(card);
            player.Cementery.Add(card);
            Stole(player);
            player.ChangedCards ++;
            if(player.ChangedCards >= 2) player.CanChange = false;
            player.HandZone.GetComponent<Zone>().UpdateZone();
        }
        else GameManager.Instance.cartelManager.GetComponent<CartelManager>().MostrarCartel("Ya no puede cambiar cartas");
    }
    public void UpdateFront()
    {
        double CounterFairies = 0;
        double CounterDemons = 0;
        //Se actualizan las cartas de cada zona
        WeatherZone.GetComponent<Zone>().UpdateZone();
        playerFairies.GetComponent<Player>().HandZone.GetComponent<Zone>().UpdateZone();
        playerDemons.GetComponent<Player>().HandZone.GetComponent<Zone>().UpdateZone();
        for(int i = 0; i < 3; i++)
        {
            CounterFairies += playerFairies.GetComponent<Player>().Field[i].GetComponent<Zone>().UpdateZone();
            CounterDemons += playerDemons.GetComponent<Player>().Field[i].GetComponent<Zone>().UpdateZone();
            playerFairies.GetComponent<Player>().Increase[i].GetComponent<Zone>().UpdateZone();
            playerDemons.GetComponent<Player>().Increase[i].GetComponent<Zone>().UpdateZone();
        }
        //Se actualiza el contador
        playerFairies.GetComponent<Player>().UpdateCounter(CounterFairies);
        playerDemons.GetComponent<Player>().UpdateCounter(CounterDemons);
    }

    public void CleanTheBoard()
    {
        for(int i = 0; i < 3; i++)
        {
            CleanZone(playerFairies.GetComponent<Player>().Field[i].GetComponent<Zone>().Cards, playerFairies.GetComponent<Player>());
            CleanZone(playerDemons.GetComponent<Player>().Field[i].GetComponent<Zone>().Cards, playerDemons.GetComponent<Player>());
        }
    
        for(int i = 0; i < 3; i++)
        {
            CleanZone(playerFairies.GetComponent<Player>().Increase[i].GetComponent<Zone>().Cards, playerFairies.GetComponent<Player>());
            CleanZone(playerDemons.GetComponent<Player>().Increase[i].GetComponent<Zone>().Cards, playerDemons.GetComponent<Player>());
        }

        CleanWeatherZone();
        
        UpdateFront();
    }
    public void CleanWeatherZone()
    {
        foreach(Card card in WeatherZone.GetComponent<Zone>().Cards)
        {
            if(card.Owner == 1)
            {
                playerFairies.GetComponent<Player>().Cementery.Add(card);
            }
            else
            {
                playerDemons.GetComponent<Player>().Cementery.Add(card);
            }
        }
        WeatherZone.GetComponent<Zone>().Cards.Clear();
    }
    public void CleanZone(List<Card> list, Player player)
    {
        foreach(Card card in list)
        {
            player.Cementery.Add(card);
        }
        list.Clear();
    }
    #endregion
}
