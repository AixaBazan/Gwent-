using System;
using System.Collections.Generic;
public class Div : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object Value {get; set;}
    public Div(CodeLocation location, Expression left, Expression right) : base(location)
    {
        this.Right = right;
        this.Left = left;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        Value = (double)Left.Value / (double)Right.Value;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
        {
            //Poner en ingles
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No se pueden dividir dos tipos que no sean numericos"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        else if((double)Right.Value == 0)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No se pueden dividir entre cero"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Number;
        return right && left;
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} / {1})", Left, Right);
        }
        return Value.ToString();
    }
}