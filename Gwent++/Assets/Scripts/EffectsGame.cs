using System.Collections.Generic;
using UnityEngine;
public enum ParticularEffect
{
    None,
    UserEffect,
    MultiplyAttack,
    Bonus,
    Weather
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
            MultiplyAttack(player, card);
        }
        else if(card.effect == ParticularEffect.Bonus)
        {

        }
        else if(card.effect == ParticularEffect.Weather)  
        {

        }
    }
    #region Card Effects
    private void MultiplyAttack(Player player, Card card)
    {
        List<Card> field = player.Field();
        int counter = 0;
        foreach(Card item in field)
        {
            if(card.Name == item.Name) counter ++;
        }
        card.Power = card.Power * counter;
    }
    private void ModificAttack()
    {

    }

    #endregion
}