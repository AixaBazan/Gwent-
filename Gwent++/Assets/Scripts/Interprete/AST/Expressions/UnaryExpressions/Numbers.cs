using System.Linq.Expressions;
using System;
using System.Collections.Generic;
sealed class Number : UnaryExpression
{
    public Number(double value, CodeLocation location) : base(location)
    {
        Value = value;
    }
    public override object? Value{get; set;}
    public override ExpressionType Type
    {
        get { return ExpressionType.Number; }
        set{}
    }
    public bool IsNumber
    {
        get
        {
            int n;
            return int.TryParse(Value.ToString(), out n);
        }
    }
    public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {
        
    }
    public override string ToString()
    {
        return String.Format("{0}",Value);
    }
}