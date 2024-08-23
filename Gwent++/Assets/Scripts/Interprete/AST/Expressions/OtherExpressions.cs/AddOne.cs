using System;
using System.Collections.Generic;
class ModificOne : Expression
{
    public ModificOne(string variable, Token op, CodeLocation location) : base(location)
    {
        this.variable = variable;
        this.Operator = op;
    }
    public override object? Value{get; set;}
    public string variable { get; set;}
    public Token Operator { get; private set;}
    public override ExpressionType Type{get;set;}
    Scope AssociatedScope {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        if(AssociatedScope.GetType(variable) == ExpressionType.ErrorType) 
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La varible a la que desea acceder no se ha definido"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(AssociatedScope.GetType(variable) != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Solo se le puede aplicar ++ y -- a variables de tipo numero"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        this.Type = ExpressionType.Number;
        return true;
    }
    public override void Evaluate()
    {
        double value = (double)AssociatedScope.Get(variable);
        if(Operator.Value == TokenValue.addOne)
        { 
            AssociatedScope.Define(variable.ToString(), value + 1); 
        }
        else
        {
            AssociatedScope.Define(variable.ToString(), value - 1); 
        }
        this.Value = AssociatedScope.Get(variable.ToString());
        System.Console.WriteLine(variable + " valor: " + Value);
    }
    public override string ToString()
    {
        return String.Format("{0}",variable);
    }
}