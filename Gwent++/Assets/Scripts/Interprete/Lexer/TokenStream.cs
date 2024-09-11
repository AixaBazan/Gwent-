using System.Collections;
using System;
using System.Collections.Generic;
// This stream has functions to operate over a list of tokens.
public class TokenStream : IEnumerable<Token>
{
    private List<Token> tokens;
    private int position;
    public int Position { get { return position; } }
    public TokenStream(IEnumerable<Token> tokens)
    {
        this.tokens = new List<Token>(tokens);
        position = 0;
    }
    //veo si algun token tiene el valor que le estoy indicando
    public bool Match(params string[] values)
    {
        foreach(var token in values)
        {
            if(Check(token))
            {
                Advance();
                return true;
            }
        }
        return false; 
    }
    //veo si el token tiene el tipo que le digo
    public bool Match(TokenType type)
    {
        if(CheckType(type))
        {
            Advance();
            return true;
        }
        return false; 
    }
    //veo si el token tiene el valor que le muestro
    public bool Check(string value)
    {
        return Peek().Value == value;
    }
    //Veo el tipo del token
    public bool CheckType(TokenType type)
    {
        return Peek().Type == type;
    }
    public Token Advance()
    {
        if (!End) position++;
        return Previous();
    }
    public Token Peek()
    {
        return tokens[position];
    }
    public Token Previous()
    {
        return tokens[position - 1];
    }
    public bool End => Peek().Type == TokenType.End;
    public Token Consume(string value, string message) 
    {
        if (Check(value)) return Advance();
        throw new CompilingError(Peek().Location, ErrorCode.Expected, message);
    }
    public bool Comma(string end = TokenValue.ClosedCurlyBracket)
    {
        return Match(TokenValue.comma) || Peek().Value == end;
    }
    
    public void MoveNext(int k)
    {
        position += k;
    }

    public void MoveBack(int k)
    {
        position -= k;
    }

    public bool Next()
    {
        if (position < tokens.Count - 1)
        {
            position++;
        }

        return position < tokens.Count;
    }

    public bool Next( TokenType type )
    {
        if (position < tokens.Count-1 && LookAhead(1).Type == type)
        {
            position++;
            return true;
        }

        return false;
    }

    public bool Next(string value)
    {            
        if (position < tokens.Count-1 && LookAhead(1).Value == value)
        {
            position++;
            return true;
        }

        return false;
    }

    public bool CanLookAhead(int k = 0)
    {
        return tokens.Count - position > k;
    }

    public Token LookAhead(int k = 0)
    {
        return tokens[position + k];
    }

    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Count; i++)
            yield return tokens[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
