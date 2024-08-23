using System;
using System.Collections.Generic;
public abstract class Stmt : AST
{
    public Stmt(CodeLocation location) : base(location){ }
    public abstract void Interprete();
    public abstract Scope AssociatedScope{get; set;}
}