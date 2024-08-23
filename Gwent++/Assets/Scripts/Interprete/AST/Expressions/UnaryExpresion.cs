using System;
using System.Collections.Generic;
public abstract class UnaryExpression : Expression
{
    public UnaryExpression(CodeLocation location) : base(location){}
}