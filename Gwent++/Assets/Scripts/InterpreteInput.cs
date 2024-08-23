using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterpreteInput : MonoBehaviour
{
    public TMP_InputField inputField; // Referencia al TMP_InputField
    public TextMeshProUGUI messageText;
    public void HandleButtonClick() 
    {
        messageText.text = "";
        // Obtener el texto ingresado por el usuario
        string Input = inputField.text;
        
        if (string.IsNullOrEmpty(Input)) 
        {
            messageText.text = "Por favor, ingresa un texto."; // Mensaje si el campo está vacío
        } 
        else 
        {
            LexicalAnalyzer lex = Compiling.Lexical;
            List<CompilingError> errors = new List<CompilingError>();
            IEnumerable<Token> tokens = lex.GetTokens(Input, errors);
            if(errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    messageText.text += error + "\n";

                }
                messageText.text += "Debe solucionar los errores del lexer para pasar a la etapa de parseo";
            }
            else
            {
                TokenStream read = new TokenStream(tokens);
                Parser parser = new Parser(read);
                GwentProgram program = parser.ParseProgram();
                if(parser.errors.Count > 0)
                {
                    foreach (var error in parser.errors)
                    {
                        messageText.text += error + "\n";

                    }
                    messageText.text += "Debe solucionar los errores del parser para pasar a la etapa del chequeo semantico";
                }
                else
                {
                    Context context = new Context();
                    Scope scope = new Scope();
                    
                    program.CheckSemantic(context, scope, errors);

                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            messageText.text += error + "\n";

                        }
                        messageText.text += "Hubo errores semanticos, no se pudo continuar la evaluacion";
                    }
                    else
                    {
                        System.Console.WriteLine("Todo bien semanticamente");
                    }
                }
            }
        }
    }
}
