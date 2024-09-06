using System;
using System.Collections.Generic;
//La clase context contiene las propiedades a la q se puede acceder cada tipo
public class Context
{
    //efectos q se van declarando
    public Dictionary<string, Effect> effects;
    //cartas que se van declarando
    public List<string> cards;

    public Context()
    {
        effects = new Dictionary<string, Effect>();
        cards = new List<string>();
    }
    public Dictionary<string, ExpressionType> contextProperties = new Dictionary<string, ExpressionType>
    {
        {"Hand" , ExpressionType.List}, {"Field", ExpressionType.List}, {"Graveyard", ExpressionType.List},
        {"TriggerPlayer", ExpressionType.PlayerId}, {"Board", ExpressionType.List}, {"Deck", ExpressionType.List}
    };
    public Dictionary<string, ExpressionType> ContextMethods = new Dictionary<string, ExpressionType>
    {
        {"HandOfPlayer", ExpressionType.List}, {"FieldOfPlayer", ExpressionType.List}, {"GraveyardOfPlayer", ExpressionType.List},
        {"DeckOfPlayer", ExpressionType.List}
    };
    public Dictionary<string, ExpressionType> cardProperties = new Dictionary<string, ExpressionType>
    {
        {"Name", ExpressionType.Text}, {"Faction", ExpressionType.Text}, {"Type", ExpressionType.Text},
        {"Range", ExpressionType.List}, {"Owner", ExpressionType.PlayerId}, {"Power", ExpressionType.Number}
    };
    public Dictionary<string, ExpressionType> ListMethodsWithParams = new Dictionary<string, ExpressionType>
    {
        {"Find", ExpressionType.List}, {"Push", ExpressionType.Function}, {"SendBottom", ExpressionType.Function},
        {"Remove", ExpressionType.Function}
    };
    public Dictionary<string, ExpressionType> ListMethodsWithoutParams  = new Dictionary<string, ExpressionType>
    {
        {"Pop", ExpressionType.Card}, //tambien quita la carta de la lista
        {"Shuffle", ExpressionType.Function}
    };
    public Dictionary<string, ExpressionType> ValidEffectParams  = new Dictionary<string, ExpressionType>
    {
        {"Number", ExpressionType.Number},
        {"String", ExpressionType.Text}, {"Bool", ExpressionType.Boolean}
    };
    public string[] ValidType = new string[]{"Oro", "Plata", "Aumento", "Clima", "Lider"};
    public string[] ValidRange = new string[]{"Melee", "Siege", "Ranged"};
    public string[] ValidSource = new string[]{"hand", "otherHand", "deck", "otherDeck", "field", "otherField", "board"};
}