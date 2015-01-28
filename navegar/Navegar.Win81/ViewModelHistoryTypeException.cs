// <copyright file="ViewModelHistoryTypeException.cs" company="Kopigi">
// Copyright © Kopigi 2013
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>26/10/2013</date>
// <project>Navegar.Win8</project>
// <web>http://www.kopigi.fr</web>
// <license>
// The MIT License (MIT)
// 
// Copyright (c) 2013 Marc Plessis
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
//  </license>

#region using

using System;

#endregion

namespace Navegar.UAP.Win81
{
    /// <summary>
    /// Exception  permettant de capturer les erreurs de chargement de fonction, lors de la navigation
    /// </summary>
    public class ViewModelHistoryTypeException : Exception
    {
        /// <summary>
        /// Constructeur de l'exception
        /// </summary>
        /// <param name="type">Type de l'objet passé en paramètre pour l'historique</param>
        public ViewModelHistoryTypeException(string type)
            : base(String.Format("{0} doesn't inherited ViewModelBase. This type is not allowed for history", type), null)
        { }
    }
}
