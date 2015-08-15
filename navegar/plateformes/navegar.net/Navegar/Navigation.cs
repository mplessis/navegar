// <copyright file="Navigation.cs" company="Kopigi">
// Copyright � Kopigi 2013
// </copyright>
//  ****************************************************************************
// <author>Marc PLESSIS</author>
// <date>26/10/2013</date>
// <project>Navegar</project>
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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Class;
using Navegar.Libs.Exceptions;
using Navegar.Libs.Interfaces;

#endregion

namespace Navegar.Plateformes.Net.WPF
{
#if MEF
    using System.ComponentModel.Composition;
#endif

    /// <summary>
    /// Impl�mentation de la classe de navigation
    /// </summary>
    /// <example>
    ///   On r�alise la navigation suivante :
    /// 
    ///   MainViewModel -&gt; FirstViewModel &lt;-&gt; SecondViewModel
    ///  
    ///   <code>
    /// Dans le ViewModelLocator.cs :
    /// 
    ///    public ViewModelLocator()
    ///    {
    ///       ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
    ///
    ///       //1. Enregistrer la classe de navigation dans l'IOC
    ///       SimpleIoc.Default.Register&lt;INavigation, Navigation&gt;();
    ///
    ///       //2. G�n�rer le viewmodel principal, le type du viewmodel peut �tre n'importe lequel
    ///       SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GenerateMainViewModelInstance&lt;MainViewModel&gt;();
    ///    }
    ///
    ///    public MainViewModel Main
    ///    {
    ///        //3. Retrouve le viewmodel principal
    ///        get
    ///        {
    ///            return SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GetMainViewModelInstance&lt;MainViewModel&gt;();
    ///        }
    ///    }
    ///
    /// 
    /// 
    /// Dans MainViewModel.cs :
    /// 
    ///    //Pour aller vers un autre ViewModel
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().NavigateTo&lt;FirstViewModel&gt;();
    /// 
    /// 
    /// 
    /// Dans FirstViewModel.cs :
    /// 
    ///    //Pour aller vers SecondViewModel.cs, en supposant que le constructeur prenne un argument et que l'on veuille revenir vers FirstViewModel
    ///    SimpleIoc.Default.GetInstance&lt;INavigation&gt;().NavigateTo&lt;SecondViewModel&gt;(this, new object[] { Data }, true);
    /// 
    /// 
    /// 
    /// Dans SecondViewModel.cs :
    /// 
    ///    //Pour revenir vers FirstViewModel :
    ///    if(SimpleIoc.Default.GetInstance&lt;INavigation&gt;().CanGoBack())
    ///    {
    ///       SimpleIoc.Default.GetInstance&lt;INavigation&gt;().GoBack();
    ///    }
    ///   </code>
    /// </example>
#if MEF
    [Export(typeof(INavigation))]
#endif
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
        /// G�n�re l'instance du ViewModel principal
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
        /// R�cup�re l'instance du ViewModel principal
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
        public string NavigateTo(Type TTo, params object[] parametersViewModel)
        {
            return Navigate(TTo, null, parametersViewModel, null, null);
        }

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
        public string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, null, null);
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        public string NavigateTo(Type TTo, bool newInstance, params object[] parametersViewModel)
        {
            return Navigate(TTo, null, parametersViewModel, null, null, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel 
        /// </summary>
        /// <typeparam name="TTo">
        /// Type du Viewmodel vers lequel la navigation est effectu�e
        /// </typeparam>
        /// <param name="newInstance">
        /// Indique si l'on g�n�re une nouvelle instance obligatoirement
        /// </param>
        /// <param name="parametersViewModel">
        /// Tableau des param�tres �ventuels � transmettre au constructeur du ViewModel
        /// </param>
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        public string NavigateTo<TTo>(bool newInstance, params object[] parametersViewModel) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, null, null, newInstance);
        }

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
        public string NavigateTo(Type TTo, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            return Navigate(TTo, null, parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

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
        public string NavigateTo<TTo>(object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), null, parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel, avec un ViewModel en historique pr�c�dent
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
        public string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false)
        {
            return Navigate(TTo, currentInstance.GetType(), parametersViewModel, null, null, newInstance);
        }

        /// <summary>
        /// Naviguer vers un ViewModel, avec un ViewModel en historique pr�c�dent
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
        public string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), currentInstance.GetType(), parametersViewModel, null, null, newInstance);
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
        public string NavigateTo(Type TTo, ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            return Navigate(TTo, currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance);
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
        public string NavigateTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false) where TTo : class
        {
            return Navigate(typeof(TTo), currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance);
        }

        /// <summary>
        /// D�terminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        public bool CanGoBack()
        {
            return _currentViewModel != null && _historyInstances.ContainsKey(_currentViewModel);
        }

        /// <summary>
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
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

        /// <summary>
        /// Naviguer vers l'historique (ViewModel pr�c�dent) depuis le ViewModel en cours, si une navigation arri�re est possible
        /// </summary>
        /// /// <param name="functionToLoad">
        /// Permet de sp�cifier un nom de fonction � appeler apr�s le chargement du viewModel cibl�
        /// </param>
        /// <param name="parametersFunction">
        /// Param�tres pour la fonction appel�e
        /// </param>
        public void GoBack(string functionToLoad, params object[] parametersFunction)
        {
            if (CanGoBack())
            {
                if (_historyInstances.ContainsKey(_currentViewModel))
                {
                    Type viewModelFrom;
                    if(_historyInstances.TryGetValue(_currentViewModel, out viewModelFrom))
                    {
                        NavigateHistory(viewModelFrom, functionToLoad, parametersFunction);    
                    }
                }
            }
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
            if(_factoriesInstances.ContainsKey(_currentViewModel))
            {
                string key;
                if(_factoriesInstances.TryGetValue(_currentViewModel, out key))
                {
                    return (ViewModelBase) SimpleIoc.Default.GetInstance(typeof(ViewModelBase), key);
                }
            }
            return null;
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

                        //Gestion d'une fonction � appeler suite � la g�n�ration de l'instance
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
        /// <returns>
        /// Retourne la cl� unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        private string Navigate(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel, string functionToLoad, object[] parametersFunction, bool newInstance = false)
        {
            try
            {
                //V�rification du type de ViewModel demand� pour l'historique
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

                //G�n�ration d'une instance du viewmodel
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

                    //Gestion d'une fonction � appeler suite � la g�n�ration de l'instance
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
        /// Affecte la propri�t� de view courante du ViewModel principal
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
