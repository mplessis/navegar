#region licence
// <copyright file="Navigation.cs" company="Kopigi">
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Class;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;
using Xamarin.Forms;

#endregion

namespace Navegar.XamarinForms
{
    /// <summary>
    /// Implémentation de la classe de navigation
    /// </summary>
    public class Navigation : NavigationBase
    {
        #region fields
        private readonly Dictionary<Type, string> _factoriesInstancesView = new Dictionary<Type, string>();
        private new readonly Dictionary<Type, bool> HistoryNavigation = new Dictionary<Type, bool>(); //La valeur indique si il s'agit d'une navigation modale ou non
        private Page _rootFrame;
        private readonly Dictionary<Type, Func<bool>> _viewsActionOnBackButtonRegister = new Dictionary<Type, Func<bool>>();

        #endregion

        /// <summary>
        /// Indique quelle plateforme est en cours d'exécution
        /// </summary>
        public override CurrentPlatformEnum CurrentPlatform => CurrentPlatformEnum.XAMARINFORMS;

        #region Surcharge de la navigation arriére

        /// <summary>
        /// Indique si le device a un bouton de retour physique ou virtuel
        /// </summary>
        /// <returns>True si un bouton est présent, sinon false</returns>
        public override BackButtonTypeEnum HasBackButton
        {
            get
            {
                return BackButtonTypeEnum.Virtual;
            }
        }

