using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Enums;
using Navegar.Libs.Exceptions;
using Navegar.Libs.Interfaces;

namespace Navegar.Libs.Class
{
    public abstract class NavigationBase : INavigation
    {
        #region fields

        protected readonly Dictionary<Type, string> FactoriesInstances = new Dictionary<Type, string>();
        protected readonly Dictionary<Type, Type> HistoryInstances = new Dictionary<Type, Type>();
        protected Type CurrentViewModel;
        protected string NavigationStateInitial;
        protected readonly Dictionary<Type, string> HistoryNavigation = new Dictionary<Type, string>();
        protected readonly Dictionary<Type, Type> ViewsRegister = new Dictionary<Type, Type>();

        #endregion

        /// <summary>
        /// Permet d'indiquer que la navigation est annulée
        /// </summary>
        public event EventHandler NavigationCanceledOnPreviewNavigate;

        /// <summary>
        /// Permet d'exécuter une action avant la navigation
        /// </summary>
        public PreNavigateDelegate<ViewModelBase> PreviewNavigate { get; set; }

        /// <summary>
        /// Déterminer si un historique est possible depuis le ViewModel en cours
        /// </summary>
        /// <returns>
        /// <c>true</c> si la navigation est possible, sinon <c>false</c>
        /// </returns>
        public bool CanGoBack()
        {
            return CurrentViewModel != null && HistoryInstances.ContainsKey(CurrentViewModel) && CanGoBackFrame();
        }

        /// <summary>
        /// Déclenche l'événement d'annulation de navigation
        /// </summary>
        public void CancelNavigation()
        {
            OnNavigationCancel();
        }

        /// <summary>
        /// Permet de vider l'historique de navigation
        /// </summary>
        public void Clear()
        {
            ClearNavigation();
        }

