using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AssignEffect : Stmt
{
    public Expression Name {get; private set;}
    public List<(string, Expression)> Params {get; private set;}
    public Selector selector {get; private set;}
    public override Scope AssociatedScope { get; set;}
    public Effect RefEffect {get; private set;}  
    public AssignEffect(Expression name, List<(string, Expression)> param, Selector sel, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.Params = param;
        this.selector = sel;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        //Se verifica que la expresion del nombre sea de tipo texto
        Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre del efecto debe ser de tipo Texto"));
            return false;
        }
        //Se verifica que se haya declarado el efecto antes
        Name.Evaluate();
        if(!context.effects.ContainsKey((string)Name.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El efecto que le desea asignar a la carta no existe, debe declararlo"));
            return false;
        }
        else
        {
            //Se asocia el efecto
            this.RefEffect = context.effects[(string)Name.Value];
        }

        //Se chequea si se asociaron todos los parametros correctamente y se annaden al scope del efecto
        if(Params.Count != RefEffect.EffectParams.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Parametros asignados incorrectamente"));
            return false;
        }
        foreach(var item in Params)
        {
            if(RefEffect.EffectParams.ContainsKey(item.Item1))
            {
                item.Item2.CheckSemantic(context, AssociatedScope, errors);
                if(item.Item2.Type == RefEffect.EffectParams[item.Item1])
                {
                    RefEffect.AssociatedScope.Define(item.Item1, item.Item2.Value);
                    RefEffect.EffectParams.Remove(item.Item1);
                }
                else
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Debe asignar como valor del parametro " + item.Item1 + " una expresion del tipo declarado en el efecto anteriormente" ));
                    return false;
                }
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable " + item.Item1 + " no es un parametro valido, ya que no fue declarado en el efecto" ));
                return false;
            }
        }

        //Se chequea que el Selector este bien semanticamente
        if(selector is null)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Debe declarar el Selector en el efecto de la carta" ));
            return false;
        }
        bool ValidSel = selector.CheckSemantic(context, AssociatedScope, errors);

        return ValidSel;
    }
    public override void Interprete()
    {
        selector.Interprete();
        string TargetsName = RefEffect.Targets.Value;
        string ContextName = RefEffect.Context.Value;

        //Se annade al scope del efecto la lista d cartas a operar
        Debug.Log("Se annade las targets y el context "+ TargetsName + "  " + ContextName);
        RefEffect.AssociatedScope.Define(TargetsName, selector.Value);
        //Se annade al scope del efecto el contexto del juego
        RefEffect.AssociatedScope.Define(ContextName, ContextGame.contextGame);
        Debug.Log("Se annadio bien las targets y el selector, voy a probrae acceder a ellas ahora");
        object t = RefEffect.AssociatedScope.Get(TargetsName);
        object c = RefEffect.AssociatedScope.Get(ContextName);
        if(t == null) Debug.Log("targets en assign effect es null");
        if(c == null) Debug.Log("context en assign effect es null");

        Debug.Log("Todo bien hasta correr el efecto");
        //Se corre el efecto
        RefEffect.Interprete();
    }
    public override string ToString()
    {
        var effectString = $"Effect: {{\n" + $"    Name: \"{Name}\",\n";
        // Formatear la representación de los parámetros
        string paramsRepresentation = Params.Count > 0 
        ? string.Join(", ", Params.Select(p => $"{p.Item1}: {p.Item2.Value}")) 
        : "Sin parámetros";

    // Combine both parts
    return $"{effectString}\n{paramsRepresentation}\n" + selector + "\n }" ;
    }
}