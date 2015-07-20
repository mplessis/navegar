﻿// <copyright file="Navigation.cs" company="Kopigi">
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

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Class;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;
using Navegar.Libs.Interfaces;

#endregion

namespace Navegar.Plateformes.NetCore.UWP.Win10
{
    /// <summary>
    /// Implémentation de la classe de navigation
    /// </summary>
    /// <example>
    ///   On réalise la navigation suivante :
    /// 
    ///   MainViewModel -&gt; FirstViewModel &lt;-&gt; SecondViewModel
    ///  
    ///   <code>
    /// Dans le ViewModelLocator.cs :
    /// 
    /// public ViewModelLocator()
    /// {
    ///    ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
    ///
    ///    //Enregistrer la classe de navigation dans l'IOC et les ViewModels
    ///    SimpleIoc.Default.Register&lt;INavigation, Navigation&gt;();
    ///    SimpleIoc.Default.Register&lt;MainViewModel&gt;();
    ///
    ///    //Association des vues avec leur modéle de vue
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().RegisterView&lt;BlankPage1ViewModel, BlankPage1&gt;();
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().RegisterView&lt;BlankPage2ViewModel, BlankPage2&gt;();
    ///  }
    ///
    ///  public MainViewModel Main
    ///  {
    ///     get { return SimpleIoc.Default.GetInstance&lt;MainViewModel&gt;(); }
    ///  }
    ///
    ///  public BlankPage1ViewModel BlankPage1ViewModel
    ///  {
    ///     get { return SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GetViewModelInstance&lt;BlankPage1ViewModel&gt;(); }
    ///  }
    ///
    ///  public BlankPage2ViewModel BlankPage2ViewModel
    ///  {
    ///     get { return SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GetViewModelInstance&lt;BlankPage2ViewModel&gt;(); }
    ///  }
    /// 
    /// 
    /// Dans MainViewModel.cs :
    /// 
    ///    //Pour aller vers un autre ViewModel
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().NavigateTo&lt;FirstViewModel&gt;();
    /// 
    /// 
    /// 
    /// Dans BlankPage1ViewModel.cs :
    /// 
    ///    //Pour aller vers SecondViewModel.cs, en supposant que le constructeur prenne un argument et que l'on veuille revenir vers FirstViewModel
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().NavigateTo&lt;BlankPage2ViewModel&gt;(this, new object[] { Data }, true);
    /// 
    /// 
    /// 
    /// Dans BlankPage2ViewModel.cs :
    /// 
    ///    //Pour revenir vers BlankPage1ViewModel :
    ///    if(SimpleIoc.Default.GetInstance&lt;INavigation&gt;().CanGoBack())
    ///    {
    ///       SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GoBack();
    ///    }
    ///   </code>
    /// </example>
    public class Navigation : NavigationBase
    {
        #region fields

        private Frame _rootFrame;

        private Func<bool> _backButtonPressedAction;

        #endregion

        #region Surcharge de la navigation arriére

        /// <summary>
        /// Indique si le device a un bouton de retour physique ou virtuel
        /// </summary>
        /// <returns>True si un bouton est présent, sinon false</returns>
        public override BackButtonTypeEnum HasBackButton
        {
            get
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                {
                    return BackButtonTypeEnum.Physical;
                }
                else
                {
                    if (Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == Windows.UI.Core.AppViewBackButtonVisibility.Visible)
                    {
                        return BackButtonTypeEnum.Virtual;
                    }
                    else
                    {
                        return BackButtonTypeEnum.None;
                    }
                }
            }
        }

