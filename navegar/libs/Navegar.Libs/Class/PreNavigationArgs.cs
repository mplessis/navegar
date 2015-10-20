using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegar.Libs.Class
{
    /// <summary>
    /// Permet de spécifier un nom de fonction et des paramètres eventuels, qui seront ajoutés (ou qui remplace la fonction de départ si elle était spécifiée) à la navigation
    /// </summary>
    public class PreNavigationArgs
    {
        public string FunctionToLoad { get; set; }
        public object[] ParametersFunctionToLoad { get; set; }
    }
}
