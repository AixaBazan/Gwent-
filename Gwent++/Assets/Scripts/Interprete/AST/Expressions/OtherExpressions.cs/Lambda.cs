using System;
using System.Collections.Generic;
class Lambda : Expression
{
    public Token Variable {get; private set;}
    public Expression Condition {get; private set;}
    public Lambda(Token variable, Expression condition, CodeLocation location) : base(location)
    {
        this.Variable = variable;
        this.Condition = condition;
    }
    public override object? Value{get; set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        if(scope.GetType(Variable.Value) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable " + Variable.Value + " ya ha sido declarada, no se puede usar como parametro de la expresion lambda"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        scope.DefineType(Variable.Value, ExpressionType.Card);
        bool validCond = Condition.CheckSemantic(context, scope, errors);
        if(Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La condicion de la expresion lambda debe devolver un booleano"));
            this.Type = ExpressionType.ErrorType;
            return false;
        }
        this.Type = ExpressionType.LambdaExpression;
        return validCond;
    }
    public override void Evaluate()
    {
        //revisar
        Condition.Evaluate();
        this.Value = Condition.Value;
    }
    public override string ToString()
    {
        return "( (" + Variable.Value + ") => " + Condition + ")";
    }
}