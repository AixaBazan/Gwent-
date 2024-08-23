using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
class Parser
{
    private TokenStream Stream;
    public List<CompilingError> errors {get; private set;}
    public Parser(TokenStream stream) 
    {
        this.Stream = stream;
        this.errors = new List<CompilingError>();
    }
    bool PanicMode(CompilingError error, string breaker = TokenValue.ClosedCurlyBracket)
    {
        errors.Add(error);
        while (!Stream.Match(breaker))
        {
            if (Stream.Peek().Type == TokenType.End || Stream.Peek().Value == TokenValue.ClosedBracket) return true;
            else Stream.MoveNext(1);
        }

        return false;
    }
    #region Gwent-Parse
    //Parse GwentProgram
    public GwentProgram ParseProgram()
    {
        GwentProgram program = new GwentProgram(new CodeLocation());

        while (!Stream.Match(TokenType.End))
        {
            try
            {
                if (Stream.Match(TokenValue.declareEffect)) 
                {
                    if(Stream.Match(TokenValue.OpenCurlyBracket))
                    {
                        program.Effects.Add(ParseEffect(Stream.LookAhead(-2).Location));
                    }
                    else
                    {
                        throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba {");
                    }
                }   
                else if (Stream.Match(TokenValue.Card))
                { 
                    if(Stream.Match(TokenValue.OpenCurlyBracket))
                    {
                        program.Cards.Add(ParseCard(Stream.LookAhead(-2).Location));
                    }
                    else
                    {
                        throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba {");
                    }
                }    
                else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion invalida, se esperaba effect o card");
            }
            catch(CompilingError error)
            {
                if (PanicMode(error)) break;
            }
        }
        return program;
    }
    #endregion

    #region Card
    //Parseando la carta
    public CardComp ParseCard(CodeLocation location)
    {
        Expression Name = null;
        Expression Power = null;
        Expression Faction = null;
        Expression Type = null;
        List<Expression> Range = null;
        List<AssignEffect> onActivation = new List<AssignEffect>();
        CodeLocation ActvLocation = location;
        do
        {
            try
            {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");
            else if(Stream.Match(TokenValue.name)) Name = AssignProperty(Name is null, "Name");
            else if(Stream.Match(TokenValue.type)) Type = AssignProperty(Type is null, "Type");
            else if(Stream.Match(TokenValue.faction)) Faction = AssignProperty(Faction is null, "Faction");
            else if(Stream.Match(TokenValue.power)) Power = AssignProperty(Power is null, "Power");
            else if(Stream.Match(TokenValue.range)) Range = AssignRange(Range is null); // revisar esto

            //OnActivation
            else if(Stream.Match((TokenValue.onActivation))) 
            {
                ActvLocation = Stream.Previous().Location;
                Stream.Consume(TokenValue.colon, "Se esperaba :");
                Stream.Consume(TokenValue.OpenSquareBracket, "Se esperaba [");
                while(Stream.Match((TokenValue.OpenCurlyBracket)))
                {
                    onActivation.Add(assignEffect(Stream.Previous().Location));
                    if(!Stream.Comma(TokenValue.ClosedSquareBracket)) 
                        throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Se espera una , entre la declaracion de cada efecto");
                }
                Stream.Consume(TokenValue.ClosedSquareBracket, "Se esperaba ]");
            }
            else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion de carta invalida");
            }catch(CompilingError error)
            {
                if (PanicMode(error, TokenValue.comma)) break;
            }
        }while(!Stream.Match(TokenValue.ClosedCurlyBracket));
        

        if(Name is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el nombre de la carta");
        if(Type is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el tipo de la carta");
        if(Faction is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro la faccion de la carta");
        if(Power is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el poder de la carta");
        if(Range is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el range de la carta");
        //Revisar para no asignar efectos dejarlo vacio si se puede
        if(onActivation.Count == 0 ) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se le asignaron efectos a la carta");
        
        return new CardComp(Name, Type, Faction, Power, Range, onActivation, location);
    }
    private AssignEffect assignEffect(CodeLocation location)
    {
        Expression Name = null;
        Selector Select = null;
        List<(string,Expression)> Params = new List<(string, Expression)>();
        AssignEffect PostAction = null;
        do
        {
            try
            {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");
            else if(Stream.Match(TokenValue.assignEffect))
            {
                Stream.Consume(TokenValue.colon, "Se esperaba : despues de Effect");
                if(Stream.Match(TokenValue.OpenCurlyBracket))
                {
                    (Name, Params) = UpdateNameAndParams();
                }
                else Name = expression();
            }
            else if(Stream.Match(TokenValue.selector))
            {
                CodeLocation loc = Stream.Previous().Location;
                Stream.Consume(TokenValue.colon, "Se esperaba : despues de Selector");
                Stream.Consume(TokenValue.OpenCurlyBracket, "Se esperaba {");
                Select = SELECTOR(loc);
            }
            //revisar
            else if (Stream.Match(TokenValue.postAction)) PostAction = assignEffect(Stream.Previous().Location);

            else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Asignacion de efecto invalida");

            if(!Stream.Comma())
            {
                throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba ,");
            }

            }catch(CompilingError error)
            {
                if (PanicMode(error, TokenValue.comma)) break;
            }
        }while(!Stream.Match(TokenValue.ClosedCurlyBracket));

        if(Name is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el nombre del efecto");
        //if(Select is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el campo Selector de la carta");

        return new AssignEffect(Name, Params, Select, location);
    }
    private (Expression, List<(string ,Expression)>) UpdateNameAndParams()
    {
        Expression Name = null;
        List<(string,Expression)> Params = new List<(string, Expression)>();
        do
        {
            try
            {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");
            else if(Stream.Match(TokenValue.name)) Name = AssignProperty(Name is null, "Name");
            else if(Stream.Match(TokenType.Identifier)) Params.Add((Stream.Previous().Value, AssignProperty(true, Stream.Previous().Value)));
            else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Asignacion de efecto invalida");
            }catch(CompilingError error)
            {
                if (PanicMode(error, TokenValue.comma)) break;
            }
        }while(!Stream.Match(TokenValue.ClosedCurlyBracket));
        return (Name, Params);
    }
    private Selector SELECTOR(CodeLocation loc)
    {
        Expression source = null;
        Expression single = null;
        Expression predicate = null;
        do
        {
            try
            {
                if (Stream.Match(TokenType.End)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");

                    else if (Stream.Match(TokenValue.Source)) source = AssignProperty(source is null, "Source");

                    else if (Stream.Match(TokenValue.single)) single = AssignProperty(single is null, "Single");

                    else if (Stream.Match(TokenValue.predicate)) predicate = AssignProperty(predicate is null, "Predicate");
                
                    else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion invalida, se espera Source, Single o Predicate");

                }
                catch (CompilingError error)
                {
                    if (PanicMode(error, TokenValue.comma)) break;
                }
            } while (!Stream.Match(TokenValue.ClosedCurlyBracket));

            if (source is null) throw  new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Debe asignar el campo Source");
            if (predicate is null) throw  new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Debe asignar  el campo Predicate");

            return new Selector(source, single, predicate, loc);
    }
    private List<Expression> AssignRange(bool IsNotDefinided)
    {
        List<Expression> range = new List<Expression>();
        if(!IsNotDefinided)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.None, "Ya se definio la propiedad Range de la carta");
        }
        Stream.Consume(TokenValue.colon, "Se esperaba :");
        Stream.Consume(TokenValue.OpenSquareBracket, "Se esperaba [");
        do
        {
            Expression exp = expression();
            range.Add(exp);
            
            if(!Stream.Comma(TokenValue.ClosedSquareBracket))
            {
                throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba ,");
            } 
        }while(!Stream.Match(TokenValue.ClosedSquareBracket));
        
        if(range.Count == 0 || range.Count > 3)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion invalida de las posibles zonas de juego de la carta");
        }
        if(!Stream.Comma())
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
        }
        else return range;
    }
    private Expression AssignProperty(bool IsNotDefinided, string name)
    {
        if(!IsNotDefinided)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.None, "Ya se definio la propiedad " + name);
        }
        Expression exp;
        if(Stream.Match(TokenValue.colon))
        {
            exp = expression();
        }
        else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba : para definir la propiedad " + name + " de la carta");
        if(!Stream.Comma())
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
        }
        return exp;
    }
    #endregion

    #region Effects
    //Parseando los efectos
    public Effect ParseEffect(CodeLocation loc)
    {
        Expression Name = null;
        Token targets = null;
        Token context = null;
        Stmt body = null;
        List<(Token, Token)> Params = new List<(Token, Token)>();
        do
        {
            try
            {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");

            //Asignar nombre
            else if(Stream.Match(TokenValue.name)) Name = AssignProperty(Name is null, "Name"); 

            //Asignar Params
            else if(Stream.Match(TokenValue.Params))
            {
                if(Params.Count > 0) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Ya declaro el campo Params del efecto");
                
                Stream.Consume(TokenValue.colon, "Se esperaba : ");
                Stream.Consume(TokenValue.OpenCurlyBracket, "Se esperaba { ");
                do
                {
                    Params.Add(AddParam());
                    if(!Stream.Comma()) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Se espera una , entre la declaracion de cada parametro");

                }while(Stream.Peek().Type == TokenType.Identifier);
                Stream.Consume(TokenValue.ClosedCurlyBracket, "Se esperaba } ");

                if(!Stream.Comma())
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
                }
            }

            //Asignar Action
            else if(Stream.Match(TokenValue.action))
            {
                if(!(body is null)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Ya declaro el campo Action del efecto");
                Stream.Consume(TokenValue.colon, "Se esperaba : ");
                Stream.Consume(TokenValue.OpenBracket, "Se esperaba (");
                if (Stream.Match(TokenType.Identifier))
                {
                    targets = Stream.Previous();
                }
                else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba un identificador");
                Stream.Consume(TokenValue.comma, "Se esperaba ,");
                if (Stream.Match(TokenType.Identifier))
                {
                    context = Stream.Previous();
                }
                else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba un identificador");
                Stream.Consume(TokenValue.ClosedBracket, "Se esperaba )");
                Stream.Consume(TokenValue.lambda, "Se esperaba =>");
                if(Stream.Match(TokenValue.OpenCurlyBracket)) 
                {
                    body = ActionBody();
                }
                else body = SingleStmt();

                if(body is null) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Declaracion invalida del Action del efecto");

                if(!Stream.Comma())
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
                }
            }
            else 
            {
                throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion de propiedad de efecto invalida, se esperaba Name, Params o Action");
            }
            }catch(CompilingError error)
            {
                if (PanicMode(error, TokenValue.comma)) break;
            }
        } while(!Stream.Match(TokenValue.ClosedCurlyBracket));

        if (Name is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el nombre del efecto");
        if (body is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el Action del efecto");

        return new Effect(Name, targets, context, body, Params, loc);
    }
    private (Token,Token) AddParam()
    {
        Token id = null;
        Token type = null;
        if(Stream.Match(TokenType.Identifier))
        {
            id = Stream.Previous();
        }
        else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba un identificador como nombre del parametro");
        Stream.Consume(TokenValue.colon, "Se esperaba : ");
        if(Stream.Match(TokenType.Identifier))
        {
            type = Stream.Previous();
        }
        else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba un identificador como tipo del parametro");
        return (id,type);
    }
    //Sentencias
    public Stmt ActionBody()
    {
        List<Stmt> statements = new List<Stmt>();
        do
        {
            try
            {
                if (Stream.Peek().Type == TokenType.End)  throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba }");
                //else if (Stream.Match(TokenValue.OpenCurlyBracket)) return new Block(Block());
                else if (Stream.Match(TokenValue.WhileCicle)) statements.Add(While());
                else if (Stream.Match(TokenValue.ForCicle)) statements.Add(For());
                else statements.Add(SingleStmt());
            }
            catch (CompilingError error)
            {
                if (PanicMode(error, TokenValue.semicolon)) break;
            }
        } while (!Stream.Match(TokenValue.ClosedCurlyBracket));

        return new Block(statements, Stream.Peek().Location);
    }
    private Stmt For()
    {
        CodeLocation loc = Stream.Previous().Location;
        string id = null;
        if(Stream.Match(TokenType.Identifier))
        {
            id = Stream.Previous().Value;
        }
        else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se espera un identificador despues de for");
        Stream.Consume(TokenValue.In, "Se esperaba la palabra reservada in");
        string collection = null;
        if(Stream.Match(TokenType.Identifier))
        {
            collection = Stream.Previous().Value;
        }
        else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se espera un identificador despues de in");
        Stmt body = null;
        if(Stream.Match(TokenValue.OpenCurlyBracket))
        {
            body = ActionBody();
            Stream.Consume(TokenValue.semicolon, "Se espera ; desp de una expresion o un cuerpo de expresiones");
        }
        else body = SingleStmt();
        
        return new For(id, collection, body, loc);
    }
    private Stmt While()
    {
        CodeLocation loc = Stream.Previous().Location;
        Stream.Consume(TokenValue.OpenBracket, "Expect '(' after 'while'.");
        Expression condition = expression();
        Stream.Consume(TokenValue.ClosedBracket, "Expect ')' after condition.");
        Stmt body = null;
        if(Stream.Match(TokenValue.OpenCurlyBracket))
        {
            body = ActionBody();
            Stream.Consume(TokenValue.semicolon, "Se espera ; desp de una expresion o un cuerpo de expresiones");
        }
        else body = SingleStmt();
        return new While(condition, body, loc);
    }
    private Stmt SingleStmt()
    {
        Stmt stmt = null;
        if (Stream.Match(TokenValue.Print))
        {
            stmt = new Print(expression(), Stream.Previous().Location);
        }
        else if (Stream.Match(TokenType.Identifier))
        {
            Stream.MoveNext(-1);
            CodeLocation loc = Stream.Peek().Location;
            Expression exp = expression();
            stmt = new ExpressionStmt(exp, Stream.Previous().Location);
            if(Stream.Match(TokenValue.Assign, TokenValue.Increase, TokenValue.Decrease))
            {
                stmt = VarDeclaration(exp, loc);
            }
        }
        else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Statement vacio o expresion invalida");
        if (!Stream.Match(TokenValue.semicolon))
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion incompleta, se esperaba ;");
        }
        return stmt;
    }
    public Stmt VarDeclaration(Expression exp, CodeLocation loc) 
    {
        Expression Id = exp;
        Token op = Stream.Previous();
        Expression initializer = expression();
        return new Var(Id ,initializer,op, loc);
    }
    #endregion

    //Parseando expresiones
    #region Expressions
    public Expression expression()
    {
        return Or();
    }
    private Expression Or()
    {
        Expression expr = And();
        while (Stream.Match(TokenValue.or)) 
        {
        Expression right = And();
        expr = new Or(Stream.Peek().Location, expr, right);
        }
        return expr;
    }
    private Expression And()
    {
        Expression expr = Equality();
        while (Stream.Match(TokenValue.and)) 
        {
        Expression right = Equality();
        expr = new And(Stream.Peek().Location, expr, right);
        }
        return expr;
    }
    private Expression Equality()
    {
        Expression exp = Comparation();
        while(!Stream.End && Stream.Match(TokenValue.equal)) 
        {
            Token operador = Stream.Previous();
            Expression right = Comparation();
            exp = new Equal(Stream.Previous().Location, exp, right);
        }
        return exp;
    }
    private Expression Comparation() 
    {
        Expression expr = Term();
        while (!Stream.End && Stream.Match(TokenValue.elderly, TokenValue.minor, TokenValue.lessOrEqual, TokenValue.greaterOrEqual)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Term();
            if(operatorToken.Value == TokenValue.elderly)
                {
                    expr = new Elderly(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.minor)
                {
                    expr = new Minor(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.lessOrEqual)
                {
                    expr = new LessOrEqual(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.greaterOrEqual)
                {
                    expr = new GreaterOrEqual(Stream.Previous().Location, expr, right);
                }
        }
        return expr;
    }
    private Expression Term()
    {
        Expression expr = Factor();
        while (!Stream.End && Stream.Match(TokenValue.Add, TokenValue.Sub, TokenValue.ConcantenationWithoutSpace, TokenValue.ConcatenationWithEspace)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Factor();
            if(operatorToken.Value == TokenValue.Add)
            {
                expr = new Add(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.ConcantenationWithoutSpace)
            {
                expr = new ConcatWithoutSpace(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.ConcatenationWithEspace)
            {
                expr = new Concat(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.Sub)
            {
                expr = new Sub(Stream.Previous().Location, expr, right);
            }
        }
        return expr;
    }
    private Expression Factor() 
    {
        Expression expr = Power();
        while (!Stream.End && Stream.Match(TokenValue.Mul, TokenValue.Div)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Power();
            if(operatorToken.Value == TokenValue.Mul)
                {
                    expr = new Mult(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.Div)
                {
                    expr = new Div(Stream.Previous().Location, expr, right);
                }
        }
        return expr;
    }
    private Expression Power() 
    {
        Expression expr = Unary();
        while (!Stream.End && Stream.Match(TokenValue.pow)) 
        {
            Expression right = Unary();
            expr = new Pow(Stream.Previous().Location, expr, right);
        }
        return expr;
    }
    private Expression Unary() 
    {
       if (!Stream.End && Stream.Match(TokenValue.Sub))
        {
            Expression right = Unary();
            return new Number(-double.Parse(Stream.Previous().Value), Stream.Previous().Location);
        }
        return Primary();
    }
    private Expression Primary()
    {
        if (Stream.Match(TokenValue.False)) 
            return new Bool(false, Stream.Previous().Location);
        
        if (Stream.Match(TokenValue.True)) 
            return new Bool(true, Stream.Previous().Location);
        
        if (Stream.Match(TokenValue.OpenBracket)) 
        {
            Expression expr = expression();
            Stream.Consume(TokenValue.ClosedBracket, "Se esperaba ')' después de la expresión."); 
            if(Stream.Match(TokenValue.lambda))
            {
                if(expr is Variable)
                {
                    expr = Lambda(Stream.LookAhead(-3));
                    return expr;
                }
                else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "La expresion lambda debe recibir como parametro una variable");
            }
            return new Grouping(expr, Stream.Previous().Location);
        }
        // Chequeo un identificador 
        if (Stream.Match(TokenType.Identifier)) 
        {
            //Expression exp;
            Token variable = Stream.Previous();
            CodeLocation VarLoc = Stream.Previous().Location;
            Expression exp = new Variable(variable.Value, Stream.Previous().Location);

            // Procesar indexados, propiedades y métodos
            exp = ProcessMemberAccess(exp, VarLoc);

            //Procesar q se le hizo ++ o --
            if(Stream.Match(TokenValue.addOne, TokenValue.substractOne))
            {
                if(exp is Variable)
                {
                    exp = new ModificOne(exp.ToString(), Stream.Previous(), VarLoc);
                }  
                else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Solo se puede aplicar ++ y -- a variables");
            }
            
            return exp;
        }
        if (Stream.Match(TokenType.Number)) 
            return new Number(double.Parse(Stream.Previous().Value), Stream.Previous().Location);
        
        if (Stream.Match(TokenType.Text)) 
            return new Text(Stream.Previous().Value, Stream.Previous().Location);
        
        throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba una expresion");
    }
    private Expression Lambda(Token var)
    {
        Expression Condition = expression();
        return new Lambda(var, Condition, var.Location);
    }

    private Expression ProcessMemberAccess(Expression exp, CodeLocation varLoc)
    {
        while (true)
        {
            if (Stream.Match(TokenValue.OpenSquareBracket))
            {
                if (Stream.Match(TokenType.Number))
                {
                    exp = new Indexer(exp, double.Parse(Stream.Previous().Value), varLoc);
                }
                else
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba un número como índice");
                }
                Stream.Consume(TokenValue.ClosedSquareBracket, "Se esperaba ]");
            }
            else if (Stream.Match(TokenValue.dot))
            {
                Token caller = null;
                if (Stream.Match(TokenType.KeyWord))
                {
                    caller = Stream.Previous();
                }
                else
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba una propiedad o método");
                }

                if (Stream.Match(TokenValue.OpenBracket))
                {
                    if (!Stream.Match(TokenValue.ClosedBracket))
                    {
                        Expression param = expression(); 
                        Stream.Consume(TokenValue.ClosedBracket, "Se esperaba )");
                        exp = new MethodWithParams(exp, caller.Value, param, varLoc);
                    }
                    else
                    {
                        exp = new Method(exp, caller.Value, varLoc);
                    }
                }
                else
                {
                    exp = new Property(exp, caller.Value, varLoc);
                }
            }
            else
            {
                break;
            }
        }
        return exp; 
    }
    #endregion
}