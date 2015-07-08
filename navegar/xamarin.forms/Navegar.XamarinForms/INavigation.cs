// <copyright file="INavigation.cs" company="Kopigi">
// Copyright � Kopigi 2015
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>07/07/2015</date>
// <project>Navegar.Xamarin.Forms</project>
// <web>http://navegar.kopigi.fr</web>
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

using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Xamarin.Forms;
using EventHandler = Navegar.XamarinForms.EventHandler;

namespace Navegar.XamarinForms
{    
    /// <summary>
    /// Interface de la classe de navigation
    /// </summary>
    public interface INavigation : IDisposable
    {
        /// <summary>
        /// Permet d'ex�cuter une action avant la navigation
        /// </summary>
        PreNavigateDelegate<ViewModelBase> PreviewNavigate { get; set; }

        /// <summary>
        /// Permet d'indiquer que la navigation est annul�e
        /// </summary>
        event EventHandler NavigationCanceledOnPreviewNavigate;

        /// <summary>
        /// D�terminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        bool CanGoBack();

        /// <summary>
        /// D�clenche l'�v�nement d'annulation de navigation
        /// </summary>
        void CancelNavigation();

        /// <summary>
        /// Permet de vider l'historique de navigation
        /// </summary>
        void Clear();

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
        T GetViewModelInstance<T>() where T : ViewModelBase;

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
        /// Permet de r�f�rencer la page principale g�n�r�e au lancement de l'application, pour la suite de la navigation
        /// </summary>
        Page InitializeRootFrame<TViewModelFirst, TViewFirst>() where TViewModelFirst : ViewModelBase
            where TViewFirst : ContentPage;

        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel
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
        Task<string> NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel en demandant une nouvelle instance du ViewModel cibl�
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
        Task<string> NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel
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
        Task<string> NavigateTo<TTo>(object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class;

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
        Task<string> NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false) where TTo : class;

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
        Task<string> NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
            where TTo : class;

        /// <summary>
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue
        /// </summary>
        void RegisterView<TViewModel, TView>() 
            where TViewModel : ViewModelBase 
            where TView : ContentPage;
    }
}
