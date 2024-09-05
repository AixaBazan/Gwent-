using System;
using System.Collections.Generic;
using UnityEngine;
public class ValuePair
{
    public AssignEffect Value1 { get; set; }
    public AssignEffect Value2 { get; set; }

    public ValuePair(AssignEffect value1, AssignEffect value2)
    {
        Value1 = value1;
        Value2 = value2;
    }
}
public class Scope 
{
    public ValuePair EffectAndPostAction;
    private Dictionary<string, object> values = new Dictionary<string, object>(); //diccionario nombre de la variable - valor
    private Dictionary<string, ExpressionType> types = new Dictionary<string, ExpressionType>(); // diccionario nombre de la variable - tipo
    public Scope? Parent {get; private set;}
    public Scope()
    {
        Parent = null; 
    }
    public Scope CreateChild()
    {
        Scope child = new Scope();
        child.Parent = this;   
        return child;
    }
    public object? Get(string name) 
    {
        if (values.ContainsKey(name))
        { 
            return values[name];  
        }
        else if(Parent != null)
        {
            return Parent.Get(name);
        }
        else 
        {
            Debug.Log("No se en contro la variable " + name);
            return null;
        }
    }
    public ExpressionType GetType(string name)
    {
        if (types.ContainsKey(name))
        {
            return types[name];
        }
        else if (Parent != null)
        {
            return Parent.GetType(name);
        }
        else
        {
            //retorno ErrorType si la variable no se encuentra en el scope
            return ExpressionType.ErrorType; 
        }
    }
    public void Define(string name, object value)
    {
        if (values.ContainsKey(name))
        {
            values[name] = value;
            return;
        }
        else if (Parent != null)
        {
            if(AssignValue(name, value))
            {
                return;
            }
            else values.Add(name,value);
        }
        else values.Add(name,value);
    }
    private bool AssignValue(string name, object value)
    {
        if(values.ContainsKey(name))
        {
            values[name] = value;
            return true;
        }
        if (Parent != null)
        {
            return Parent.AssignValue(name, value); // Llama al método assign en el entorno padre
        }
        return false;
    }
    
    public void DefineType(string name, ExpressionType type)
    {
        if(types.ContainsKey(name))
        {
            types[name] = type;
        }
        else if(Parent != null)
        {
            if(AssignType(name, type))
            {
                return;
            }
            else types.Add(name, type);
        }
        else types.Add(name,type);
    }
    private bool AssignType(string name, ExpressionType type)
    {
        if(types.ContainsKey(name))
        {
            types[name] = type;
            return true;
        }
        if (Parent != null)
        {
            return Parent.AssignType(name, type); // Llama al método assign en el entorno padre
        }
        return false;
    }
    
}