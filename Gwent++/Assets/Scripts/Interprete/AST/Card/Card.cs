using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

public class CardComp : AST
{
    public Expression Name{get;private set;}
    public Expression Power{get;private set;}
    public Expression Faction{get;private set;}
    public List<Expression> Range{get;private set;}
    public Expression Type{get;private set;}
    public List<AssignEffect> OnActivation {get; private set;}
    public Scope AssociatedScope { get; set;}
    List<string> range {get; set;}
    public CardComp(Expression name, Expression type, Expression faction, Expression power, List<Expression> range, List<AssignEffect> activation, CodeLocation location):base(location)
    {
        this.Name = name;
        this.Type = type;
        this.Faction = faction;
        this.Power = power;
        this.Range = range;
        this.OnActivation = activation;
        this.range = new List<string>();
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        //Cada carta tiene su propio scope
        this.AssociatedScope = scope.CreateChild();

        //Se chequean las distintas propiedades de la carta y q sean del tipo valido
        //Poder
        //preguntar si es valido ponerlo negativo
        Power.CheckSemantic(context, AssociatedScope, errors);
        if (Power.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El poder de la carta debe ser una expresion de tipo numero"));
            return false;
        }

        //Tipo
        Type.CheckSemantic(context, AssociatedScope, errors);
        if(Type.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El tipo de la carta debe ser una expresion de tipo texto"));
            return false;
        }
        //Chequear q pusieran un tipo valido
        Type.Evaluate();
        if(!context.ValidType.Contains((string)Type.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Tipo de carta invalido, se espera Oro, Plata, Aumento, Clima o Lider"));
            return false;
        }

        //Nombre
        Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre de la carta debe ser una expresion de tipo texto"));
            return false;
        }

        //Faccion
        Faction.CheckSemantic(context, AssociatedScope, errors);
        if(Faction.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La faccion de la carta debe ser una expresion de tipo texto"));
            return false;
        } 
        //Chequear q pusieran una faccion valida
        Faction.Evaluate();
        if(((string)Faction.Value != "Fairies") || ((string)Faction.Value != "Demons"))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las facciones para las cartas disponibles son: Fairies y Demons"));
            return false;
        }

        //Range
        foreach(var item in Range)
        {
            item.CheckSemantic(context, AssociatedScope, errors);
            if(item.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Se esperaba una expresion de texto en el Range de la carta"));
                return false;
            }
            item.Evaluate();
            if(!context.ValidRange.Contains((string)item.Value))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Range invalido, se espera: Melee, Ranged o Siege"));
                return false;
            }
            range.Add((string)item.Value);
        }

        //OnActivation 
        foreach(var item in OnActivation)
        {
            bool valid = item.CheckSemantic(context, scope, errors);
            if(valid == false) return false;
        }

        // Como esta todo en orden se annade la carta al contexto
        context.cards.Add((string)Name.Value);

        return true;
    }
    //Falta interpretar selector y efectos
    public void CardBuilder()
    {
        Name.Evaluate();
        Power.Evaluate();
        //Creando las instancias de la carta segun su tipo:
    }
    //Arreglar ToString
    public override string ToString()
    { 
        return String.Format("Card {0} \n\t Power: {1} \n\t Faction: {2} \n\t Type: {3} \n\t OnActivation: ", Name, Power, Faction, Type);
    }
}