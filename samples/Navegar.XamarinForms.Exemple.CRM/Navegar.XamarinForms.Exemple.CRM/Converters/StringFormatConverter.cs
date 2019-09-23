using System;
using System.Globalization;
using Xamarin.Forms;

namespace Navegar.XamarinForms.Exemple.CRM.Converters
{
    /// <summary>
    /// Utilise la fonction String.Format pour retourner le résultat demandé
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (parameter == null)
            {
                return value;
            }
            return String.Format((String)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
