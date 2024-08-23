using System;
using System.Collections.Generic;
public abstract class Card
{
    public abstract string Name { get; set; }
    public abstract double Power { get; set; }
    public abstract string Faction {get; set;}
    public abstract CardType Type { get; set;}
    public int Owner 
    {
        get
        {
            if(this.Faction == "Fairies") return 1;
            else return 2;
        }
        set{}
    }
    public abstract List<string> GameZone { get; set; } 
    //Efecto
    public List<AssignEffect> effects {get; set;}
    public void ExecuteEffect()
    {

    }
}
public class UnitCard : Card
{
    public override string Name { get; set; }
    public override double Power { get; set; }
    public override string Faction { get; set; }
    public override CardType Type { get; set;}
    public override List<string> GameZone { get; set; } 
    public UnitCard(string name, double power, string faction, CardType type, List<string> gameZone)
    {
        this.Name = name;
        this.Power = power;
        this.Faction = faction;
        this.Type = type;
        this.GameZone = gameZone;
    }
}
public class SpecialCard : Card
{
    public override string Name { get; set; }
    public override double Power { get {return 0;} set{} }
    public override string Faction { get; set; }
    public override CardType Type { get; set;}
    public override List<string> GameZone { get; set; } 

    public SpecialCard(string name, string faction, CardType type)
    {
        this.Name = name;
        this.Faction = faction;
        this.Type = type;
        this.GameZone = new List<string>();
        UpdateGameZone();
    }
    private void UpdateGameZone()
    {
        if(Type == CardType.Clima)
        {
            GameZone.Add("ZonaClima");
        }
        //verificar las aumento
    }
}
public class LeaderCard : Card
{
    public override string Name { get; set; }
    public override double Power { get {return 0;} set{} }
    public override string Faction { get; set; }
    public override CardType Type{ get {return CardType.Lider;} set{}}
    public override List<string> GameZone { get; set; } //revisar
    public LeaderCard(string name, string faction)
    {
        this.Name = name;
        this.Faction = faction;
        this.GameZone = new List<string>();
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