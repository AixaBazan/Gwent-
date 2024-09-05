using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Selector : Stmt
{
    public Expression Source { get; private set;}
    public Expression Single {get; private set;}
    public Expression Predicate {get; private set;}
    public List<Card> Value {get; private set;}
    public bool IsPostAction {get; set;} //En el selector necesito saber si el efecto q estoy analizando es un PostAction o no
    public Selector(Expression source, Expression single, Expression predicate, CodeLocation location) : base(location)
    {
        this.Source = source;
        this.Single = single;
        this.Predicate = predicate;
    }
    public override Scope AssociatedScope {get;set;}
    public Scope EffectScope {get;set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();   
        this.EffectScope = scope;

        //Chequeando el Source
        bool ValidSource = Source.CheckSemantic(context, AssociatedScope, errors);
        if(Source.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Source del Selector debe ser una expresion de tipo texto"));
            return false;
        }
        Source.Evaluate();

        if(!IsPostAction)//es un efecto normal no un PostAction
        {
            if(!context.ValidSource.Contains(Source.Value))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Source del efecto declarado es invalido"));
                return false;
            }
        }
        else
        {
            if((string)Source.Value != "parent" && !context.ValidSource.Contains(Source.Value))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Source del efecto PostAction declarado es invalido, postAct bool : " + IsPostAction));
                return false;
            }
        }

        //Chequeando el Single
        if(Single is null)
        {
            Single.Type  = ExpressionType.Boolean;
            Single.Value = false;
        }
        else
        {
            bool ValidSingle = Single.CheckSemantic(context, AssociatedScope, errors);
            if(Single.Type != ExpressionType.Boolean)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Single del Selector debe ser una expresion booleana"));
                return false;
            }
        }

        //Chequeando el Predicate
        Predicate.CheckSemantic(context, AssociatedScope, errors);
        if(Predicate.Type != ExpressionType.LambdaExpression)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Predicate debe recibir una expresion lambda"));
            return false;
        }
        return true;
    }
    public override void Interprete()
    {
        List<Card> cards = new List<Card>();
        Source.Evaluate();
        Single.Evaluate();

        //Se define la lista de cartas q es el objetivo
        switch (Source.Value)
        {
            case "board":
                cards = ContextGame.contextGame.Board;
                break;
            case "hand":
                cards = ContextGame.contextGame.Hand;
                break;
            case "otherHand":
                cards = ContextGame.contextGame.HandOfPlayer(ContextGame.contextGame.EnemyPlayer);
                break;
            case "deck":
                cards = ContextGame.contextGame.Deck;
                break;
            case "otherDeck":
                cards = ContextGame.contextGame.DeckOfPlayer(ContextGame.contextGame.EnemyPlayer);
                break;
            case "field":
                cards = ContextGame.contextGame.Field;
                break;
            case "otherField":
                cards = ContextGame.contextGame.FieldOfPlayer(ContextGame.contextGame.EnemyPlayer);
                break;
            case "parent":
                cards = EffectScope.EffectAndPostAction.Value1.selector.Value;
                Debug.Log("cogio su parent");
                foreach(var item in cards)
                {
                    Debug.Log(item);
                }
                Debug.Log("termino de recorrer la lista del padre");
                break;
            default:
                throw new Exception($"Sorce '{Source.Value}' invalido.");
        }

        //Se filtran las cartas segun el predicate y el single
        List<Card> filteredCards = new List<Card>();
        Lambda predicate = (Lambda)Predicate;
        foreach(Card card in cards)
        {
            AssociatedScope.Define(predicate.Var.variable , card);
            Predicate.Evaluate();
            Debug.Log("valor del predicate " + Predicate.Value);
            if((bool)Predicate.Value)
            {
                filteredCards.Add(card);
                if((bool)Single.Value == false)
                { 
                    break;
                }
            }
        }
        this.Value = filteredCards;
        Debug.Log("todo bien al seleccion el selector , cant d cartas : " + filteredCards.Count);
    }
    public override string ToString()
    {
        return "Selector: \n\t Source: " + Source +  "\n\t Single: " + Single + "\n\t Predicate: " + Predicate;
    }
}