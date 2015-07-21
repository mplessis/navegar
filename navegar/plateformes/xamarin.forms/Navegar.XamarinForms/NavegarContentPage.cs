﻿#region licence
// <copyright file="NavegarContentPage.cs" company="Kopigi">
// Copyright © Kopigi 2015
// </copyright>
// ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>21/07/2015</date>
// <project>Navegar.XamarinForms</project>
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
using Xamarin.Forms;

#endregion

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
