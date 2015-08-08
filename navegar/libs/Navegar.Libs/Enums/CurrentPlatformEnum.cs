using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegar.Libs.Enums
{
    public enum CurrentPlatformEnum
    {
        /// <summary>
        /// Plateforme .NET pour WPF
        /// </summary>
        WPF = 0,

        /// <summary>
        /// Plateforme UAP pour Windows et Windows Phone 8.1
        /// </summary>
        UAP = 1,

        /// <summary>
        /// Plateforme UWP pour Windows 10
        /// </summary>
        UWP = 2,

        /// <summary>
        /// Plateforme pour Xamarin Forms
        /// </summary>
        XAMARINFORMS = 3
    }
}
