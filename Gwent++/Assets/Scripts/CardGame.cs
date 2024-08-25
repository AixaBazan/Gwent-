using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    void Start()
    {
        this.context = GameObject.Find("Context").GetComponent<ContextGame>();
    }
    private ContextGame context;
    public Sprite Image;
    public string Description;
    public string Name;
    public double Power;
    public double OriginalPower;
    public string Faction;
    public CardType Type;
    public Player Owner 
    {
        get
        {
            if(this.Faction == "Fairies") return context.PlayerFairies.GetComponent<Player>().Id;
            else return context.PlayerDemons.GetComponent<Player>().Id;
        }
        set{}
    }
    public List<string> GameZone;
    //Efecto
    public List<AssignEffect> effects;
    public void ExecuteEffect()
    {

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