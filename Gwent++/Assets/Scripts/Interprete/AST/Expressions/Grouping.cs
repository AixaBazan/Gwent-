using System;
using System.Collections.Generic;
public class Grouping : Expression
{
    public Expression expression{get; private set;}
    public Grouping(Expression exp, CodeLocation location):base(location)
    {
        this.expression = exp;
    }
    public override void Evaluate()
    {
        expression.Evaluate();
        this.Value = expression.Value;
    }
    public override object Value{get; set;}
    public override ExpressionType Type{get;set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool isValid = expression.CheckSemantic(context, scope, errors);
        if(expression.Type == ExpressionType.Number)
        {
            this.Type = ExpressionType.Number;
            return isValid;
        }
        else if(expression.Type == ExpressionType.Text)
        {
            this.Type = ExpressionType.Text;
            return isValid;
        }
        else if(expression.Type == ExpressionType.Boolean)
        {
            this.Type = ExpressionType.Boolean;
            return isValid;
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Expression entre parentesis invalida"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0})", expression);
        }
        return Value.ToString();
    }
}