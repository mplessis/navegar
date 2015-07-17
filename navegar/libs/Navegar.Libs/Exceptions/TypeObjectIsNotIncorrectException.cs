using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegar.Libs.Exceptions
{
    /// <summary>
    /// Exception  permettant de capturer les erreurs dues à un type d'objet incorrect
    /// </summary>
    public class TypeObjectIsNotIncorrectException : Exception
    {
        /// <summary>
        /// Constructeur de l'exception
        /// </summary>
        /// <param name="e">
        /// L'exception déclenchée lors de la navigation
        /// </param>
        /// <param name="expectedType">Nom du type attendu</param>
        public TypeObjectIsNotIncorrectException(string expectedType)
            : base(string.Format("Type of object is incorrect, {0} is expected", expectedType), null)
        { }
    }
}
