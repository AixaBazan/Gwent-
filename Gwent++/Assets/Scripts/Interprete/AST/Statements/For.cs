using System;
using System.Collections.Generic;
using UnityEngine;
class For : Stmt
{
    string IterationVar;
    string Collection;
    Stmt Body;
    public override Scope AssociatedScope{get; set;}
    public For(string token, string collection, Stmt body, CodeLocation location) : base(location)
    {
        this.IterationVar = token;
        this.Collection = collection;
        this.Body = body;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        //Se crea un nuevo hijo para el scope
        this.AssociatedScope = scope.CreateChild();
        //Se chequea q no se haya declarado la variable a iterar antes
        if(AssociatedScope.GetType(IterationVar) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable " + IterationVar + " ya esta definida, no puede ser usada"));
            return false;
        }
        // Se define en el scope
        AssociatedScope.DefineType(IterationVar, ExpressionType.Card);
        // Se chequea que la lista a iterar este definida
        if(AssociatedScope.GetType(Collection) == ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La lista " + Collection + " no esta definida"));
            return false;
        }
        //Se chequea semanticamente el cuerpo del for
        bool ValidBody = Body.CheckSemantic(context, AssociatedScope, errors);
        return ValidBody;
    }
    //revisar
    public override void Interprete()
    {
        List<Card> collection = (List<Card>)AssociatedScope.Get(Collection);
        foreach(Card card in collection)
        {
            AssociatedScope.Define(IterationVar, card);
            Body.Interprete();
        }
    }
    public override string ToString()
    {
        return $"for ({IterationVar} in {Collection}) {{\n    {Body}\n}}";
    }
}