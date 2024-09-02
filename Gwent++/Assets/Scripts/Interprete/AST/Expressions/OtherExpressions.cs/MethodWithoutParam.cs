using System;
using System.Collections.Generic;
class Method : Expression
{
    public Method(Expression exp, string method, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
    }
    public Expression expression{ get; set; }
    public string method { get; set; }
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
    {
        bool ValidExpression = expression.CheckSemantic(context, table, errors);
        if(expression.Type == ExpressionType.List)
        {
            if(context.ListMethodsWithoutParams.ContainsKey(method))
            {
                Type = context.ListMethodsWithoutParams[method];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas no contienen el metodo " + method));
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
        List<Card> PropValue = (List<Card>)expression.Value;
        if (PropValue is List<Card> list)
        {
            switch (method)
            {
                case "Pop":
                    this.Value = ContextGame.contextGame.Pop(PropValue);
                    break;
                case "Shuffle":
                    ContextGame.contextGame.Shuffle(PropValue);
                    break;
                default:
                    throw new Exception($"Metodo '{method}' invalido.");
            }
        }
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "()");
    }
}