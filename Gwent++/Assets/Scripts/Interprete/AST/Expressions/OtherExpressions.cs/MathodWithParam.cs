using System;
using System.Collections.Generic;
class MethodWithParams : Expression
{
    public MethodWithParams(Expression exp, string method, Expression param, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
        this.param = param;
    }
    public Expression expression{ get; set;}
    public string method { get; set;}
    public Expression param {get; set;}
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        expression.CheckSemantic(context, scope, errors);
        param.CheckSemantic(context, scope, errors);
        System.Console.WriteLine(param + " " + param.Type);
        
        if(expression.Type == ExpressionType.List)
        {
            if(context.ListMethodsWithParams.ContainsKey(method))
            {
                Type = context.ListMethodsWithParams[method];

                //chequear el tipo del parametro
                if(method == "Find")
                {
                    if(param.Type != ExpressionType.LambdaExpression)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El metodo find de las listas debe recibir como parametro una expresion predicate"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                else
                {
                    if(param.Type != ExpressionType.Card)
                    {
                        System.Console.WriteLine("entro aqui");
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas reciben como parametro un objeto de tipo carta"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                }
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas no presentan el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
            
        }
        else if(expression.Type == ExpressionType.Context)
        {
            //revisar q contiene el metodo y actualizar el tipo de retorno
            if(context.ContextMethods.ContainsKey(method))
            {
                Type = context.ContextMethods[method];
                //chequear el tipo del parametro
                if(param.Type != ExpressionType.PlayerId)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Los parametros que reciben los metodos del context deben referirse a un jugador"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El contexto no contiene el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
            
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La expresion que declaro no presenta metodos para acceder"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override void Evaluate()
    {
       this.Value = Type.ToString();
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "(" + param + ")");
    }
}