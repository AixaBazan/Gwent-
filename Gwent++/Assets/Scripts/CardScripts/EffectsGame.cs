using System.Collections.Generic;
using UnityEngine;
using System;
public enum ParticularEffect
{
    None,
    UserEffect,
    MultiplyAttack,
    Increase,
    Weather,
    CleanWeather,
    Decoy,
    DrawCard
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
            Debug.Log("Se activo el efecto del mult por n su ataque");
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

        }
        else if(card.effect == ParticularEffect.DrawCard)
        {
            ContextGame.contextGame.Stole(player);
        }
    }
    #region Card Effects
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
        player.HandZone.GetComponent<Zone>().Cards.Add(card);
        player.HandZone.GetComponent<Zone>().Cards.Remove(decoy);
        card.IsPlayed = false;
        if(card != null && card.GameZone.Contains(ValidZone.Melee))
        {
            player.MeleeZone.GetComponent<Zone>().Cards.Remove(card);
            player.MeleeZone.GetComponent<Zone>().Cards.Add(decoy);
        }
        else if(card != null && card.GameZone.Contains(ValidZone.Ranged))
        {
            player.RangedZone.GetComponent<Zone>().Cards.Remove(card);
            player.RangedZone.GetComponent<Zone>().Cards.Add(decoy);
        }
        else if(card != null && card.GameZone.Contains(ValidZone.Siege))
        {
            player.SiegeZone.GetComponent<Zone>().Cards.Remove(card);
            player.SiegeZone.GetComponent<Zone>().Cards.Add(decoy);
        }
    }
    private Card CardWithGreaterPower(List<Card> list)
    {
        Card card = null;
        double GreaterPower = int.MinValue;
        foreach(var item in list)
        {
            if(item.Power > GreaterPower)
            {
                card = item;
                GreaterPower = item.Power;
            }
        }
        return card;
    }
    #endregion
}