using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hulk_Interpreter
{
    public class Function
    {
        public string Name {get; set;}
        public List<Token> Parametros ;
        public List<Token> CuerpoDelafuncion;
        public Function ( string name , List <Token> parametros, List<Token> cuerpoDeLaFuncion){
            Name= name;
            Parametros = parametros;
            CuerpoDelafuncion= cuerpoDeLaFuncion;
            
        }

        public void Show (){
            System.Console.WriteLine(Name);
            System.Console.WriteLine("Los parametros de la funcion son :");
            foreach (var item in Parametros)
            {   
                System.Console.WriteLine(item.Type + "---" + item.Value);
                
            }

            System.Console.WriteLine("El cuerpo de la funcion tiene la forma:");
            foreach (var item in CuerpoDelafuncion)
            {
                System.Console.WriteLine(item.Type + "---" + item.Value);
            }
        }
    }
}