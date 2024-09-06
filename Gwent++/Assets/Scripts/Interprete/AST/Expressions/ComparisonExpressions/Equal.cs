using System;
using System.Collections.Generic;
class Equal : BinaryExpression
{
    public override ExpressionType Type{get;set;}
    public override object Value{get;set;}
    public Equal(CodeLocation location, Expression left, Expression right) : base(location)
    {
        this.Right = right;
        this.Left = left;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        if(Right.Type == ExpressionType.Number && Left.Type == ExpressionType.Number)
        {
            this.Value = (double)this.Left.Value == (double)this.Right.Value;
        }
        else if(Right.Type == ExpressionType.Boolean && Left.Type == ExpressionType.Boolean)
        {
            this.Value = (bool)this.Left.Value == (bool)this.Right.Value;
        }
        else
        {
           this.Value = (string)this.Left.Value == (string)this.Right.Value;
        }
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        //revisar
        if(((Right.Type == ExpressionType.Number) && (Left.Type == ExpressionType.Number)) || 
        ((Right.Type == ExpressionType.Text) && (Left.Type == ExpressionType.Text)) ||
         ((Right.Type == ExpressionType.Boolean) && (Left.Type == ExpressionType.Boolean)))
        {
            Type = ExpressionType.Boolean;
            return right && left;
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Solo se permite comparar numeros, booleanos o textos"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} == {1})", Left, Right);
        }
        return Value.ToString();
    }
}
