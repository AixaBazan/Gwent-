using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Block : Stmt
{
    public List<Stmt> Statements { get; } // Lista de declaraciones en el bloque
    public Block(List<Stmt> statements, CodeLocation location): base(location)
    {
        Statements = statements; // Inicializa la lista de declaraciones
    }
    public override Scope AssociatedScope { get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        foreach(Stmt item in Statements)
        {
            bool valid = item.CheckSemantic(context, AssociatedScope , errors);
            if( valid == false)
            {
                return false;
            }
        }
        return true;
    }
    public override void Interprete()
    {
        foreach(Stmt item in Statements)
        {
            item.Interprete();
        }
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Block : {");

        foreach (Stmt item in Statements)
        {
            sb.AppendLine("  " + item.ToString());
        }

        sb.AppendLine("} EndBlock");

        return sb.ToString();
    }
}