using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegar.Libs.Enums
{
    /// <summary>
    /// Enumération décrivant le mode d'affichage du bouton virtuel dans la barre de titre d'une application UWP Windows 10
    /// </summary>
    public enum BackButtonViewEnum
    {
        /// <summary>
        /// Pas de bouton virtuel
        /// </summary>
        None = 0,

        /// <summary>
        /// Affichage géré automatiquement par Navegar. Si le mode d'UI de Windows est en mode Mouse le bouton est affiché, en mode Touch il est supprimé de la barre de titre
        /// </summary>
        Auto = 1,

        /// <summary>
        /// L'affichage du bouton est géré par le développeur, par exemple en utilisant la fonction ShowVirtualBackButton dans la fonction OnNavigatedTo de la Page
        /// </summary>
        Manual = 2
    }
}