        private Func<bool> _backButtonPressed;
        /// <summary>
        /// Evenement de navigation arriére avec le bouton physique ou virtuel
        /// Permet de définir soit même une fonction gérant ce retour sans utiliser celui par défaut de Navegar
        /// </summary>
        /// <remarks>
        /// Si aucun bouton physique ou virtuel n'est présent sur le device, la valeur est égale à null
        /// </remarks>
        public override Func<bool> BackButtonPressed
        {
            get { return _backButtonPressed; }
            set
            {
                if (HasBackButton == BackButtonTypeEnum.Physical)
                {
                    _backButtonPressedAction = value;
                    if (value != null)
                    {
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonsBackPressed;
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressedOverride;
                    }
                    else
                    {
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtonBackPressedOverride;
                        Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
                    }
                    _backButtonPressed = value;
                }
                else
                {
                    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
                    {
                        _backButtonPressedAction = value;
                        if (value != null)
                        {
                            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= VirtualBackPressed;
                            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += VirtualBackPressedOverride;
                        }
                        else
                        {
                            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= VirtualBackPressedOverride;
                            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += VirtualBackPressed;

                        }
                        _backButtonPressed = value;
                    }
                    else
                    {
                        _backButtonPressed = null;
                        _backButtonPressedAction = null;
                    }
                }
            }
        }

        /// <summary>
        /// Permet de faire un override de OnBackButtonPressed pour la page associée au ViewModel.
        /// Attention il faut que page hérite de NavegarContentPage pour que cela soit pris en compte.
        /// Si votre page hérite bien de NavegarContentPage mais que vous ne définissez de fonction personnalisée, celle par défaut de Navegar sera appliquée
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel associé</typeparam>
        /// <param name="func">Fonction personnalisée pour le OnBackButtonPressed</param>
        /// <remarks>
        /// Votre fonction doit retourner false pour permetre de continuer la naigation arriére, sinon elle sera stoppée.
        /// Spécifique à la plateforme Xamarin.Forms
        /// Léve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas implémentée sur la plateforme courante
        /// </remarks>
        public override void RegisterBackPressedAction<TViewModel>(Action func)
        {
            throw new NotImplementedForCurrentPlatformException();
        }
        #endregion

