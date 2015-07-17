using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegar.Libs.Enums
{
    /// <summary>
    /// Enumération pour le type de bouton de retour disponible sur le device
    /// </summary>
    public enum BackButtonTypeEnum
    {
        /// <summary>
        /// Aucun bouton disponible
        /// </summary>
        None = 0,

        /// <summary>
        /// Bouton physique
        /// </summary>
        Physical = 1,

        /// <summary>
        /// Bouton virtuel
        /// </summary>
        Virtual = 2
    }
}
