using System;
using Windows.UI.Xaml.Data;

namespace Navegar.UAP.Exemple.CRM.Converters
{
	/// <summary>
	/// Convert Boolean to Visibility.
	/// </summary>
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
            return (value is bool && (bool)value) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
            return value is Windows.UI.Xaml.Visibility && (Windows.UI.Xaml.Visibility)value == Windows.UI.Xaml.Visibility.Visible;
		}
	}
}
