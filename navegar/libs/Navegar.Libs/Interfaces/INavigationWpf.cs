#region licence
// <copyright file="INavigationWpf.cs" company="Kopigi">
// Copyright � Kopigi 2015
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
using GalaSoft.MvvmLight;
using Navegar.Libs.Enums;

#endregion

namespace Navegar.Libs.Interfaces
{
    /// <summary>
    /// Interface de la classe de navigation
    /// </summary>
    public interface INavigationWpf
    {
        /// <summary>
        /// Nettoie l'ensemble de l'historique de navigation
        /// </summary>
        void Clear();

        /// <summary>
        /// Indique quelle plateforme est en cours d'ex�cution
        /// </summary>
        CurrentPlatformEnum CurrentPlatform { get; }

        /// <summary>
        /// D�terminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        bool CanGoBack();

        /// <summary>
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
        /// </summary>
        void GoBack();

        /// <summary>
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
        /// </summary>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        void GoBack(string functionToLoad, params object[] parametersFunction);

        /// <summary>
        /// Permet de connaitre le type du ViewModel au niveau n-1 de l'historique de navigation
        /// </summary>
        /// <returns>Type du ViewModel</returns>
        Type GetTypeViewModelToBack();

        /// <summary>
        /// Permet de retrouver l'instance du ViewModel courant
        /// </summary>
        /// <returns>ViewModel courant</returns>
        ViewModelBase GetViewModelCurrent();

        /// <summary>
        /// R�cup�re l'instance du ViewModel
        /// </summary>
        /// <typeparam name="T">
        /// Type du ViewModel
        /// </typeparam>
        /// <returns>
        /// Instance du ViewModel
        /// </returns>
        ViewModelBase GetViewModelInstance<T>() where T : ViewModelBase;

        /// <summary>
        /// G�n�re l'instance du ViewModel principal
        /// </summary>
        /// <typeparam name="TMain">
        /// Type du ViewModel principal
        /// </typeparam>
        void GenerateMainViewModelInstance<TMain>() where TMain : class, new();

        /// <summary>
        /// R�cup�re l'instance du ViewModel principal
        /// </summary>
        /// <typeparam name="TMain">
        /// Type du ViewModel principal
        /// </typeparam>
        /// <returns>
        /// Instance du ViewModel principal
        /// </returns>
        TMain GetMainViewModelInstance<TMain>() where TMain : class;

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo(Type TTo, params object[] parametersViewModel);

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo(Type TTo, bool newInstance, params object[] parametersViewModel);

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel
        /// Le param�tre <param name="functionToLoad"></param> permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo(Type TTo, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false);

        /// <summary>
        /// Naviguer vers un ViewModel
        /// Le param�tre <param name="functionToLoad"></param> permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class;

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique pr�c�dent
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectu�e
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false);

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique pr�c�dent
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectu�e
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false) where TTo : class;

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique pr�c�dent. 
        /// Le param�tre <param name="functionToLoad"></param> permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectu�e
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false);

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique pr�c�dent. 
        /// Le param�tre <param name="functionToLoad"></param> permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectu�e
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
            where TTo : class;
    }
}
