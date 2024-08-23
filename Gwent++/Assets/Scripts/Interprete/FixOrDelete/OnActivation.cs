using System;
using System.Collections.Generic;
public class OnActivation : Stmt
{
    public override Scope AssociatedScope {get; set;}
    public List<AssignEffect> assignEffects{get; set;}
    public OnActivation(List<AssignEffect> effects, CodeLocation location) : base(location)
    {
        this.assignEffects = effects;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        foreach(var item in assignEffects)
        {
            bool valid = item.CheckSemantic(context, scope, errors);
            if(valid == false) return false;
        }
        return true;
    }
    public override void Interprete()
    {
        
    }
    public override string ToString()
    {
        string s = " \n";
        foreach(var item in assignEffects)
        {
            s += (item);
            s += " \n ";
        }
        return s;
    }
}