// <copyright file="FrameNavigationException.cs" company="Kopigi">
// Copyright © Kopigi 2015
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>07/07/2015</date>
// <project>Navegar.Xamarin.Forms</project>
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
//  </license>

#region using

using System;

#endregion
namespace Navegar.XamarinForms
{
    /// <summary>
    /// Exception  permettant de capturer les erreurs dues aux Frame, lors de la navigation
    /// </summary>
    public class FrameNavigationException : Exception
    {
        /// <summary>
        /// Constructeur de l'exception
        /// </summary>
        /// <param name="e">
        /// L'exception déclenchée lors de la navigation
        /// </param>
        public FrameNavigationException()
            : base("The Frame object isn't initialized in navigation service", null)
        {}
    }
}
