using System;
using System.Collections.Generic;
using UnityEngine;
class Var : Stmt
{
    Expression Name;
    Expression InitialValue;
    Token Operator;
    public override Scope AssociatedScope {get; set;}
    public Var(Expression name, Expression initializer, Token op, CodeLocation location) : base(location)
    {
        Name = name;
        InitialValue = initializer;
        Operator = op;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;

        InitialValue.CheckSemantic(context, AssociatedScope, errors);
        if(Name is Variable)
        {
            if(Operator.Value == TokenValue.Assign)
            {
                AssociatedScope.DefineType(Name.ToString(), InitialValue.Type);  
                return true;
            }
            if(Operator.Value == TokenValue.Increase || Operator.Value == TokenValue.Decrease)
            {
                if(AssociatedScope.GetType(Name.ToString()) != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "No se pueden incrementar o decrementar el valor de una variable que no sea tipo Number o que no exista"));
                    return false;
                }
                if(InitialValue.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "No se pueden incrementar o decrementar el valor de una variable en una expresion q no sea de tipo Number"));
                    return false;
                }
                else return true;
            }
        }
        else if(Name is Property)
        {
            Name.CheckSemantic(context, AssociatedScope, errors);
            if(Name.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "Solo se permite modificar la propiedad Power de la carta"));
                return false;
            }
            if(InitialValue.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(InitialValue.Location, ErrorCode.Invalid, "No se pueden incrementar o decrementar el valor de una variable en una expresion q no sea de tipo Number"));
                return false;
            }
            return true; 
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Solo se puede asignar, incrementar o decrementar variables y la propiedad Power de las cartas"));
        }
        return false;
    }   
    public override void Interprete()
    {
        InitialValue.Evaluate();

        if(Name is Variable)
        {
            if(Operator.Value == TokenValue.Assign) 
            {
                AssociatedScope.Define(Name.ToString(), InitialValue.Value);
                return;
            }
            double actualValue = (double)AssociatedScope.Get(Name.ToString());
            if (Operator.Value == TokenValue.Increase)
                AssociatedScope.Define(Name.ToString(), actualValue + (double)InitialValue.Value);
            else if(Operator.Value == TokenValue.Decrease)
                AssociatedScope.Define(Name.ToString(), actualValue - (double)InitialValue.Value);
        }
        else if(Name is Property)
        {
            Name.Evaluate();
            Property property = (Property)Name;
            Card card = (Card)property.expression.Value;
            Debug.Log(card.Name);
            if(Operator.Value == TokenValue.Assign)
            {
                card.Power = (double)InitialValue.Value;
                return;
            }
            if (Operator.Value == TokenValue.Increase)
                card.Power = (double)Name.Value + (double)InitialValue.Value;
            else if(Operator.Value == TokenValue.Decrease)
                card.Power = (double)Name.Value - (double)InitialValue.Value;
        }  
    }
    public override string ToString()
    {
        if(InitialValue != null) return Name + "=" + InitialValue + ";";
        return Name.ToString();
    }
}