using System;
using System.Collections.Generic;
public class Print : Stmt
{
    Expression expression;
    public override Scope AssociatedScope{get;set;}
    public Print(Expression exp, CodeLocation location) : base(location)
    {
        this.expression = exp;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        bool check = expression.CheckSemantic(context, AssociatedScope , errors);
        return check;
    }
    public override void Interprete()
    {
        expression.Evaluate();
        Console.WriteLine(expression.Value);
    }
    public override string ToString()
    {
        return $"print {expression.ToString()}";
    }
}