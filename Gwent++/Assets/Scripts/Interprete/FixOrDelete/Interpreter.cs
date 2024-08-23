// class Interpreter : AST
// {
//     public List<CompilingError> Errors {get; set;}
//     public List<Card> Cards {get; set;}
//     //public Dictionary<string, effect> Elements {get; set;}
//     public Interpreter(CodeLocation location) : base (location)
//     {
//         Errors = new List<CompilingError>();
//         Cards = new List<Card>();
//         //Elements = new Dictionary<string, Element>();
//     } 
//     /* To check a program semantic we sould first collect all the existing elements and store them in the context.
//     Then, we check semantics of elements and cards */
//     public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
//     {
//         // bool checkElements = true;
//         // foreach (Element element in Elements.Values)
//         // {
//         //     checkElements = checkElements && element.CollectElements(context, scope.CreateChild(), errors);
//         // }
//         // foreach (Element element in Elements.Values)
//         // {
//         //     checkElements = checkElements && element.CheckSemantic(context, scope.CreateChild(), errors);
//         // }
//         bool checkCards = true;
//         foreach (Card card in Cards)
//         {
//             checkCards = checkCards && card.CheckSemantic(context, scope, errors);
//         }

//         return checkCards;
//     }

//     public void Evaluate()
//     {
//         foreach (Card card in Cards)
//         {
//             card.Interprete();
//         }
//     }

//     public override string ToString()
//     {
//         string s = "";
//         // foreach (Element element in Elements.Values)
//         // {
//         //     s = s + "\n" + element.ToString();
//         // }
//         foreach (Card card in Cards)
//         {
//             s += "\n" + card.ToString();
//         }
//         return s;
//     }
// }