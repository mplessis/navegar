#region licence
// <copyright file="Navigation.cs" company="Kopigi">
// Copyright © Kopigi 2015
// </copyright>
// ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>21/07/2015</date>
// <project>Navegar.Plateformes.Net.WPF</project>
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
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Class;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;
using Navegar.Libs.Interfaces;

#endregion

namespace Navegar.Plateformes.Net.WPF
{

    /// <summary>
    /// Implémentation de la classe de navigation
    /// </summary>
    public class Navigation : INavigationWpf
    {
        #region fields

        private readonly Dictionary<Type, string> _factoriesInstances = new Dictionary<Type, string>();
        private readonly Dictionary<Type, Type> _historyInstances = new Dictionary<Type, Type>();
        private Type _currentViewModel;
        private string _keyMain;
        private object _mainViewModel;

        #endregion

        /// <summary>
        /// Déterminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        public bool CanGoBack()
        {
            return _currentViewModel != null && _historyInstances.ContainsKey(_currentViewModel);
        }

        /// <summary>
        /// Nettoie l'ensemble de l'historique de navigation
        /// </summary>
        public void Clear()
        {
            if (_historyInstances != null)
            {
                _historyInstances.Clear();   
            }

            if (_factoriesInstances != null)
            {
                foreach (var instance in _factoriesInstances)
                {
                    SimpleIoc.Default.Unregister(instance.Key);
                }
                _factoriesInstances.Clear();
            }
            GC.Collect();
        }

        /// <summary>
        /// Indique quelle plateforme est en cours d'exécution
        /// </summary>
        public CurrentPlatformEnum CurrentPlatform => CurrentPlatformEnum.WPF;

        /// <summary>
        /// Génére l'instance du ViewModel principal
        /// </summary>
        /// <typeparam name="TMain">
        /// Type du ViewModel principal
        /// </typeparam>
        public void GenerateMainViewModelInstance<TMain>() where TMain : class, new()
        {
            _keyMain = Guid.NewGuid().ToString();
            SimpleIoc.Default.Register(() => new TMain(), _keyMain);
            _mainViewModel = SimpleIoc.Default.GetInstance<TMain>(_keyMain);
        }

        /// <summary>
        /// Récupére l'instance du ViewModel principal
        /// </summary>
        /// <typeparam name="TMain">
        /// Type du ViewModel principal
        /// </typeparam>
        /// <returns>
        /// Instance du ViewModel principal
        /// </returns>
        public TMain GetMainViewModelInstance<TMain>() where TMain : class
        {
            if(_mainViewModel != null)
            {
                _mainViewModel = SimpleIoc.Default.GetInstance<TMain>(_keyMain);
            }
            return (TMain)_mainViewModel;
        }

