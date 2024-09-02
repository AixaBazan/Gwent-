using System;
using System.Collections.Generic;
class Variable: UnaryExpression
{
    public Variable(string variable, CodeLocation location) : base(location)
    {
        this.variable = variable;
    }
    public override object? Value{get; set;}
    public string variable { get; set;}
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
        this.Type = AssociatedScope.GetType(variable);
        return true;
    }
    public override void Evaluate()
    {
        this.Value = AssociatedScope.Get(variable);
    }
    public override string ToString()
    {
        return String.Format("{0}",variable);
    }
}