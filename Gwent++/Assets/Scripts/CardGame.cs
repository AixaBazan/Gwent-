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
    public bool IsPlayed = false;
    public int Owner 
    {
        get
        {
            if(this.Faction == "Fairies")  return 1;
            else return 2;
        }
        set{}
    }
    public List<string> Range;
    public List<ValidZone> GameZone;
    //Efecto
    public ParticularEffect effect;
    public List<AssignEffect> effects {get; set;}
    public void ExecuteEffect()
    {
        Effects.Instance.RunEffect(this);
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
public enum ValidZone
{
    Melee,
    Ranged,
    Siege,
    IncreaseMelee,
    IncreaseRanged,
    IncreaseSiege,
    WeatherZone
}

