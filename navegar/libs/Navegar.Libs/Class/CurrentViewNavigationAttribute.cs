#region licence
// <copyright file="CurrentViewNavigationAttribute.cs" company="Kopigi">
// Copyright © Kopigi 2015
// </copyright>
// ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>21/07/2015</date>
// <project>Navegar.Libs</project>
// <web>http://navegar.kopigi.fr</web>
// <license>
// The MIT License (MIT)
// 
// Copyright (c) 2015 Marc Plessis
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </license>
#endregion
#region using

using System;
using System.Linq;
using System.Reflection;

#endregion

namespace Navegar.Libs.Class
{
    /// <summary>
    /// Attribut permettant de retrouver la propriétée servant d'affichage du nouveau ViewModel dans le ViewModel principal
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CurrentViewNavigationAttribute : Attribute
    {
        /// <summary>
        /// Retrieve informations on CurrentViewAttribute
        /// </summary>
        /// <returns>
        /// </returns>
        public static PropertyInfo GetCurrentViewProperty (Type t)
        {
            var entityProperties = t.GetRuntimeProperties();
            return (from prop in entityProperties
                   let attrs = prop.GetCustomAttributes(false)
                   where attrs.Any(obj => obj.GetType() == typeof(CurrentViewNavigationAttribute))
                   select prop).FirstOrDefault();
        }
    }
}
