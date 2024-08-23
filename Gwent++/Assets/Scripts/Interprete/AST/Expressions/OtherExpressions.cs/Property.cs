using System;
using System.Collections.Generic;
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
       this.Value = Type.ToString();
    }
    public override string ToString()
    {
        return String.Format(expression + "." + Caller);
    }
}