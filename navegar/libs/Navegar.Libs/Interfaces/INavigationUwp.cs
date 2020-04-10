#region licence
// <copyright file="INavigation.cs" company="Kopigi">
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
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Navegar.Libs.Class;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;

#endregion

namespace Navegar.Libs.Interfaces
{
    /// <summary>
    /// D�l�gu� s'ex�cutant avant toute navigation avant
    /// </summary>
    /// <typeparam name="T">Type de la navigation</typeparam>
    /// <param name="currentViewModelInstance">Instance du ViewModel courant</param>
    /// <param name="currentViewModel">Type du ViewModel courant</param>
    /// <param name="viewModelToNavigate">Type du ViewModel vers lequel la navigation va</param>
    /// <param name="preNavigationArgs">Argument permettant de remplacer ou de sp�cifier une fonction et ses param�tres eventuels, � executer apr�s la navigation. Null en retour pour ne pas sp�cifier de fonction</param>
    /// <returns>true pour que la navigation continue, false pour bloquer la navigation ce qui va d�clencher l'�v�nement NavigationCanceledOnPreviewNavigate</returns>
    public delegate bool PreNavigateDelegate<T>(T currentViewModelInstance, Type currentViewModel, Type viewModelToNavigate, out PreNavigationArgs preNavigationArgs) where T : ViewModelBase;

    /// <summary>
    /// Interface de la classe de navigation
    /// </summary>
    public interface INavigationUwp : IDisposable
    {
        /// <summary>
        /// Permet d'ex�cuter une action avant la navigation
        /// </summary>
        PreNavigateDelegate<ViewModelBase> PreviewNavigate { get; set; }

        /// <summary>
        /// Permet d'indiquer que la navigation est annul�e
        /// </summary>
        event EventHandler NavigationCanceledOnPreviewNavigate;

        #region Surcharge de la navigation arri�re
        /// <summary>
        /// Permet de faire un override de OnBackButtonPressed pour la page associ�e au ViewModel.
        /// Attention il faut que page h�rite de NavegarContentPage pour que cela soit pris en compte.
        /// Si votre page h�rite bien de NavegarContentPage mais que vous ne d�finissez de fonction personnalis�e, celle par d�faut de Navegar sera appliqu�e
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel associ�</typeparam>
        /// <param name="func">Fonction personnalis�e pour le OnBackButtonPressed</param>
        /// <remarks>
        /// Votre fonction doit retourner false pour permetre de continuer la naigation arri�re, sinon elle sera stopp�e.
        /// Sp�cifique � la plateforme Xamarin.Forms
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// </remarks>
        void RegisterBackPressedAction<TViewModel>(Func<bool> func) where TViewModel : ViewModelBase;

        /// <summary>
        /// Evenement de navigation arri�re avec le bouton physique
        /// Permet de d�finir soit m�me une fonction g�rant ce retour sans utiliser celui par d�faut de Navegar
        /// </summary>
        Func<bool> BackButtonPressed { get; set; }

        #endregion

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
        /// Indique quelle plateforme est en cours d'ex�cution
        /// </summary>
        CurrentPlatformEnum CurrentPlatform { get; }

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
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
        /// </summary>
        /// <param name="functionsToLoad">
        /// Permet de d�finir un dictionnaire contenant les noms des fonctions � appeler apr�s le chargement du viewModel cibl� avec leurs param�tres �ventuels</param>
        void GoBack(Dictionary<string, object[]> functionsToLoad);

        /// <summary>
        /// Indique si le device a un bouton de retour physique
        /// </summary>
        /// <returns>True si un bouton est pr�sent, sinon false</returns>
        BackButtonTypeEnum HasBackButton { get; }

        /// <summary>
        /// Permet de r�f�rencer la Frame Principale g�n�r� au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <param name="rootFrame">Frame de navigation principale</param>
        /// <remarks>
        /// Sp�cifique aux plateformes .netcore
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// </remarks>
        void InitializeRootFrame(object rootFrame);

        /// <summary>
        /// Permet de r�f�rencer la page principale g�n�r�e au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <remarks>
        /// Sp�cifique � la plateforme Xamarin.Forms
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// </remarks>
        object InitializeRootFrame<TViewModelFirst, TViewFirst>() where TViewModelFirst : ViewModelBase;

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
        string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class;

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
        string NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class;

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
        string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
            where TTo : class;

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
        /// <remarks>
        /// Support�e uniquement sur la plateforme Xamarin.Forms, dans les autres cas une exception <see cref="NotImplementedForCurrentPlatformException"/> sera lev�e
        /// </remarks>
        string NavigateModalTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
            where TTo : class;

        /// <summary>
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue
        /// </summary>
        void RegisterView<TViewModel, TView>()
            where TViewModel : ViewModelBase where TView : class;

        /// <summary>
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue 
        /// en incluant si un bouton back virtuel doit etre activ� dans la barre de titre de l'application
        /// </summary>
        /// <param name="backVirtualButton">Indique si l'on doit ou non afficher un bouton de retour virtuel ou bien si l'affichage du bouton est g�r� par le d�veloppeur</param>
        /// <remarks>
        /// Sp�cifique � la plateforme .netcore UWP (Windows 10)
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// Si le device utilis� poss�de un bouton physique cette fonction n'affiche pas de bouton, sauf � forcer l'affichage avec le param�tre
        /// </remarks>
        void RegisterView<TViewModel, TView>(BackButtonViewEnum backVirtualButton)
            where TViewModel : ViewModelBase where TView : class;

        /// <summary>
        /// Perrmet d'afficher le bouton virtuel dans la barre de titre de l'application
        /// </summary>
        /// <param name="visible">indique si l'on doit rendre le bouton visible ou non</param>
        /// <param name="force">permet de forcer l'affichage m�me si le device poss�de un bouton physique</param>
        /// <remarks>
        /// Sp�cifique � la plateforme .netcore UWP (Windows 10)
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// Si le device utilis� poss�de un bouton physique cette fonction n'affiche pas de bouton, sauf � forcer l'affichage avec le param�tre
        /// </remarks>
        void ShowVirtualBackButton(bool visible = true, bool force=false);
    }
}