        /// <summary>
        /// Evenement de navigation arriére avec le bouton physique ou virtuel
        /// Permet de définir soit même une fonction gérant ce retour sans utiliser celui par défaut de Navegar
        /// </summary>
        /// <remarks>
        /// Si aucun bouton physique ou virtuel n'est présent sur le device, la valeur est égale à null
        /// </remarks>
        public override Func<bool> BackButtonPressed
        {
            get { throw new NotImplementedForCurrentPlatformException(); }
            set
            {
                throw new NotImplementedForCurrentPlatformException();
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
        public override void RegisterBackPressedAction<TViewModel>(Func<bool> func)
        {
            if (!_viewsActionOnBackButtonRegister.ContainsKey(typeof(TViewModel)))
            {
                _viewsActionOnBackButtonRegister.Add(typeof(TViewModel), func);
            }
        }

        #endregion

        /// <summary>
        /// Permet de référencer la Frame Principale généré au lancement de l'application, pour la suite de la navigation
        /// </summary>
        /// <param name="rootFrame">Frame de navigation principale</param>
        /// <remarks>
        /// Spécifique aux plateformes .netcore
        /// Léve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas implémentée sur la plateforme courante
        /// </remarks>
        public override void InitializeRootFrame(object rootFrame)
        {
            throw new NotImplementedForCurrentPlatformException();
        }

        /// <summary>
        /// Permet de référencer la page principale générée au lancement de l'application, pour la suite de la navigation
        /// </summary>
        public override object InitializeRootFrame<TViewModelFirst, TViewFirst>()
        {
            _rootFrame = (Page)Activator.CreateInstance(typeof(TViewFirst));
            _rootFrame.BindingContext = (TViewModelFirst) Activator.CreateInstance<TViewModelFirst>();
            return new NavigationPage(_rootFrame);
        }

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
        /// <remarks>
        /// Supportée uniquement sur la plateforme Xamarin.Forms, dans les autres cas une exception <see cref="NotImplementedForCurrentPlatformException"/> sera levée
        /// </remarks>
        public override string NavigateModalTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad,
            object[] parametersFunction, bool newInstance = false)
        {
            return Navigate<TTo>(typeof(TTo), currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance, true);
        }

        /// <summary>
        /// Permet d'associer un type pour la vue à un type pour le modéle de vue
        /// </summary>
        public override void RegisterView<TViewModel, TView>()
        {
            if (!ViewsRegister.ContainsKey(typeof(TViewModel)))
            {
                SimpleIoc.Default.Register<TView>();
                ViewsRegister.Add(typeof(TViewModel), typeof(TView));
            }
        }

        /// <summary>
        /// Permet d'associer un type pour la vue à un type pour le modéle de vue 
        /// en incluant si un bouton back virtuel doit etre activé dans la barre de titre de l'application
        /// </summary>
        /// <param name="backVirtualButton">Indique si l'on doit ou non afficher un bouton de retour virtuel</param>
        /// <remarks>
        /// Spécifique à la plateforme .netcore UWP (Windows 10)
        /// Léve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas implémentée sur la plateforme courante
        /// Si le device utilisé posséde un bouton physique cette fonction n'affiche pas de bouton, sauf à forcer l'affichage avec le paramétre
        /// </remarks>
        public override void RegisterView<TViewModel, TView>(BackButtonViewEnum backVirtualButton)
        {
            throw new NotImplementedForCurrentPlatformException();
        }

        /// <summary>
        /// Perrmet d'afficher le bouton virtuel dans la barre de titre de l'application
        /// </summary>
        /// <param name="visible">indique si l'on doit rendre le bouton visible ou non</param>
        /// <param name="force">permet de forcer l'affichage même si le device posséde un bouton physique</param>
        /// <remarks>
        /// Spécifique à la plateforme .netcore UWP (Windows 10)
        /// Léve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas implémentée sur la plateforme courante
        /// Si le device utilisé posséde un bouton physique cette fonction n'affiche pas de bouton, sauf à forcer l'affichage avec le paramétre
        /// </remarks>
        public override void ShowVirtualBackButton(bool visible = true, bool force = false)
        {
            throw new NotImplementedForCurrentPlatformException();
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
        protected override async void Navigate(Type viewModelToName, string functionToLoad, object[] parametersFunction)
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
                        //On récupére l'historique de navigation pour savoir si il s'agit d'une navigation hiérarchique ou modale
                        bool isModal;
                        if (!HistoryNavigation.TryGetValue(viewModelToName, out isModal))
                        {
                            isModal = false;
                        }

                        var result = await SetGoBack(isModal);
                        if (result)
                        {
                            CurrentViewModel = viewModelToName;

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
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </param>
        /// <param name="functionsToLoad">
        /// Permet de définir un dictionnaire contenant les noms des fonctions à appeler aprés le chargement du viewModel ciblé avec leurs paramètres éventuels</param>
        protected override void Navigate(Type viewModelToName, Dictionary<string, object[]> functionsToLoad)
        {
            throw new NotImplementedForCurrentPlatformException();
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
        /// <param name="isModal">
        /// Indique que l'on souhaite une navigation modal, supportée uniquement par la plateforme Xamarin.Forms
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        protected override string Navigate<TTo>(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false, bool isModal = false)
        {
            try
            {
                //Vérification du type de ViewModel demandé pour l'historique
                if (!viewModelToName.GetTypeInfo().IsSubclassOf(typeof(ViewModelBase)))
                {
                    throw new ViewModelHistoryTypeException(viewModelToName.ToString());
                }

                //Pré-navigation
                PreNavigationArgs preNavigationArgs;
                if (!PreNavigateTo(viewModelFromName, viewModelToName, out preNavigationArgs))
                {
                    OnNavigationCancel();
                    return string.Empty;
                }

                //On remplace la fonction désignée par celle ajoutée à la pre-navigation
                if (preNavigationArgs != null)
                {
                    functionToLoad = preNavigationArgs.FunctionToLoad;
                    parametersFunction = preNavigationArgs.ParametersFunctionToLoad;
                }

                //Gestion de l'historique
                HistoryNavigateTo(viewModelFromName, viewModelToName, isModal);

                //Génération d'une instance du viewmodel
                var key = GenerateNewInstanceViewModelNavigateTo<TTo>(viewModelToName, parametersViewModel, newInstance);

                var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, key);
                if (instance != null)
                {

                    CurrentViewModel = viewModelToName;

                    //On cherche la page correspondante dans les enregistrements
                    var typePage = GetPageRegisterWithViewModel(CurrentViewModel);
                    if (typePage != null)
                    {
                        SetCurrentView(instance, typePage, isModal);
                    }
                }

                //Gestion d'une fonction à appeler suite à la génération de l'instance
                LoadFunctionViewModelNavigateTo<TTo>(instance, functionToLoad, parametersFunction);

                return key;
            }
            catch (Exception e)
            {
                throw new NavigationException(e);
            }
        }

        /// <summary>
        /// Permet de retrouver le type de vue à afficher
        /// </summary>
        /// <param name="viewModelName">Nom du viewModel utilisé</param>
        /// <returns></returns>
        protected Type GetPageRegisterWithViewModel(Type viewModelName)
        {
            if (ViewsRegister.ContainsKey(viewModelName))
            {
                return ViewsRegister[viewModelName];
            }
            return null;
        }

        /// <summary>
        /// Affecte la propriété de view courante du ViewModel principal
        /// </summary>
        /// <param name="instanceToNavigate">
        /// Instance de la vue qui est devenue la vue courante
        /// </param>
        /// <param name="modal">
        /// Indique si l'on souhaite que la navigation soit modale
        /// </param>
        protected async void SetCurrentView(ViewModelBase instanceToNavigate, Type typePage, bool modal = false)
        {
            try
            {
                string key;
                if (!_factoriesInstancesView.ContainsKey(typePage))
                {
                    var instanceNew = Activator.CreateInstance(typePage);
                    key = Guid.NewGuid().ToString();
                    SimpleIoc.Default.Register(() => instanceNew, key);
                    _factoriesInstancesView.Add(typePage, key);
                }
                else
                {
                    _factoriesInstancesView.TryGetValue(typePage, out key);
                }
                
                var contentPage = (Page) SimpleIoc.Default.GetInstance(typePage, key);

                //Définition du BindingContext
                contentPage.BindingContext = instanceToNavigate;

                //Association de l'override éventuel du OnBackButtonPressed de la page
                AssignActionBackPressed(contentPage, instanceToNavigate);

                //Navigation
                if (!modal)
                {
                    await _rootFrame.Navigation.PushAsync(contentPage);
                }
                else
                {
                    await _rootFrame.Navigation.PushModalAsync(contentPage);
                }
                
            }
            catch (Exception e)
            {
                throw new FrameNavigationException();
            }
        }

        /// <summary>
        /// Permet d'assigner la fonction gérée par le bouton de retour arriére
        /// </summary>
        /// <param name="contentPage"></param>
        /// <param name="instanceToNavigate"></param>
        protected void AssignActionBackPressed(Page contentPage, ViewModelBase instanceToNavigate)
        {
            //Association de l'override éventuel du OnBackButtonPressed de la page
            if (contentPage is NavegarContentPage)
            {
                var func = GetActionOnBackButton(instanceToNavigate.GetType());
                ((NavegarContentPage)contentPage).OnBackPressed = func ?? HardwareButtonsBackPressed;
            }
            else
            {
                if (contentPage is NavegarMasterDetailPage)
                {
                    var func = GetActionOnBackButton(instanceToNavigate.GetType());
                    ((NavegarMasterDetailPage)contentPage).OnBackPressed = func ?? HardwareButtonsBackPressed;
                }
                else
                {
                    if (contentPage is NavegarCarouselPage)
                    {
                        var func = GetActionOnBackButton(instanceToNavigate.GetType());
                        ((NavegarCarouselPage)contentPage).OnBackPressed = func ?? HardwareButtonsBackPressed;
                    }
                    else
                    {
                        if (contentPage is NavegarTabbedPage)
                        {
                            var func = GetActionOnBackButton(instanceToNavigate.GetType());
                            ((NavegarTabbedPage)contentPage).OnBackPressed = func ?? HardwareButtonsBackPressed;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permet de gérer l'état de navigation à l'instant T pour un ViewModel
        /// </summary>
        /// <param name="viewModelFromName">ViewModel pris en compte</param>
        /// <param name="isModal">Indique que la navigation est de type modale, supportée uniquement sur la plateforme Xamarin.Forms</param>
        protected override void SetNavigationHistory(Type viewModelFromName, bool isModal = false)
        {
            if (!HistoryNavigation.ContainsKey(viewModelFromName))
            {
                HistoryNavigation.Add(viewModelFromName, isModal);
            }
        }

        /// <summary>
        /// Permet de revenir en arriére dans la pile de navigation des pages
        /// </summary>
        protected async Task<bool> SetGoBack(bool isModal = false)
        {
            //On revient en arriére
            if (!isModal)
            {
                await _rootFrame.Navigation.PopAsync();
            }
            else
            {
                await _rootFrame.Navigation.PopModalAsync();
            }
            return true;
        }

        /// <summary>
        /// Permet de savoir si l'on peut revenir en arriere au niveau des Frame (pour la plateforme Xamarin true est toujours renvoyé)
        /// </summary>
        /// <returns>Résultat de la demande</returns>
        protected override bool CanGoBackFrame()
        {
            return true;
        }

        /// <summary>
        /// Vide l'historique de navigation de la classe et de la Frame de WinRT
        /// </summary>
        protected override void ClearNavigation()
        {
            base.ClearNavigation();

            //On vide les instances de vues dans SimpleIoc
            foreach (var instance in _factoriesInstancesView)
            {
                var instanceSimple = SimpleIoc.Default.GetInstance(instance.Key, instance.Value);
                SimpleIoc.Default.Unregister(instanceSimple);
            }
            _factoriesInstancesView.Clear();
            _rootFrame.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Permet de écupérer la fonction à associer à la page pour OnBackButtonPressed
        /// </summary>
        /// <param name="viewModel">Le Viewmodel associé</param>
        /// <returns></returns>
        protected Func<bool> GetActionOnBackButton(Type viewModel)
        {
            Func<bool> func;
            if (_viewsActionOnBackButtonRegister.TryGetValue(viewModel, out func))
            {
                return func;
            }
            return null;
        } 

        /// <summary>
        /// Fonction par défaut du retour en arriére par le bouton phyique ou virtuel du device
        /// </summary>
        protected bool HardwareButtonsBackPressed()
        {
            if (CanGoBack())
            {
                GoBack();
                return true;
            }
            return false;
        }

        #endregion
    }
}