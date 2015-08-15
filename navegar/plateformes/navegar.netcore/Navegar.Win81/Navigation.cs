// <copyright file="Navigation.cs" company="Kopigi">
// Copyright � Kopigi 2013
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>26/10/2013</date>
// <project>Navegar.Win8</project>
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

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Class;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;
using Navegar.Libs.Interfaces;

#endregion

namespace Navegar.Plateformes.NetCore.UAP.Win81
{ 

    /// <summary>
    /// Impl�mentation de la classe de navigation
    /// </summary>
    public class Navigation : NavigationBase
    {
        #region fields

        private Frame _rootFrame;
        private Func<bool> _backButtonPressedAction;

        #endregion

        #region Surcharge de la navigation arri�re

        /// <summary>
        /// Indique si le device a un bouton de retour physique
        /// </summary>
        /// <returns>True si un bouton est pr�sent, sinon false</returns>
        public override BackButtonTypeEnum HasBackButton
        {
            get
            {
#if WINDOWS_PHONE_APP
                return BackButtonTypeEnum.Physical;
#else
                return BackButtonTypeEnum.None;
#endif
            }
        }

        /// <summary>
        /// Evenement de navigation arri�re avec le bouton physique ou virtuel
        /// Permet de d�finir soit m�me une fonction g�rant ce retour sans utiliser celui par d�faut de Navegar
        /// </summary>
        /// <remarks>
        /// Si aucun bouton physique ou virtuel n'est pr�sent sur le device, la valeur est �gale � null
        /// </remarks>
        private Func<bool> _backButtonPressed;
        public override Func<bool> BackButtonPressed
        {
            get { return _backButtonPressed; }
            set
            {
#if WINDOWS_PHONE_APP
                if (HasBackButton == BackButtonTypeEnum.Physical)
                {
                    _backButtonPressedAction = value;
                    if (value != null)
                    {
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonBackPressed;
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressedOverride;
                    }
                    else
                    {
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonBackPressedOverride;
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressed;
                    }
                    _backButtonPressed = value;
                }
                else
                {
                    _backButtonPressed = null;
                }
            }
#else
                _backButtonPressed = null;
                _backButtonPressedAction = null;
            }
#endif
        }

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
        public override void RegisterBackPressedAction<TViewModel>(Action func)
        {
            throw new NotImplementedForCurrentPlatformException();
        }

        #endregion

        /// <summary>
        /// Permet de r�f�rencer la Frame Principale g�n�r� au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <param name="rootFrame">Frame de navigation principale</param>
        /// <remarks>
        /// Sp�cifique aux plateformes .netcore
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// </remarks>
        public override void InitializeRootFrame(object rootFrame)
        {
            if (!(rootFrame is Frame))
            {
                throw new TypeObjectIsNotIncorrectException("Frame");
            }

            NavigationStateInitial = ((Frame)rootFrame).GetNavigationState();
            _rootFrame = (Frame)rootFrame;
#if WINDOWS_PHONE_APP
            //Gestion du bouton de retour physique ou virtuel du device
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressed;
#endif
        }

