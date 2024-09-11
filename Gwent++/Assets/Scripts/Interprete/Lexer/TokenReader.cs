using System;
using System.Collections.Generic;
class TokenReader
{
    string code;
    int pos;
    int line;
    public TokenReader(string code)
    {
        this.code = code;
        this.pos = 0;
        this.line = 1;
    }
    public CodeLocation Location
    {
        get
        {
            return new CodeLocation
            {
                Line = line,
            };
        }
    }
    /* Peek the next character */
    public char Peek()
    {
        if (pos < 0 || pos >= code.Length)
            throw new InvalidOperationException();

        return code[pos];
    }
    // Lee cualquier caracter a continuacion, siempre que la posicion siguiente sea valida
    public char ReadAny()
    {
        if (EndOfFile)
            throw new InvalidOperationException();

        if (EndOfLine)
        {
            line++;
        }
        return this.code[this.pos++];
    }
    public bool EndOfFile
    {
        get{ return pos >= code.Length;}
    }
    public bool EndOfLine
    {
        get{return EndOfFile || code[pos] == '\n';}
    }
    public bool ContinuesWith(string prefix)
    {
        if (pos + prefix.Length > code.Length)
            return false;
        for (int i = 0; i < prefix.Length; i++)
            if (code[pos + i] != prefix[i])
                return false;
        return true;
    }
    public bool Match(string prefix)
    {
        if (ContinuesWith(prefix))
        {
            pos += prefix.Length;
            return true;
        }
        return false;
    }
    public bool ValidIdCharacter(char c, bool begining)
    {
        return c == '_' || (begining ? char.IsLetter(c) : char.IsLetterOrDigit(c));
    }

    // El metodo ReadID  lee una cadena de caracteres siempre que estos sean validos para crear un identificador,
    // y devuelve el verdadero si la cadena obtenida no es vacia, ademas de devolver la cadena como valor de retorno
    public bool ReadID(out string id)
    {
        id = "";        
        while (!EndOfLine && ValidIdCharacter(Peek(), id.Length == 0))
        {
            id+=ReadAny();
        }
        return id.Length > 0;
    }
    public bool ReadNumber(out string number)
    {
        number = "";
        while(!EndOfLine && char.IsDigit(Peek()))
        {
            number += ReadAny();
        }
        if(!EndOfLine && Match("."))
        {
            number += ".";
            while (!EndOfLine && char.IsDigit(Peek()))
                number += ReadAny();
        }
        if(number.Length == 0)
        {
            return false;
        }
        return number.Length > 0;
    }
    // Este metodo lee hasta un string determinado. Solo devuelve falso si se alcanza un salto de linea o el fin del codigo
    public bool ReadUntil(string end, out string text)
    {
        text = "";
        while (!Match(end))
        {
            if (EndOfLine || EndOfFile)
                return false;
            text += ReadAny();
        }
        return true;
    }
    //Este metodo lee y omite el codigo mientras sea un espacio en blanco
    public bool ReadWhiteSpace()
    {
        if (char.IsWhiteSpace(Peek()))
        {
            ReadAny();
            return true;
         }
        return false;
    }
}