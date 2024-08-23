using System;
using System.Collections.Generic;
class Or : BinaryExpression
{
    public override ExpressionType Type{get;set;}
    public override object? Value{get;set;}
    public Or(CodeLocation location, Expression left, Expression right) : base(location)
    {
        this.Right = right;
        this.Left = left;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        
        this.Value = (bool)this.Left.Value || (bool)this.Right.Value;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if((Right.Type != ExpressionType.Boolean) || (Left.Type != ExpressionType.Boolean))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las expresiones deben ser booleanas"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Boolean;
        return right && left;
    }
}