        /// <summary>
        /// Permet de r�f�rencer la page principale g�n�r�e au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <remarks>
        /// Sp�cifique � la plateforme Xamarin.Forms
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// </remarks>
        public override object InitializeRootFrame<TViewModelFirst, TViewFirst>()
        {
            throw new NotImplementedForCurrentPlatformException();
        }

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
        public override string NavigateModalTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad,
            object[] parametersFunction, bool newInstance = false)
        {
            throw new NotImplementedForCurrentPlatformException();
        }

        /// <summary>
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue
        /// </summary>
        public override void RegisterView<TViewModel, TView>()
        {
            if (!ViewsRegister.ContainsKey(typeof(TViewModel)))
            {
                ViewsRegister.Add(typeof(TViewModel), typeof(TView));
            }
        }

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
        public override void ShowVirtualBackButton(bool visible = true, bool force = false)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            ClearNavigation();
            GC.Collect();
        }

        #region protected

        /// <summary>
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours
        /// </summary>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        protected override void Navigate(Type viewModelToName, string functionToLoad, object[] parametersFunction)
        {
            if (viewModelToName != null)
            {
                string key = String.Empty;

                if (FactoriesInstances.ContainsKey(viewModelToName))
                {
                    FactoriesInstances.TryGetValue(viewModelToName, out key);
                }

                if (!String.IsNullOrEmpty(key))
                {
                    var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, key);
                    if (instance != null)
                    {
                        var result = SetGoBack(viewModelToName);
                        if (result)
                        {
                            CurrentViewModel = viewModelToName;

                            //Gestion d'une fonction � appeler suite � la g�n�ration de l'instance
                            //La recherche de la m�thode doit se faire sur le type de l'instance
                            if (!string.IsNullOrEmpty(functionToLoad))
                            {
                                var method = instance.GetType().GetMethod(functionToLoad, parametersFunction);
                                if (method != null)
                                {
                                    method.Invoke(instance, parametersFunction);    
                                }
                                else
                                {
                                    var countParameters = parametersFunction != null
                                                  ? ((IEnumerable<object>)parametersFunction).Count()
                                                  : 0;
                                    throw new FunctionToLoadNavigationException(string.Format("{0} with {1} parameter(s)", functionToLoad, countParameters), instance.GetType().Name);
                                }
                            }
                        }
                        else
                        {
                            throw new HistoryNavigationException();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </param>
        /// <param name="viewModelFromName">
        /// Type du Viewmodel depuis lequel la navigation est effectu�e
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
        /// <param name="isModal">
        /// Indique que l'on souhaite une navigation modal, support�e uniquement par la plateforme Xamarin.Forms
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        protected override string Navigate<TTo>(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false, bool isModal = false)
        {
            try
            {
                //V�rification du type de ViewModel demand� pour l'historique
                if (!viewModelToName.GetTypeInfo().IsSubclassOf(typeof(ViewModelBase)))
                {
                    throw new ViewModelHistoryTypeException(viewModelToName.ToString());
                }

                //Pr�-navigation
                if (!PreNavigateTo(viewModelFromName, viewModelToName))
                {
                    OnNavigationCancel();
                    return string.Empty;
                }

                //Gestion de l'historique
                HistoryNavigateTo(viewModelFromName, viewModelToName, isModal);

                //G�n�ration d'une instance du viewmodel
                var key = GenerateNewInstanceViewModelNavigateTo<TTo>(viewModelToName, parametersViewModel, newInstance);

                //Lancement de l'instance et navigation
                var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, key);
                if (instance != null)
                {
                    Type instanceToNavigate;
                    var result = ViewsRegister.TryGetValue(instance.GetType(), out instanceToNavigate);
                    if(result)
                    {
                        CurrentViewModel = viewModelToName;
                        SetCurrentView(instanceToNavigate);
                    }
                }

                //Gestion d'une fonction � appeler suite � la g�n�ration de l'instance
                LoadFunctionViewModelNavigateTo<TTo>(instance, functionToLoad, parametersFunction);
                
                //Renvoi de la cl� de l'instance du ViewModel dans l'IOC
                return key;
            }
            catch (Exception e)
            {
                throw new NavigationException(e);
            }
        }

        /// <summary>
        /// Affecte la propri�t� de view courante du ViewModel principal
        /// </summary>
        /// <param name="instanceToNavigate">
        /// Instance de la vue qui est devenue la vue courante
        /// </param>
        private void SetCurrentView(Type instanceToNavigate)
        {
            try
            {
                _rootFrame.Navigate(instanceToNavigate);    
            }
            catch (Exception e)
            {
                throw new FrameNavigationException();
            }
        }

        /// <summary>
        /// Permet de g�rer l'�tat de navigation � l'instant T pour un ViewModel
        /// </summary>
        /// <param name="viewModelFromName">ViewModel pris en compte</param>
        /// <param name="isModal">Indique que la navigation est de type modale, support�e uniquement sur la plateforme Xamarin.Forms</param>
        protected override void SetNavigationHistory(Type viewModelFromName, bool isModal = false)
        {
            if (!HistoryNavigation.ContainsKey(viewModelFromName))
            {
                HistoryNavigation.Add(viewModelFromName, _rootFrame.GetNavigationState());
            }
        }

        /// <summary>
        /// Permet de revenir en arri�re dans la pile de navigation des pages
        /// </summary>
        protected bool SetGoBack(Type historyViewModel)
        {
            //Sauvegarde de l'�tat actuel pour revenir en arri�re si il le faut
            var navigationSave = _rootFrame.GetNavigationState();
            string navigationState;

            //Chargement de l'�tat voulu
            if (HistoryNavigation.ContainsKey(historyViewModel))
            {
                if (HistoryNavigation.TryGetValue(historyViewModel, out navigationState))
                {
                    _rootFrame.SetNavigationState(navigationState);  
                }

                //V�rification de la bonne coh�rence
                Type historyType;
                var currentType = _rootFrame.CurrentSourcePageType;
                var result = ViewsRegister.TryGetValue(historyViewModel, out historyType);
                if (result)
                {
                    if (currentType != historyType)
                    {
                        //On reprend la position pr�c�dente car le type de page dans la Frame
                        //ne correspond pas � celui vers lequel on doit revenir
                        _rootFrame.SetNavigationState(navigationSave);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Permet de savoir si l'on peut revenir en arriere au niveau des Frame
        /// </summary>
        /// <returns>R�sultat de la demande</returns>
        protected override bool CanGoBackFrame()
        {
            return _rootFrame.CanGoBack;
        }

        /// <summary>
        /// Vide l'historique de navigation de la classe et de la Frame de WinRT
        /// </summary>
        protected override void ClearNavigation()
        {
            HistoryInstances.Clear();

            //On vide les instances dans SimpleIoc
            foreach (var instance in FactoriesInstances)
            {
                var instanceSimple = SimpleIoc.Default.GetInstance(instance.Key, instance.Value);
                SimpleIoc.Default.Unregister(instanceSimple);
            }
            FactoriesInstances.Clear();
            _rootFrame.SetNavigationState(NavigationStateInitial);
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Fonction par d�faut du retour en arri�re par le bouton phyique ou virtuel du device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HardwareButtonBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (CanGoBack())
            {
                GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Surcharge de la fonction de retour arri�re, d�finie par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HardwareButtonBackPressedOverride(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = _backButtonPressedAction();
        }
#endif

#endregion
    }
}