        /// <summary>
        /// Permet de référencer la Frame Principale généré au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <param name="rootFrame">Frame de navigation principale</param>
        public override void InitializeRootFrame(object rootFrame)
        {
            if (!(rootFrame is Frame))
            {
                throw new TypeObjectIsNotIncorrectException("Frame");
            }

            _navigationStateInitial = ((Frame)rootFrame).GetNavigationState();
            _rootFrame = (Frame)rootFrame;

            if (HasBackButton == BackButtonTypeEnum.Physical)
            {
                //Gestion du bouton de retour physique du device
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            }

            if(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
            {
                //Gestion du bouton de retour vituel du device
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += VirtualBackPressed;
            }
        }

        /// <summary>
        /// Permet de référencer la page principale générée au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <remarks>
        /// Spécifique à la plateforme Xamarin.Forms
        /// Léve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas implémentée sur la plateforme courante
        /// </remarks>
        public override object InitializeRootFrame<TViewModelFirst, TViewFirst>()
        {
            throw new NotImplementedForCurrentPlatformException();
        }

        /// <summary>
        /// Permet d'associer un type pour la vue à un type pour le modéle de vue
        /// </summary>
        public override void RegisterView<TViewModel, TView>()
        {
            if (!_viewsRegister.ContainsKey(typeof(TViewModel)))
            {
                _viewsRegister.Add(typeof(TViewModel), typeof(TView));
            }
        }

        /// <summary>
        /// Perrmet d'afficher le bouton virtuel dans la barre de titre de l'application
        /// </summary>
        /// <param name="visible">indique si l'on doit rendre le bouton visible ou non</param>
        /// <param name="force">permet de forcer l'affichage même si le device posséde un bouton physique</param>
        /// <remarks>
        /// Si le device utilisé posséde un bouton physique cette fonction n'affiche pas de bouton, sauf à forcer l'affichage avec le paramétre
        /// Supporté uniquement sur la version desktop de Windows 10
        /// </remarks>
        public override void ShowVirtualBackButton(bool visible = true, bool force = false)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Core.SystemNavigationManager"))
            {
                if((HasBackButton == BackButtonTypeEnum.None && HasBackButton != BackButtonTypeEnum.Virtual) || force)
                {
                    Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = visible ? Windows.UI.Core.AppViewBackButtonVisibility.Visible : Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
                }
                else
                {
                    if (!visible)
                    {
                        Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                }
            }
        }

        public override void Dispose()
        {
            ClearNavigation();
            GC.Collect();
        }

        #region private

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours
        /// </summary>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </param>
        /// <param name="functionToLoad">
        /// Permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </param>
        /// <param name="parametersFunction">
        /// Paramétres pour la fonction appelée
        /// </param>
        protected override void Navigate(Type viewModelToName, string functionToLoad, object[] parametersFunction)
        {
            if (viewModelToName != null)
            {
                string key = String.Empty;

                if (_factoriesInstances.ContainsKey(viewModelToName))
                {
                    _factoriesInstances.TryGetValue(viewModelToName, out key);
                }

                if (!String.IsNullOrEmpty(key))
                {
                    var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, key);
                    if (instance != null)
                    {
                        var result = SetGoBack(viewModelToName);
                        if (result)
                        {
                            _currentViewModel = viewModelToName;

                            //Gestion d'une fonction à appeler suite à la génération de l'instance
                            //La recherche de la méthode doit se faire sur le type de l'instance
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
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </param>
        /// <param name="viewModelFromName">
        /// Type du Viewmodel depuis lequel la navigation est effectuée
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
        protected override string Navigate<TTo>(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            try
            {
                //Pré-navigation
                if (PreviewNavigate != null)
                {
                    ViewModelBase currentInstance = null;
                    if (viewModelFromName != null)
                    {
                        if (_factoriesInstances.ContainsKey(viewModelToName))
                        {
                            string keyInstance;
                            if (_factoriesInstances.TryGetValue(viewModelToName, out keyInstance))
                            {
                                currentInstance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, keyInstance);
                            }
                        }
                    }

                    if (!PreviewNavigate(currentInstance, viewModelFromName, viewModelToName))
                    {
                        OnNavigationCancel();
                        return string.Empty;
                    }
                }

                //Vérification du type de ViewModel demandé pour l'historique
                if (!viewModelToName.GetTypeInfo().IsSubclassOf(typeof(ViewModelBase)))
                {
                    throw new ViewModelHistoryTypeException(viewModelToName.ToString());
                }

                //Gestion de l'historique
                if (viewModelFromName != null)
                {
                    if (_historyInstances.ContainsKey(viewModelToName))
                    {
                        _historyInstances[viewModelToName] = viewModelFromName;
                    }
                    else
                    {
                        _historyInstances.Add(viewModelToName, viewModelFromName);
                    }

                    //Gestion de l'historique de navigation
                    SetNavigationHistory(viewModelFromName);
                }

                //Génération d'une instance du viewmodel
                string key;
                if (_factoriesInstances.ContainsKey(viewModelToName) && !newInstance)
                {
                    _factoriesInstances.TryGetValue(viewModelToName, out key);
                }
                else
                {
                    if (_factoriesInstances.ContainsKey(viewModelToName))
                    {
                        //Suppression de l'instance du viewModel dans le cache de SimpleIOC
                        _factoriesInstances.TryGetValue(viewModelToName, out key);

                        if (key != null)
                        {
                            _factoriesInstances.Remove(viewModelToName);
                            SimpleIoc.Default.Unregister<TTo>(key);
                        }
                    }

                    var instanceNew = Activator.CreateInstance(viewModelToName, parametersViewModel);
                    key = Guid.NewGuid().ToString();
                    SimpleIoc.Default.Register<TTo>(() => (TTo)instanceNew, key);
                    _factoriesInstances.Add(viewModelToName, key);
                }

                var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, key);
                if (instance != null)
                {
                    Type instanceToNavigate;
                    var result = _viewsRegister.TryGetValue(instance.GetType(), out instanceToNavigate);
                    if (result)
                    {
                        _currentViewModel = viewModelToName;
                        SetCurrentView(instanceToNavigate);
                    }
                }

                //Gestion d'une fonction à appeler suite à la génération de l'instance
                if (!string.IsNullOrEmpty(functionToLoad))
                {
                    var method = typeof(TTo).GetMethod(functionToLoad, parametersFunction);
                    if (method != null)
                    {
                        method.Invoke(instance, parametersFunction);
                    }
                    else
                    {
                        var countParameters = parametersFunction != null
                                                  ? ((IEnumerable<object>)parametersFunction).Count()
                                                  : 0;
                        throw new FunctionToLoadNavigationException(string.Format("{0} with {1} parameter(s)", functionToLoad, countParameters), typeof(TTo).Name);
                    }
                }

                return key;
            }
            catch (Exception e)
            {
                throw new NavigationException(e);
            }
        }


        /// <summary>
        /// Affecte la propriété de view courante du ViewModel principal
        /// </summary>
        /// <param name="instanceToNavigate">
        /// Instance de la vue qui est devenue la vue courante
        /// </param>
        protected void SetCurrentView(Type instanceToNavigate)
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
        /// Permet de gérer l'état de navigation à l'instant T pour un ViewModel
        /// </summary>
        /// <param name="viewModelFromName">ViewModel pris en compte</param>
        protected override void SetNavigationHistory(Type viewModelFromName)
        {
            if (!_historyNavigation.ContainsKey(viewModelFromName))
            {
                _historyNavigation.Add(viewModelFromName, _rootFrame.GetNavigationState());
            }
        }

        /// <summary>
        /// Permet de revenir en arriére dans la pile de navigation des pages
        /// </summary>
        protected bool SetGoBack(Type historyViewModel)
        {
            //Sauvegarde de l'état actuel pour revenir en arriére si il le faut
            var navigationSave = _rootFrame.GetNavigationState();
            string navigationState;

            //Chargement de l'état voulu
            if (_historyNavigation.ContainsKey(historyViewModel))
            {
                if (_historyNavigation.TryGetValue(historyViewModel, out navigationState))
                {
                    _rootFrame.SetNavigationState(navigationState);
                }

                //Vérification de la bonne cohérence
                Type historyType;
                var currentType = _rootFrame.CurrentSourcePageType;
                var result = _viewsRegister.TryGetValue(historyViewModel, out historyType);
                if (result)
                {
                    if (currentType != historyType)
                    {
                        //On reprend la position précédente car le type de page dans la Frame
                        //ne correspond pas à celui vers lequel on doit revenir
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
        /// <returns>Résultat de la demande</returns>
        protected override bool CanGoBackFrame()
        {
            return _rootFrame.CanGoBack;
        }

        /// <summary>
        /// Vide l'historique de navigation de la classe et de la Frame de WinRT
        /// </summary>
        protected override void ClearNavigation()
        {
            _historyInstances.Clear();

            //On vide les instances dans SimpleIoc
            foreach (var instance in _factoriesInstances)
            {
                var instanceSimple = SimpleIoc.Default.GetInstance(instance.Key, instance.Value);
                SimpleIoc.Default.Unregister(instanceSimple);
            }
            _factoriesInstances.Clear();
            _rootFrame.SetNavigationState(_navigationStateInitial);
        }

        /// <summary>
        /// Fonction par défaut du retour en arriére par le bouton phyique du device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (CanGoBack())
            {
                GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Surcharge de la fonction de retour arriére sur le bouton physique, définie par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HardwareButtonBackPressedOverride(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = _backButtonPressedAction();
        }

        /// <summary>
        /// Fonction par défaut du retour en arriére par le bouton virtuel du device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VirtualBackPressed(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (CanGoBack())
            {
                GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Surcharge de la fonction de retour arriére sur le bouton virtuel, définie par l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void VirtualBackPressedOverride(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            e.Handled = _backButtonPressedAction();
        }
        #endregion
    }
}
