using System;
using System.Collections.Generic;
public abstract class AST
{
    public CodeLocation Location {get; set;}
    public abstract bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors);
    public AST(CodeLocation location)
    {
        Location = location;
    }
}