        /// <summary>
        /// Permet de connaitre le type du ViewModel au niveau n-1 de l'historique de navigation
        /// </summary>
        /// <returns>Type du ViewModel</returns>
        public Type GetTypeViewModelToBack()
        {
            if (CanGoBack())
            {
                if (_historyInstances.ContainsKey(_currentViewModel))
                {
                    Type viewModelFrom;
                    if (_historyInstances.TryGetValue(_currentViewModel, out viewModelFrom))
                    {
                        return viewModelFrom;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Permet de retrouver l'instance du ViewModel courant
        /// </summary>
        /// <returns>ViewModel courant</returns>
        public ViewModelBase GetViewModelCurrent()
        {
            if (_factoriesInstances.ContainsKey(_currentViewModel))
            {
                string key;
                if (_factoriesInstances.TryGetValue(_currentViewModel, out key))
                {
                    return (ViewModelBase)SimpleIoc.Default.GetInstance(typeof(ViewModelBase), key);
                }
            }
            return null;
        }

        /// <summary>
        /// Récupére l'instance du ViewModel
        /// </summary>
        /// <typeparam name="T">
        /// Type du ViewModel
        /// </typeparam>
        /// <returns>
        /// Instance du ViewModel
        /// </returns>
        public T GetViewModelInstance<T>() where T : ViewModelBase
        {
            if (_factoriesInstances != null && _factoriesInstances.ContainsKey(typeof(T)))
            {
                string key;
                var result = _factoriesInstances.TryGetValue(typeof(T), out key);
                if (result)
                {
                    return SimpleIoc.Default.GetInstance<T>(key);
                }
            }
            return null;
        }

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        /// /// <param name="functionToLoad">
        /// Permet de spécifier un nom de fonction à appeler aprés le chargement du viewModel ciblé
        /// </param>
        /// <param name="parametersFunction">
        /// Paramétres pour la fonction appelée
        /// </param>
        public void GoBack(string functionToLoad, params object[] parametersFunction)
        {
            if (CanGoBack())
            {
                if (_historyInstances.ContainsKey(_currentViewModel))
                {
                    Type viewModelFrom;
                    if (_historyInstances.TryGetValue(_currentViewModel, out viewModelFrom))
                    {
                        NavigateHistory(viewModelFrom, functionToLoad, parametersFunction);
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
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        public string NavigateTo(Type TTo, params object[] parametersViewModel)
        {
            return Navigate(TTo, null, parametersViewModel, null, null);
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
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
        public string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, null, null);
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        public string NavigateTo(Type TTo, bool newInstance, params object[] parametersViewModel)
        {
            return Navigate(TTo, null, parametersViewModel, null, null, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectuée
        /// </typeparam>
        /// <param name="newInstance">
        /// Indique si l'on génére une nouvelle instance obligatoirement
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des paramétres éventuels à transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        public string NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, null, null, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel
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
        public string NavigateTo(Type TTo, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            return Navigate(TTo, null, parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel
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
        public string NavigateTo<TTo>(object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel, avec un ViewModel en historique précédent
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
        public string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false)
        {
            return Navigate(TTo, currentInstance.GetType(), parametersViewModel, null, null, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel, avec un ViewModel en historique précédent
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
        public string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), currentInstance.GetType(), parametersViewModel, null, null, newInstance);
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
        public string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            return Navigate(TTo, currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance);
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
        public string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        public void GoBack ()
        {
            if (CanGoBack()) 
            {
                if(_historyInstances.ContainsKey(_currentViewModel))
                {
                    Type viewModelFrom;
                    if(_historyInstances.TryGetValue(_currentViewModel, out viewModelFrom))
                    {
                        NavigateHistory(viewModelFrom, string.Empty, null);    
                    }
                }
            }
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
        private void NavigateHistory(Type viewModelToName, string functionToLoad, object[] parametersFunction)
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
                    var instance = (ViewModelBase) SimpleIoc.Default.GetInstance(typeof(ViewModelBase), key);
                    if (instance != null)
                    {
                        _currentViewModel = viewModelToName;
                        SetCurrentView(instance);

                        //Gestion d'une fonction à appeler suite à la génération de l'instance
                        if (!string.IsNullOrEmpty(functionToLoad))
                        {
                            var method = viewModelToName.GetMethod(functionToLoad, parametersFunction);

                            if (method != null)
                            {
                                method.Invoke(instance, parametersFunction);
                            }
                            else
                            {
                                throw new FunctionToLoadNavigationException(functionToLoad, instance.GetType().Name);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
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
        private string Navigate(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            try
            {
                //Vérification du type de ViewModel demandé pour l'historique
                if (!viewModelToName.IsSubclassOf(typeof(ViewModelBase)))
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
                }

                //Génération d'une instance du viewmodel
                string key;
                if (_factoriesInstances.ContainsKey(viewModelToName) && !newInstance)
                {
                    _factoriesInstances.TryGetValue(viewModelToName, out key);
                }
                else
                {
                    if(_factoriesInstances.ContainsKey(viewModelToName))
                    {
                        //Suppression de l'instance du viewModel dans le cache de SimpleIOC
                        _factoriesInstances.TryGetValue(viewModelToName, out key);

                        if (key != null)
                        {
                            _factoriesInstances.Remove(viewModelToName);
                            //SimpleIoc.Default.Unregister<TTo>(key);  
                            SimpleIoc.Default.Unregister(key);
                        }
                    }

                    var instanceNew = Activator.CreateInstance(viewModelToName, parametersViewModel);
                    key = Guid.NewGuid().ToString();
                    SimpleIoc.Default.Register<ViewModelBase>(() => (ViewModelBase)instanceNew, key);
                    _factoriesInstances.Add(viewModelToName, key);
                }

                var instance = (ViewModelBase)SimpleIoc.Default.GetInstance(typeof(ViewModelBase), key);
                if (instance != null)
                {
                    _currentViewModel = viewModelToName;
                    SetCurrentView(instance);

                    //Gestion d'une fonction à appeler suite à la génération de l'instance
                    if (!string.IsNullOrEmpty(functionToLoad))
                    {
                        var method = viewModelToName.GetMethod(functionToLoad, parametersFunction);
                        if (method != null)
                        {
                            method.Invoke(instance, parametersFunction);
                        }
                        else
                        {
                            throw new FunctionToLoadNavigationException(functionToLoad, instance.GetType().Name);
                        }
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
        private void SetCurrentView(ViewModelBase instanceToNavigate)
        {
            var type = _mainViewModel.GetType();
            var prop = CurrentViewNavigationAttribute.GetCurrentViewProperty(type);
            _mainViewModel.GetType().GetProperty(prop.Name).SetValue(_mainViewModel, instanceToNavigate, null);
        }

        #endregion
    }
}
