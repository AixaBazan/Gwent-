using System;
using System.Collections.Generic;
class Bool: UnaryExpression
{
    public Bool(bool value, CodeLocation location) : base(location)
    {
        Value = value;
    }
    public override object? Value{get; set;}
    public override ExpressionType Type
    {
        get { return ExpressionType.Boolean; }
        set{}
    }
    public bool IsBool
    {
        get
        {
            return Value is bool;
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