using System;
using System.Collections.Generic;
using UnityEngine;
class Lambda : Expression
{
    public  Variable Var {get; private set;}
    public Expression Condition {get; private set;}
    public Lambda(Variable variable, Expression condition, CodeLocation location) : base(location)
    {
        this.Var = variable;
        this.Condition = condition;
    }
    public override object? Value{get; set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        //Se chequea si ya esta en uso la variable 
        if(scope.GetType(Var.variable) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable " + Var.variable + " ya ha sido declarada, no se puede usar como parametro de la expresion lambda"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        scope.DefineType(Var.variable , ExpressionType.Card);

        //Chequeo semantico d la condicion
        bool validCond = Condition.CheckSemantic(context, scope, errors);
        if(Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La condicion de la expresion lambda debe devolver un booleano"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        this.Type = ExpressionType.LambdaExpression;
        return validCond;
    }
    public override void Evaluate()
    {
        Debug.Log("Entro a evaluar el predicate");
        //Var.Evaluate(); //se le asocia su valor a la carta
        Debug.Log("No paso del Var.Evaluate");
        Condition.Evaluate(); //se evalua la condicion
        this.Value = Condition.Value; //el valor es el resultado d evaluar la condicion

    }
    public override string ToString()
    {
        return "( (" + Var.variable + ") => " + Condition + ")";
    }
}