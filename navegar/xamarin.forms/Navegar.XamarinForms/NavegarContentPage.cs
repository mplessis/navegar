using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Navegar.XamarinForms
{
    /// <summary>
    /// Permet d'appliquer une fonction personnalisée, surchargeant OnBackButtonPressed de Xamarin.Forms,
    /// à l'aide de la fonction RegisterBackPressedAction de Navegar, dans votre ViewModel
    /// </summary>
    /// <example>
    /// Dans votre ViewModel définissez une fonction de signature : bool OnBack(); Le nom n'importe pas
    /// puis dans le constructeur de ce ViewModel faites : 
    /// RegisterBackPressedAction = OnBack;
    /// </example>
    public class NavegarContentPage : ContentPage
    {
        public Action OnBackPressed { get; set; }

        protected override bool OnBackButtonPressed()
        {
            this.OnBackPressed();
            return true;
        }
    }
}
