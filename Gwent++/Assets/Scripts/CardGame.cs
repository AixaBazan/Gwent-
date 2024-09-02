using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Sprite Image;
    public string Description;
    public string Name;
    public double Power;
    public double OriginalPower;
    public string Faction;
    public CardType Type;
    public int Owner 
    {
        get
        {
            if(this.Faction == "Fairies")  return 1;
            else return 2;
        }
        set{}
    }
    public List<string> GameZone;
    //Efecto
    public List<AssignEffect> effects {get; set;}
    public void ExecuteEffect()
    {
        foreach(var effect in effects)
        {
            effect.Interprete();
        }
    }
}
public enum CardType
{
    Oro,
    Plata,
    Aumento,
    Lider,
    Clima
}