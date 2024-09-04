using System;
using System.Collections.Generic;
using UnityEngine;
public class Property : Expression
{
    public Property(Expression exp, string caller, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.Caller = caller;
    }
    public Expression expression{ get; set; }
    public string Caller { get; set; }
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
    {
        bool ValidExpression = expression.CheckSemantic(context, table, errors);
        if(expression.Type == ExpressionType.Card)
        {
            if(context.cardProperties.ContainsKey(Caller))
            {
                Type = context.cardProperties[Caller];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La carta no contiene la propiedad " + Caller));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else if(expression.Type == ExpressionType.Context)
        {
            if(context.contextProperties.ContainsKey(Caller))
            {
                Type = context.contextProperties[Caller];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El contexto no contiene la propiedad " + Caller));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La expresion que declaro no tiene propiedades para acceder"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override void Evaluate()
    {
        expression.Evaluate();
        object PropValue = expression.Value;
        Debug.Log(PropValue);
        if (PropValue is Card unit)
        {
            switch (Caller)
            {
                case "Name":
                    this.Value = unit.Name;
                    break;
                case "Faction":
                    this.Value = unit.Faction.ToString();
                    break;
                case "Power":
                    this.Value = unit.Power;
                    break;
                case "Type":
                    this.Value = unit.Type.ToString();
                    break;
                case "Owner":
                    this.Value = unit.Owner;
                    break;
                default:
                    throw new Exception($"Propiedad '{Caller}' invalida.");
            }
        }
        else if(PropValue is ContextGame context)
        {
            switch (Caller)
            {
                case "Board":
                    this.Value = ContextGame.contextGame.Board;
                    break;
                case "TriggerPlayer":
                    this.Value = (int)ContextGame.contextGame.TriggerPlayer.ID;
                    break;
                case "Hand":
                    this.Value = ContextGame.contextGame.HandOfPlayer(ContextGame.contextGame.TriggerPlayer); 
                    break;
                case "Field":
                    this.Value = ContextGame.contextGame.FieldOfPlayer(ContextGame.contextGame.TriggerPlayer); 
                    break;
                case "Deck":
                    this.Value = ContextGame.contextGame.DeckOfPlayer(ContextGame.contextGame.TriggerPlayer); 
                    break;
                case "Graveyard":
                    this.Value = ContextGame.contextGame.GraveyardOfPlayer(ContextGame.contextGame.TriggerPlayer); 
                    break;
                default:
                    throw new Exception($"Property '{Caller}' not found.");
            }
        }
    }
    public override string ToString()
    {
        return String.Format(expression + "." + Caller);
    }
}