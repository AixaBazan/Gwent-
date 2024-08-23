using System;
using System.Collections.Generic;
class Player
{
    public int Id {get; private set;}
    public List<Card> Hand {get; private set;}
    public List<Card> Cementery {get; private set;}
    public List<Card> Field {get; private set;}
    public List<Card> Deck {get; private set;}
    public Player(int id, List<Card> hand, List<Card> cementery, List<Card> field, List<Card> deck)
    {
        this.Id = id;
        this.Hand = hand;
        this.Cementery = cementery;
        this.Field = field;
        this.Deck = deck;
    }
}