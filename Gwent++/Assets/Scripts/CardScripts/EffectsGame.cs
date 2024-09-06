using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public enum ParticularEffect
{
    None, UserEffect, MultiplyAttack, Increase, Weather, CleanWeather, Decoy,
    DrawCard, FairiesLeaderEffect, DemonsLeaderEffect, DeleteRowWithFewerCards,
    DeleteCardWithMorePower, DeletCardWithFewerPower, Average, PutWeatherCard, PutIncreaseCard
}
public class Effects : MonoBehaviour 
{
    public static Effects Instance;
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
    public void RunEffect(Card card)
    {
        Player player = ContextGame.contextGame.GetPlayer(card.Owner);
        Player enemy = ContextGame.contextGame.EnemyPlayer;

        if(card.effect == ParticularEffect.None)
        {
            Debug.Log("Carta sin efecto");
        }
        else if(card.effect == ParticularEffect.UserEffect)
        {
            Debug.Log("Se activo el efecto de la carta creada por el usuario");
            foreach(var effect in card.effects)
            {
                effect.Interprete();
            }
        }
        else if(card.effect == ParticularEffect.MultiplyAttack)
        {
            MultiplyPower(player, card);
        }
        else if(card.effect == ParticularEffect.Increase)
        {
            if(card.GameZone.Contains(ValidZone.IncreaseMelee))
            {
                ModificPower(5, player.MeleeZone.GetComponent<Zone>().Cards);
            }
            if(card.GameZone.Contains(ValidZone.IncreaseRanged))
            {
                ModificPower(5, player.RangedZone.GetComponent<Zone>().Cards);
            }
            if(card.GameZone.Contains(ValidZone.IncreaseSiege))
            {
                ModificPower(5, player.SiegeZone.GetComponent<Zone>().Cards);
            }
        }
        else if(card.effect == ParticularEffect.Weather)  
        {
            if(card.Range.Contains("Melee"))
            {
                ModificPower(-5, ContextGame.contextGame.playerFairies.GetComponent<Player>().MeleeZone.GetComponent<Zone>().Cards);
                ModificPower(-5, ContextGame.contextGame.playerDemons.GetComponent<Player>().MeleeZone.GetComponent<Zone>().Cards);
            }
            if(card.Range.Contains("Ranged"))
            {
                ModificPower(-5, ContextGame.contextGame.playerFairies.GetComponent<Player>().RangedZone.GetComponent<Zone>().Cards);
                ModificPower(-5, ContextGame.contextGame.playerDemons.GetComponent<Player>().RangedZone.GetComponent<Zone>().Cards);
            }
            if(card.Range.Contains("Siege"))
            {
                ModificPower(-5, ContextGame.contextGame.playerFairies.GetComponent<Player>().SiegeZone.GetComponent<Zone>().Cards);
                ModificPower(-5, ContextGame.contextGame.playerDemons.GetComponent<Player>().SiegeZone.GetComponent<Zone>().Cards);
            }
        }
        else if(card.effect == ParticularEffect.CleanWeather)
        {
            ContextGame.contextGame.CleanWeatherZone();
        }
        else if(card.effect == ParticularEffect.Decoy)
        {
            Decoy(player.GetField(), player, card);
        }
        else if(card.effect == ParticularEffect.DrawCard)
        {
            ContextGame.contextGame.Stole(player);
        }
        else if(card.effect == ParticularEffect.FairiesLeaderEffect)
        {
            //Aumenta en 10 el poder de las cartas Plata de su campo
            ModificPower(10, player.GetField());
        }
        else if(card.effect == ParticularEffect.DemonsLeaderEffect)
        {
            //Destruye la fila con mas cartas del jugador contrario
            List<Card> list = enemy.Field.OrderByDescending(c => c.GetComponent<Zone>().Cards.Count()).FirstOrDefault().GetComponent<Zone>().Cards;
            ContextGame.contextGame.CleanZone(list, enemy);
        }
        else if(card.effect == ParticularEffect.DeleteRowWithFewerCards)
        {
            List<Card> list = enemy.Field.OrderBy(c => c.GetComponent<Zone>().Cards.Count()).FirstOrDefault().GetComponent<Zone>().Cards;
            ContextGame.contextGame.CleanZone(list, enemy);
        }
        else if(card.effect == ParticularEffect.DeleteCardWithMorePower)
        {
            if(enemy.GetField().Count > 0)
            {
                Card cardToRemove = player.GetField().OrderByDescending(c => c.Power).FirstOrDefault();
                ContextGame.contextGame.RemoveGame(cardToRemove, player.GetField());
            }
        }
        else if(card.effect == ParticularEffect.DeletCardWithFewerPower)
        {
            if(enemy.GetField().Count > 0)
            {
                Card cardToRemove = player.GetField().OrderBy(c => c.Power).FirstOrDefault();
                ContextGame.contextGame.RemoveGame(cardToRemove, player.GetField());
            }
        }
        else if(card.effect == ParticularEffect.Average)
        {
            List<Card> list = player.GetField();
            if(list.Count > 0)
            {
                double average = list.Average(c => c.Power);
                foreach(var item in list)
                {
                    item.Power = average;
                }
            }
        }
        else if(card.effect == ParticularEffect.PutWeatherCard)
        {
            Card item = FindSpecialCard(player.Deck, CardType.Clima);
            if(item != null) CardManager.Instance.MoveCard(item);
        }
        else if(card.effect == ParticularEffect.PutIncreaseCard)
        {
            Card item = FindSpecialCard(player.Deck, CardType.Aumento);
            if(item != null) CardManager.Instance.MoveCard(item);
        }
    }
    #region Card Effects
    private Card FindSpecialCard(List<Card> list, CardType type)
    {
        Card card = null;
        foreach(var item in list)
        {
            if(item.Type == type)
            {
                card = item;
                break;
            }
        }
        return card;
    }
    private void MultiplyPower(Player player, Card card)
    {
        List<Card> field = player.GetField();
        int counter = 0;
        foreach(Card item in field)
        {
            if(card.Name == item.Name) counter ++;
        }
        card.Power = card.Power * counter;
    }
    private void ModificPower(int n, List<Card> cards)
    {
        foreach(Card item in cards)
        {
            if(item.Type == CardType.Plata)
            {
                item.Power += n;
            }
        }
    }
    private void Decoy(List<Card> list, Player player, Card decoy)
    {
        Card card = CardWithGreaterPower(list);
        if(card != null)
        {   
            player.HandZone.GetComponent<Zone>().Cards.Add(card);
            player.HandZone.GetComponent<Zone>().Cards.Remove(decoy);
            card.IsPlayed = false;
            decoy.IsPlayed = true;
            if(card.GameZone.Contains(ValidZone.Melee))
            {
                player.MeleeZone.GetComponent<Zone>().Cards.Remove(card);
                player.MeleeZone.GetComponent<Zone>().Cards.Add(decoy);
            }
            else if(card.GameZone.Contains(ValidZone.Ranged))
            {
                player.RangedZone.GetComponent<Zone>().Cards.Remove(card);
                player.RangedZone.GetComponent<Zone>().Cards.Add(decoy);
            }
            else if(card.GameZone.Contains(ValidZone.Siege))
            {
                player.SiegeZone.GetComponent<Zone>().Cards.Remove(card);
                player.SiegeZone.GetComponent<Zone>().Cards.Add(decoy);
            }
        }
        else
        {
            Debug.Log("No hay cartas disponibles en el campo para ser cambiadas por el sennuelo");
        }
        
    }
    private Card CardWithGreaterPower(List<Card> list)
    {
        Card card = null;
        double GreaterPower = int.MinValue;
        foreach(var item in list)
        {
            if(item.Power > GreaterPower && item.Type == CardType.Plata)
            {
                card = item;
                GreaterPower = item.Power;
            }
        }
        return card;
    }
    #endregion
}