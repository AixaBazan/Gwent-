/* The scope is used to check if the elements are alredy in use inside
  every Card or Element declaration. We must create a Scope Child in every case */
//  public class Scope
//     {
//         public Scope? Parent;

//         public List<string> effects;

//         public Scope()
//         {
//             effects = new List<string>();   
//         }

//         public Scope CreateChild()
//         {
//             Scope child = new Scope();
//             child.Parent = this;
               
//             return child;
//         }

//     }