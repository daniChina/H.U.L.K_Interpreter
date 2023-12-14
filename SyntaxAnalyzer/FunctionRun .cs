using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace SyntaxAnalyzer
{
    public partial class SyntaxAnalyzer 
    {
        public bool IsFunctionAdd(){//Aqui voy a revisar si la funcion que se va a ejecutar existe en el Diccionario que guarda a las funciones declaradas 
            for ( int i =0 ; i< FuncionesDeclaradas.Count ; i++){
                if (FuncionesDeclaradas.ContainsKey(currentToken.Value.ToString()!)){
                   return true;
                };
            }
            return false ;

        }
        
    }
}