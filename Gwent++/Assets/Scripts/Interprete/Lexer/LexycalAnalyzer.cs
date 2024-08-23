using System;
using System.Collections.Generic;
using System.Linq;
class LexicalAnalyzer
{
    Dictionary<string, string> operators = new Dictionary<string, string>();
    Dictionary<string, string> keywords = new Dictionary<string, string>();
    Dictionary<string, string> texts = new Dictionary<string, string>();
    public IEnumerable<string> Keywords { get { return keywords.Keys; } }

    // Associates an operator symbol with the correspondent token value 
    public void RegisterOperator(string op, string tokenValue)
    {
        this.operators[op] = tokenValue;
    }
    // Associates a keyword with the correspondent token value 
    public void RegisterKeyword(string keyword, string tokenValue)
    {
        this.keywords[keyword] = tokenValue;
    }

    // Associates a Text literal starting delimiter with their correspondent ending delimiter 
    public void RegisterText(string start, string end)
    {
        this.texts[start] = end;
    } 
    // Matches a new symbol in the code and read it from the string. The new symbol is added to the token list as an operator
    private bool MatchSymbol(TokenReader stream, List<Token> tokens)
    {
        foreach (var op in operators.Keys.OrderByDescending(k => k.Length))
            if (stream.Match(op))
            {
                tokens.Add(new Token(TokenType.Operator, operators[op], stream.Location));
                return true;
            }
        return false;
    }
    // Matches a Text part in the code and read the literal from the stream.
    //The tokens list is updated with the new string token and errors is updated with new errors if detected. 
    private bool MatchText (TokenReader stream, List<Token> tokens, List<CompilingError> errors)
    {
        foreach (var start in texts.Keys.OrderByDescending(k=>k.Length))
        {
            string text;
            if (stream.Match(start))
            {
                if (!stream.ReadUntil(texts[start], out text))
                    errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, texts[start]));
                    tokens.Add(new Token(TokenType.Text, text, stream.Location));
                    return true;
            }
        }
        return false;
    }
    // Returns all tokens read from the code and populate the errors list with all lexical errors detected
    public IEnumerable<Token> GetTokens(string code, List<CompilingError> errors)
    {
        List<Token> tokens = new List<Token>();

        TokenReader stream = new TokenReader(code);

        while (!stream.EndOfFile)
        {
            string value;

            if (stream.ReadWhiteSpace())
                continue;

            if (stream.ReadID(out value))
            {
                if (keywords.ContainsKey(value))
                    tokens.Add(new Token(TokenType.KeyWord, keywords[value], stream.Location));
                else
                    tokens.Add(new Token(TokenType.Identifier, value, stream.Location));
                continue;
            }

            if (MatchSymbol(stream, tokens))
                continue;

            if(stream.ReadNumber(out value))
            {
                double d;
                if (!double.TryParse(value, out d))
                {
                    errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                }
                tokens.Add(new Token(TokenType.Number, value, stream.Location));
                continue;
            }
    
            if (MatchText(stream, tokens, errors))
                continue;

            var unkOp = stream.ReadAny();
            errors.Add(new CompilingError(stream.Location, ErrorCode.Unknown, unkOp.ToString()));
        }
        // Agregar el token de fin de documento
        tokens.Add(new Token(TokenType.End, "END", stream.Location));
        return tokens;
    }
}