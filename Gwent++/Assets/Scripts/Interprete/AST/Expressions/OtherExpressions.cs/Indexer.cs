using System;
using System.Collections.Generic;
using UnityEngine;
public class Indexer : Expression
{
    public Indexer(Expression exp, double index, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.Index = index;
    }
    Expression expression;
    double Index;
    public override object Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool validExp = expression.CheckSemantic(context, scope, errors);
        if(expression.Type != ExpressionType.List)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Solo se permite indexar en listas"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        //verificar q index sea un numero entero
        if(Index != (int)Index)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El index debe ser un numero entero"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Card;
        return true;
    }
    public override void Evaluate()
    {
        expression.Evaluate();
        List<Card> list = (List<Card>)expression.Value;
        this.Value = list[(int)Index];
    }
    public override string ToString()
    {
        return String.Format(expression + "[" + Index + "]");
    }
}