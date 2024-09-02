using System;
using System.Collections.Generic;
using UnityEngine;
class MethodWithParams : Expression
{
    public MethodWithParams(Expression exp, string method, Expression param, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
        this.param = param;
    }
    public Expression expression{ get; set;}
    public string method { get; set;}
    public Expression param {get; set;}
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        expression.CheckSemantic(context, scope, errors);
        param.CheckSemantic(context, scope, errors);
        System.Console.WriteLine(param + " " + param.Type);
        
        if(expression.Type == ExpressionType.List)
        {
            if(context.ListMethodsWithParams.ContainsKey(method))
            {
                Type = context.ListMethodsWithParams[method];

                //chequear el tipo del parametro
                if(method == "Find")
                {
                    if(param.Type != ExpressionType.LambdaExpression)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El metodo find de las listas debe recibir como parametro una expresion predicate"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                else
                {
                    if(param.Type != ExpressionType.Card)
                    {
                        System.Console.WriteLine("entro aqui");
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas reciben como parametro un objeto de tipo carta"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas no presentan el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
            
        }
        else if(expression.Type == ExpressionType.Context)
        {
            //revisar q contiene el metodo y actualizar el tipo de retorno
            if(context.ContextMethods.ContainsKey(method))
            {
                Type = context.ContextMethods[method];
                //chequear el tipo del parametro
                if(param.Type != ExpressionType.PlayerId)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Los parametros que reciben los metodos del context deben referirse a un jugador"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El contexto no contiene el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
            
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La expresion que declaro no presenta metodos para acceder"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override void Evaluate()
    {
        expression.Evaluate();
        Debug.Log(expression.Value + "valor d la expresion antes d un metodo con parametro");
        param.Evaluate();
        Debug.Log("valor del parametro " + param.Value);
        object ExpressionValue = expression.Value;
        if (ExpressionValue is ContextGame context)
        {
            Debug.Log("reconocio q es un contexto");
            Player player = ContextGame.contextGame.GetPlayer((int)param.Value);
            Debug.Log("player id " + player.ID);
            switch (method)
            {
                case "HandOfPlayer":
                    this.Value = ContextGame.contextGame.HandOfPlayer(player);
                    break;
                case "DeckOfPlayer":
                    this.Value = ContextGame.contextGame.DeckOfPlayer(player);
                    Debug.Log("asocio el value del deck " + this.Value);
                    break;
                case "FieldOfPlayer":
                    this.Value = ContextGame.contextGame.FieldOfPlayer(player);
                    break;
                case "GraveyardOfPlayer":
                    this.Value = ContextGame.contextGame.GraveyardOfPlayer(player);
                    break;
                default:
                    throw new Exception($"Metodo '{method}' invalido.");
            }
        }
        else if(ExpressionValue is List<Card> list)
        {
            Debug.Log("reconocio q es una lista, va a entrar al switch");
            switch(method)
            {
                case "Push":
                    Debug.Log("reconocio el push");
                    ContextGame.contextGame.Push((Card)param.Value, list);
                    Debug.Log("pusheo");
                    break;
                case "SendBottom":
                    ContextGame.contextGame.SendBottom((Card)param.Value, list);
                    break;
                case "Remove":
                    list.Remove((Card)param.Value);
                    break;
                case "Find":
                    //implementar find
                    //this.Value = ContextGame.contextGame.GraveyardOfPlayer(player);
                    break;
                default:
                    throw new Exception($"Metodo '{method}' invalido.");
            }
        }
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "(" + param + ")");
    }
}