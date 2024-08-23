using System;
using System.Collections.Generic;
//El enum ExpressionType agrupa los tipos de expresiones
public enum ExpressionType
{
    Anytype,
    Identifier,
    Text,
    Number,
    Boolean,
    Card,
    List,
    Context,
    Function,
    LambdaExpression,
    PlayerId,
    ErrorType
}
