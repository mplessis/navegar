#region licence
// <copyright file="Navigation.cs" company="Kopigi">
// Copyright � Kopigi 2015
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
    /// Impl�mentation de la classe de navigation
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
        /// Indique quelle plateforme est en cours d'ex�cution
        /// </summary>
        public override CurrentPlatformEnum CurrentPlatform => CurrentPlatformEnum.XAMARINFORMS;

        #region Surcharge de la navigation arri�re

        /// <summary>
        /// Indique si le device a un bouton de retour physique ou virtuel
        /// </summary>
        /// <returns>True si un bouton est pr�sent, sinon false</returns>
        public override BackButtonTypeEnum HasBackButton
        {
            get
            {
                return BackButtonTypeEnum.Virtual;
            }
        }

        /// <summary>
        /// Evenement de navigation arri�re avec le bouton physique ou virtuel
        /// Permet de d�finir soit m�me une fonction g�rant ce retour sans utiliser celui par d�faut de Navegar
        /// </summary>
        /// <remarks>
        /// Si aucun bouton physique ou virtuel n'est pr�sent sur le device, la valeur est �gale � null
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
        public override void RegisterBackPressedAction<TViewModel>(Func<bool> func)
        {
            if (!_viewsActionOnBackButtonRegister.ContainsKey(typeof(TViewModel)))
            {
                _viewsActionOnBackButtonRegister.Add(typeof(TViewModel), func);
            }
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
            throw new NotImplementedForCurrentPlatformException();
        }

        /// <summary>
        /// Permet de r�f�rencer la page principale g�n�r�e au lancement de l'application, pour la suite de la navigation
        /// </summary>
        public override object InitializeRootFrame<TViewModelFirst, TViewFirst>()
        {
            _rootFrame = (Page)Activator.CreateInstance(typeof(TViewFirst));
            _rootFrame.BindingContext = (TViewModelFirst) Activator.CreateInstance<TViewModelFirst>();
            return new NavigationPage(_rootFrame);
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
            return Navigate<TTo>(typeof(TTo), currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance, true);
        }

        /// <summary>
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue
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
        /// Permet d'associer un type pour la vue � un type pour le mod�le de vue 
        /// en incluant si un bouton back virtuel doit etre activ� dans la barre de titre de l'application
        /// </summary>
        /// <param name="backVirtualButton">Indique si l'on doit ou non afficher un bouton de retour virtuel</param>
        /// <remarks>
        /// Sp�cifique � la plateforme .netcore UWP (Windows 10)
        /// L�ve une exception <exception cref="NotImplementedException" /> si la fonction n'est pas impl�ment�e sur la plateforme courante
        /// Si le device utilis� poss�de un bouton physique cette fonction n'affiche pas de bouton, sauf � forcer l'affichage avec le param�tre
        /// </remarks>
        public override void RegisterView<TViewModel, TView>(BackButtonViewEnum backVirtualButton)
        {
            throw new NotImplementedForCurrentPlatformException();
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
            throw new NotImplementedForCurrentPlatformException();
        }

        public override void Dispose()
        {
            ClearNavigation();
            GC.Collect();
        }

        #region private

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
                        //On r�cup�re l'historique de navigation pour savoir si il s'agit d'une navigation hi�rarchique ou modale
                        bool isModal;
                        if (!HistoryNavigation.TryGetValue(viewModelToName, out isModal))
                        {
                            isModal = false;
                        }

                        var result = await SetGoBack(isModal);
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
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
        /// </summary>
        /// <param name="viewModelToName">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </param>
        /// <param name="functionsToLoad">
        /// Permet de d�finir un dictionnaire contenant les noms des fonctions � appeler apr�s le chargement du viewModel cibl� avec leurs param�tres �ventuels</param>
        protected override void Navigate(Type viewModelToName, Dictionary<string, object[]> functionsToLoad)
        {
            throw new NotImplementedForCurrentPlatformException();
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
                PreNavigationArgs preNavigationArgs;
                if (!PreNavigateTo(viewModelFromName, viewModelToName, out preNavigationArgs))
                {
                    OnNavigationCancel();
                    return string.Empty;
                }

                //On remplace la fonction d�sign�e par celle ajout�e � la pre-navigation
                if (preNavigationArgs != null)
                {
                    functionToLoad = preNavigationArgs.FunctionToLoad;
                    parametersFunction = preNavigationArgs.ParametersFunctionToLoad;
                }

                //Gestion de l'historique
                HistoryNavigateTo(viewModelFromName, viewModelToName, isModal);

                //G�n�ration d'une instance du viewmodel
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

                //Gestion d'une fonction � appeler suite � la g�n�ration de l'instance
                LoadFunctionViewModelNavigateTo<TTo>(instance, functionToLoad, parametersFunction);

                return key;
            }
            catch (Exception e)
            {
                throw new NavigationException(e);
            }
        }

        /// <summary>
        /// Permet de retrouver le type de vue � afficher
        /// </summary>
        /// <param name="viewModelName">Nom du viewModel utilis�</param>
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
        /// Affecte la propri�t� de view courante du ViewModel principal
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

                //D�finition du BindingContext
                contentPage.BindingContext = instanceToNavigate;

                //Association de l'override �ventuel du OnBackButtonPressed de la page
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
        /// Permet d'assigner la fonction g�r�e par le bouton de retour arri�re
        /// </summary>
        /// <param name="contentPage"></param>
        /// <param name="instanceToNavigate"></param>
        protected void AssignActionBackPressed(Page contentPage, ViewModelBase instanceToNavigate)
        {
            //Association de l'override �ventuel du OnBackButtonPressed de la page
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
        /// Permet de g�rer l'�tat de navigation � l'instant T pour un ViewModel
        /// </summary>
        /// <param name="viewModelFromName">ViewModel pris en compte</param>
        /// <param name="isModal">Indique que la navigation est de type modale, support�e uniquement sur la plateforme Xamarin.Forms</param>
        protected override void SetNavigationHistory(Type viewModelFromName, bool isModal = false)
        {
            if (!HistoryNavigation.ContainsKey(viewModelFromName))
            {
                HistoryNavigation.Add(viewModelFromName, isModal);
            }
        }

        /// <summary>
        /// Permet de revenir en arri�re dans la pile de navigation des pages
        /// </summary>
        protected async Task<bool> SetGoBack(bool isModal = false)
        {
            //On revient en arri�re
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
        /// Permet de savoir si l'on peut revenir en arriere au niveau des Frame (pour la plateforme Xamarin true est toujours renvoy�)
        /// </summary>
        /// <returns>R�sultat de la demande</returns>
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
        /// Permet de �cup�rer la fonction � associer � la page pour OnBackButtonPressed
        /// </summary>
        /// <param name="viewModel">Le Viewmodel associ�</param>
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
        /// Fonction par d�faut du retour en arri�re par le bouton phyique ou virtuel du device
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