        /// <summary>
        /// Permet de connaitre le type du ViewModel au niveau n-1 de l'historique de navigation
        /// </summary>
        /// <returns>Type du ViewModel</returns>
        public Type GetTypeViewModelToBack()
        {
            if (CanGoBack())
            {
                if (HistoryInstances.ContainsKey(CurrentViewModel) && CanGoBackFrame())
                {
                    Type viewModelFrom;
                    if (HistoryInstances.TryGetValue(CurrentViewModel, out viewModelFrom))
                    {
                        return viewModelFrom;
                    }
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
            if (FactoriesInstances.ContainsKey(typeof(T)))
            {
                string key;
                var result = FactoriesInstances.TryGetValue(typeof(T), out key);
                if (result)
                {
                    return SimpleIoc.Default.GetInstance<T>(key);
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
            if (FactoriesInstances.ContainsKey(CurrentViewModel))
            {
                string key;
                if (FactoriesInstances.TryGetValue(CurrentViewModel, out key))
                {
                    return (ViewModelBase)SimpleIoc.Default.GetInstance(CurrentViewModel, key);
                }
            }
            return null;
        }

        /// <summary>
        /// Naviguer vers l'historique (ViewModel précédent) depuis le ViewModel en cours, si une navigation arriére est possible
        /// </summary>
        public void GoBack()
        {
            if (CanGoBack())
            {
                if (HistoryInstances.ContainsKey(CurrentViewModel) && CanGoBackFrame())
                {
                    Type viewModelFrom;
                    if (HistoryInstances.TryGetValue(CurrentViewModel, out viewModelFrom))
                    {
                        Navigate(viewModelFrom, null, null);
                    }
                }
            }
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
                if (HistoryInstances.ContainsKey(CurrentViewModel) && CanGoBackFrame())
                {
                    Type viewModelFrom;
                    if (HistoryInstances.TryGetValue(CurrentViewModel, out viewModelFrom))
                    {
                        Navigate(viewModelFrom, functionToLoad, parametersFunction);
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
        public string NavigateTo<TTo>(params object[] parametersViewModel) where TTo : class
        {
            return Navigate<TTo>(typeof(TTo), null, parametersViewModel, null, null, false);
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
            return Navigate<TTo>(typeof(TTo), null, parametersViewModel, null, null, newInstance);
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
            return Navigate<TTo>(typeof(TTo), null, parametersViewModel, functionToLoad, parametersFunction, newInstance);
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
            return Navigate<TTo>(typeof(TTo), currentInstance.GetType(), parametersViewModel, null, null, newInstance);
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
            return Navigate<TTo>(typeof(TTo), currentInstance.GetType(), parametersViewModel, functionToLoad, parametersFunction, newInstance);
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
        /// <param name="isModal">
        /// Permet d'appeler la fenetre cible de la navigation en mode modal, ceci n'est supporté que sur la plateforme Xamarin.Forms
        /// </param>
        /// <returns>
        /// Retourne la clé unique pour SimpleIoc, de l'instance du viewmodel vers lequel la navigation a eu lieu
        /// </returns>
        /// <remarks>
        /// Supportée uniquement sur la plateforme Xamarin.Forms, dans les autres cas une exception <see cref="NotImplementedForCurrentPlatformException"/> sera levée
        /// </remarks>
        public abstract string NavigateModalTo<TTo>(ViewModelBase currentInstance, object[] parametersViewModel,
            string functionToLoad,
            object[] parametersFunction, bool newInstance = false)
            where TTo : class;

        #region protected

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
        protected abstract void Navigate(Type viewModelToName, string functionToLoad, object[] parametersFunction);

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
        protected abstract string Navigate<TTo>(Type viewModelToName, Type viewModelFromName, object[] parametersViewModel,
            string functionToLoad, object[] parametersFunction, bool newInstance = false, bool isModal = false) where TTo : class;

        /// <summary>
        /// Permet de gérer l'état de navigation à l'instant T pour un ViewModel
        /// </summary>
        /// <param name="viewModelFromName">ViewModel pris en compte</param>
        /// <param name="isModal">Indique que la navigation est de type modale, supportée uniquement sur la plateforme Xamarin.Forms</param>
        protected abstract void SetNavigationHistory(Type viewModelFromName, bool isModal = false);

        /// <summary>
        /// Permet de savoir si l'on peut revenir en arriere au niveau des Frame
        /// </summary>
        /// <returns>Résultat de la demande</returns>
        protected abstract bool CanGoBackFrame();

        /// <summary>
        /// Vide l'historique de navigation de la classe et de la Frame de WinRT
        /// </summary>
        protected abstract void ClearNavigation();

        /// <summary>
        /// Génére l'événement d'annulation de la navigation
        /// </summary>
        protected void OnNavigationCancel()
        {
            if (NavigationCanceledOnPreviewNavigate != null)
            {
                NavigationCanceledOnPreviewNavigate(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gére le déclenchement éventuel de l'événement de PreNavigation pour la fonction Navigate&lt;TTo&gt;
        /// </summary>
        /// <param name="viewModelFromName">ViewModel d'où l'on vient</param>
        /// <param name="viewModelToName">ViewModel vers lequel on va</param>
        /// <returns>True continue la navigation, False déclenche l'annulation de la navigation</returns>
        protected bool PreNavigateTo(Type viewModelFromName, Type viewModelToName)
        {
            if (PreviewNavigate != null)
            {
                ViewModelBase currentInstance = null;
                if (viewModelFromName != null)
                {
                    if (FactoriesInstances.ContainsKey(viewModelToName))
                    {
                        string keyInstance;
                        if (FactoriesInstances.TryGetValue(viewModelToName, out keyInstance))
                        {
                            currentInstance = (ViewModelBase)SimpleIoc.Default.GetInstance(viewModelToName, keyInstance);
                        }
                    }
                }

                if (!PreviewNavigate(currentInstance, viewModelFromName, viewModelToName))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gére l'historisation de la navigation pour la fonction Navigate&lt;TTo&gt;
        /// </summary>
        /// <param name="viewModelFromName">ViewModel de départ</param>
        /// <param name="viewModelToName">ViewModel de destination</param>
        /// <param name="isModal">Indique si la navigation est modale, supportée uniquement par la plateforme Xamarin.Forms</param>
        protected void HistoryNavigateTo(Type viewModelFromName, Type viewModelToName, bool isModal = false)
        {
            if (viewModelFromName != null)
            {
                if (HistoryInstances.ContainsKey(viewModelToName))
                {
                    HistoryInstances[viewModelToName] = viewModelFromName;
                }
                else
                {
                    HistoryInstances.Add(viewModelToName, viewModelFromName);
                }

                //Gestion de l'historique de navigation
                SetNavigationHistory(viewModelFromName, isModal);
            }
        }

        /// <summary>
        /// Permet de générer une nouvelle instance du ViewModel vers leque on va
        /// </summary>
        /// <typeparam name="TTo">Type du ViewModel de destination</typeparam>
        /// <param name="viewModelToName">ViewModel de destination</param>
        /// <param name="parametersViewModel">Paramètres du ViewModel</param>
        /// <param name="newInstance">Indique si l'on souhaite une nouvelle instance</param>
        /// <returns>La clé identifiant l'instane dans l'IOC</returns>
        protected string GenerateNewInstanceViewModelNavigateTo<TTo>(Type viewModelToName, object[] parametersViewModel, bool newInstance = false) where TTo : class
        {
            string key;
            if (FactoriesInstances.ContainsKey(viewModelToName) && !newInstance)
            {
                FactoriesInstances.TryGetValue(viewModelToName, out key);
            }
            else
            {
                if (FactoriesInstances.ContainsKey(viewModelToName))
                {
                    //Suppression de l'instance du viewModel dans le cache de SimpleIOC
                    FactoriesInstances.TryGetValue(viewModelToName, out key);

                    if (key != null)
                    {
                        FactoriesInstances.Remove(viewModelToName);
                        SimpleIoc.Default.Unregister<TTo>(key);
                    }
                }

                var instanceNew = Activator.CreateInstance(viewModelToName, parametersViewModel);
                key = Guid.NewGuid().ToString();
                SimpleIoc.Default.Register<TTo>(() => (TTo)instanceNew, key);
                FactoriesInstances.Add(viewModelToName, key);
            }

            return key;
        }

        /// <summary>
        /// Permet de lancer une fonction aprés l'appel au ViewModel
        /// </summary>
        /// <typeparam name="TTo">Type du ViewModel de destination</typeparam>
        /// <param name="instance">Instance courante du ViewModel de destination</param>
        /// <param name="functionToLoad">Fonction à charger</param>
        /// <param name="parametersFunction">Paramètres de la fonction</param>
        protected void LoadFunctionViewModelNavigateTo<TTo>(ViewModelBase instance, string functionToLoad, object[] parametersFunction) where TTo : class
        {
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
        }

        #endregion

        #region Not Implemented

        public abstract Func<bool> BackButtonPressed { get; set; }

        public abstract BackButtonTypeEnum HasBackButton { get; }

        public abstract void InitializeRootFrame(object rootFrame);

        public abstract object InitializeRootFrame<TViewModelFirst, TViewFirst>() where TViewModelFirst : ViewModelBase;

        public abstract void Dispose();

        public abstract void RegisterBackPressedAction<TViewModel>(Action func) where TViewModel : ViewModelBase;

        public abstract void RegisterView<TViewModel, TView>() where TViewModel : ViewModelBase where TView : class;

        public abstract void ShowVirtualBackButton(bool visible = true, bool force = false);

        #endregion
    }
}
