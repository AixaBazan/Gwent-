using System;
using System.Collections.Generic;
using System.Linq;
public class Effect : AST
{
    Expression Name;
    public Token Targets {get; private set;}
    public Token Context {get; private set;}
    Stmt Body;
    public Scope AssociatedScope {get; private set;}
    public Dictionary <string, ExpressionType> EffectParams {get; private set;}
    List<(Token,Token)> Params;
    public Effect(Expression name, Token targets, Token context, Stmt body, List<(Token, Token)> Param, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.Targets = targets;
        this.Context = context;
        this.Body = body;
        this.Params = Param;
        this.EffectParams = new Dictionary <string,ExpressionType>();
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild(); //Se crea el scope del efecto
        
        //Se verifica que la expresion del nombre sea de tipo texto
        bool ValidName = Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "El nombre del efecto debe ser de tipo Texto"));
            return false;
        }
        //Se chequea q no se hayan declarado dos efectos con el mismo nombre
        Name.Evaluate();
        if(context.effects.ContainsKey((string)Name.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El efecto " + Name.Value + " ya existe"));
            return false;
        }

        //Se chequea que el tipo asociado a los parametros sean validos, y se actualiza el diccionario de EffectParams
        foreach(var param in Params)
        {
            if(context.ValidEffectParams.ContainsKey(param.Item2.Value))
            {
                this.EffectParams.Add(param.Item1.Value, context.ValidEffectParams[param.Item2.Value]);
                AssociatedScope.DefineType(param.Item1.Value, context.ValidEffectParams[param.Item2.Value]);
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El tipo asociado al parametro " + param.Item1.Value + " no es valido"));
                return false;
            }
        }

        //Se annaden al scope las variables que se le pasan al action como parametro
        if(AssociatedScope.GetType(Targets.Value) != ExpressionType.ErrorType || AssociatedScope.GetType(Context.Value) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable pasada al Action del efecto ya ha sido declarada"));
            return false;
        }
        AssociatedScope.DefineType(Targets.Value, ExpressionType.List);
        AssociatedScope.DefineType(Context.Value, ExpressionType.Context);

        //Se chequea semanticamente el cuerpo del action
        bool ValidAction = Body.CheckSemantic(context, AssociatedScope, errors);

        //Si esta bien se annade al contexto REVISAR
        if(ValidName && ValidAction)
        {
            Name.Evaluate();
            context.effects.Add((string)Name.Value, this);
        }
        return ValidName && ValidAction;
        
    }
    //correr el efecto
    public void Interprete()
    {
        Body.Interprete();
    }
    public override string ToString()
    {
        // Obtener el nombre del efecto
        string effectName = Name.ToString(); 

        // Obtener el cuerpo de la acción
        string actionBody = Body.ToString(); 

        // Formatear la representación de los parámetros
        string paramsRepresentation = Params.Count > 0 
        ? string.Join(", ", Params.Select(p => $"{p.Item1.Value}: {p.Item2.Value}")) 
        : "Sin parámetros";

        // Formatear la salida
        return $"effect\n{{\n  Name: \"{effectName}\",\n  Action: {actionBody},\n  Params: {{{paramsRepresentation}}}\n}}";
    }
}