// <copyright file="INavigation.cs" company="Kopigi">
// Copyright © Kopigi 2015
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>08/05/2105</date>
// <project>Navegar.UWP.Win10</project>
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

using System;

#region using

using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using EventHandler = Navegar.UWP.Win10.EventHandler;

#endregion

namespace Navegar.UWP.Win10
{ 
    /// <summary>
    /// Interface de la classe de navigation
    /// </summary>
    public interface INavigation : IDisposable
    {
        /// <summary>
        /// Permet d'exécuter une action avant la navigation
        /// </summary>
        PreNavigateDelegate<ViewModelBase> PreviewNavigate { get; set; }

        /// <summary>
        /// Permet d'indiquer que la navigation est annulée
        /// </summary>
        event EventHandler NavigationCanceledOnPreviewNavigate;

        /// <summary>
        /// Evenement de navigation arriére avec le bouton physique
        /// Permet de définir soit même une fonction gérant ce retour sans utiliser celui par défaut de Navegar
        /// </summary>
        System.EventHandler<Windows.Phone.UI.Input.BackPressedEventArgs> BackButtonPressed { get; set; }

        /// <summary>
        /// Evenement de navigation arriére avec le bouton virtuel
        /// Permet de définir soit même une fonction gérant ce retour sans utiliser celui par défaut de Navegar
        /// </summary>
        System.EventHandler<Windows.UI.Core.BackRequestedEventArgs> BackVirtualButtonPressed { get; set; }

        /// <summary>
        /// Déterminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        bool CanGoBack();

        /// <summary>
        /// Déclenche l'événement d'annulation de navigation
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
        /// Récupére l'instance du ViewModel
        /// </summary>
        /// <typeparam name="T">
        /// Type du ViewModel
        /// </typeparam>
        /// <returns>
        /// Instance du ViewModel
        /// </returns>
        T GetViewModelInstance<T>() where T : ViewModelBase;

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        void GoBack();

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        /// <param name="functionToLoad">
        /// Permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </param>
        /// <param name="parametersFunction">
        /// Paramétres pour la fonction appelée
        /// </param>
        void GoBack(string functionToLoad, params object[] parametersFunction);

        /// <summary>
        /// Indique si le device a un bouton de retour physique
        /// </summary>
        /// <returns>True si un bouton est présent, sinon false</returns>
        bool HasBackButton { get; }

        /// <summary>
        /// Indique si le device a un bouton de retour virtuel affiché
        /// </summary>
        /// <returns>True si un bouton est présent, sinon false</returns>
        bool HasVirtualBackButtonShow { get; }

        /// <summary>
        /// Permet de référencer la Frame Principale généré au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <param name="rootFrame">Frame de navigation principale</param>
        void InitializeRootFrame(Frame rootFrame);
        
        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel en demandant une nouvelle instance du ViewModel ciblé
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class;

        /// <summary>
        /// Naviguer vers un ViewModel sans historiser le ViewModel actuel
        /// Le paramètre <param name="functionToLoad"></param> permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </param>
        /// <param name="parametersFunction">
        /// Paramétres pour la fonction appelée
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class;

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique précédent
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectuée
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false) where TTo : class;

        /// <summary>
        /// Navigeur vers un ViewModel, avec un ViewModel en historique précédent. 
        /// Le paramètre <param name="functionToLoad"></param> permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="currentInstance">
        /// Viewmodel depuis lequel la navigation est effectuée
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </param>
        /// <param name="parametersFunction">
        /// Paramétres pour la fonction appelée
        /// </param>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
            where TTo : class;

        /// <summary>
        /// Permet d'associer un type pour la vue à un type pour le modéle de vue
        /// </summary>
        void RegisterView<TViewModel, TView>() 
            where TViewModel : ViewModelBase 
            where TView : Page;

        /// <summary>
        /// Perrmet d'afficher le bouton virtuel dans la barre de titre de l'application
        /// </summary>
        /// <param name="visible">indique si l'on doit rendre le bouton visible ou non</param>
        /// <param name="force">permet de forcer l'affichage même si le device posséde un bouton physique</param>
        /// <remarks>
        /// Si le device utilisé posséde un bouton physique cette fonction n'affiche pas de bouton, sauf à forcer l'affichage avec le paramétre
        /// </remarks>
        void ShowVirtualBackButton(bool visible = true, bool force=false);
    }
}
