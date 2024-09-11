using System;
using System.Collections.Generic;
using UnityEngine;
class Lambda : Expression
{
    public Variable Var {get; private set;}
    Expression Condition;
    Scope AssociatedScope;
    public Lambda(Variable variable, Expression condition, CodeLocation location) : base(location)
    {
        this.Var = variable;
        this.Condition = condition;
    }
    public override object Value{get; set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        //Se chequea si ya esta en uso la variable 
        if(AssociatedScope.GetType(Var.variable) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Var.Location, ErrorCode.Invalid, "La variable " + Var.variable + " ya ha sido declarada, no se puede usar como parametro de la expresion lambda"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        AssociatedScope.DefineType(Var.variable , ExpressionType.Card);

        //Chequeo semantico d la condicion
        bool validCond = Condition.CheckSemantic(context, AssociatedScope, errors);
        if(Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(Condition.Location, ErrorCode.Invalid, "La condicion de la expresion lambda debe devolver un booleano"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        this.Type = ExpressionType.LambdaExpression;
        return validCond;
    }
    public override void Evaluate()
    {
        Condition.Evaluate();
        this.Value = Condition.Value; 
    }
    public override string ToString()
    {
        return "( (" + Var.variable + ") => " + Condition + ")";
    }
}