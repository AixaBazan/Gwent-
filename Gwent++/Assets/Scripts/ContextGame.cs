using System;
using System.Collections.Generic;
class ContextGame
{
    public int TriggerPlayer{ get; set; } // retorna el Id del jugador q esta en su turno
    public List<Card> Board{ get; set; } //retorna todas las listas del campo, hacer metodo 
    public List<Card> HandOfPlayer(Player player)
    {
        return player.Hand;
    }
    public List<Card> FieldOfPlayer(Player player)
    {
        return player.Field;
    }
    public List<Card> GraveyardOfPlayer(Player player)
    {
        return player.Cementery;
    }
    public List<Card> DeckOfPlayer(Player player)
    {
        return player.Deck;
    }
}
