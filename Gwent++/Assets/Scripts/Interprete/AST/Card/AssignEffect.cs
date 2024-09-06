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
    public Effect RefEffect {get; set;}  
    public PostAction postAction {get; set;}
    public AssignEffect(Expression name, List<(string, Expression)> param, Selector sel, PostAction post, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.Params = param;
        this.selector = sel;
        this.postAction = post;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        //Se verifica que la expresion del nombre sea de tipo texto
        Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "El nombre del efecto debe ser de tipo Texto"));
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

        //Se chequea que el Selector este bien semanticamente y que se haya declarado
        selector.IsPostAction = false; //no es un postAction
        if(selector is null)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Debe declarar el Selector en el efecto de la carta" ));
            return false;
        }
        bool ValidSel = selector.CheckSemantic(context, AssociatedScope, errors);

        //Se chequea el PostAction
        //Si no tiene Selector se le asocia este selector
        if(postAction != null)
        {
            postAction.Effect.selector.IsPostAction = true; //el efecto tiene un postAction y se le informa al selector q lo es
            if(postAction.Effect.selector is null)
            {
                postAction.Effect.selector = this.selector;
            }
            postAction.CheckSemantic(context, AssociatedScope, errors);
            postAction.AssociatedScope.EffectAndPostAction = new ValuePair(this, postAction.Effect);
        }
        return ValidSel;
    }
    public override void Interprete()
    {
        selector.Interprete();
        string TargetsName = RefEffect.Targets.Value;
        string ContextName = RefEffect.Context.Value;

        //Se annade al scope del efecto la lista d cartas a operar
        RefEffect.AssociatedScope.Define(TargetsName, selector.Value);
        //Se annade al scope del efecto el contexto del juego
        RefEffect.AssociatedScope.Define(ContextName, ContextGame.contextGame);
        //Se corre el efecto
        RefEffect.Interprete();
        //Se corre el PostAction(Si tiene)
        if(postAction != null)
        {
            Debug.Log("Se va a correr el PostAction");
            postAction.Interprete();
        }
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

public class PostAction : Stmt
{
    public AssignEffect Effect {get; private set;}
    public PostAction(AssignEffect PostActionEffect, CodeLocation location) : base(location)
    {
        this.Effect = PostActionEffect;
    }
    public override Scope AssociatedScope {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        //Se verifica que la expresion del nombre sea de tipo texto
        Effect.Name.CheckSemantic(context, AssociatedScope, errors);
        if(Effect.Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Effect.Name.Location, ErrorCode.Invalid, "El nombre del efecto del PostAction debe ser de tipo Texto"));
            return false;
        }
        //Se verifica que se haya declarado el efecto antes
        Effect.Name.Evaluate();
        if(!context.effects.ContainsKey((string)Effect.Name.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El efecto que le desea asignar a la carta no existe, debe declararlo"));
            return false;
        }
        else
        {
            //Se asocia el efecto
            Effect.RefEffect = context.effects[(string)Effect.Name.Value];
        }

        //Se chequea si se asociaron todos los parametros correctamente y se annaden al scope del efecto
        if(Effect.Params.Count != Effect.RefEffect.EffectParams.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Parametros asignados incorrectamente"));
            return false;
        }
        foreach(var item in Effect.Params)
        {
            if(Effect.RefEffect.EffectParams.ContainsKey(item.Item1))
            {
                item.Item2.CheckSemantic(context, AssociatedScope, errors);
                if(item.Item2.Type == Effect.RefEffect.EffectParams[item.Item1])
                {
                    Effect.RefEffect.AssociatedScope.Define(item.Item1, item.Item2.Value);
                    Effect.RefEffect.EffectParams.Remove(item.Item1);
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
        //Se chequea el Selector
        Effect.selector.CheckSemantic(context, AssociatedScope, errors);

        //Se chequea el PostAction(si tiene)
        if(Effect.postAction != null)
        {
            Effect.postAction.Effect.selector.IsPostAction = true;
            Effect.postAction.CheckSemantic(context, AssociatedScope, errors);
        }
        return true;
    }
    public override void Interprete()
    {
        Effect.Interprete();
    }
    public override string ToString()
    {
        return Effect.ToString();
    }
}