using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using GalaSoft.MvvmLight.Views;
using Navegar.Libs.Interfaces;
using Navegar.UAP.Exemple.CRM.Controllers;
using Navegar.UAP.Exemple.CRM.ViewModels;

namespace Navegar.UAP.Exemple.CRM
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif
                //Initialisation et navigation vers la premiére page de l'application
                SimpleIoc.Default.GetInstance<INavigation>().InitializeRootFrame(rootFrame);

                //Permet d'exécuter une fonction avant chaque navigation, si la fonction n'est pas vérifiée Navegar déclenche l'événement NavigationCanceledOnPreviewNavigate,
                //il faut donc s'y abonner pour traiter l'erreur
                //remarque : pour tester cette pré-navigation et l'annulation, allez dans la fonction OpenClient du ViewModel ListClientsPageViewModel et décommentez la ligne "//UsersController.IsConnected = false;"
                SimpleIoc.Default.GetInstance<INavigation>().PreviewNavigate += PreviewNavigate;
                SimpleIoc.Default.GetInstance<INavigation>().NavigationCanceledOnPreviewNavigate += OnNavigationCanceledOnPreviewNavigate;

#if WINDOWS_PHONE_APP
                SimpleIoc.Default.GetInstance<INavigation>().BackButtonPressed += BackButtonPressed;
#endif

                //Navigation vers la premiére page
                if (string.IsNullOrEmpty(SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<LandingPageViewModel>(true)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP

        /// <summary>
        /// Permet de surcharger le retour arriére, du bouton physique ou virtuel du téléphone. 
        /// Fonction implémentée dans Navegar, cette surcharge n'est pas indispensable, elle vous permet simplement plus de liberté par rapport au métier de votre application
        /// </summary>
        private bool BackButtonPressed()
        {
            if (SimpleIoc.Default.GetInstance<INavigation>().CanGoBack())
            {
                //Lorsque l'on revient d'une commande on rafraichi la liste des commandes sur la page de liste
                if (SimpleIoc.Default.GetInstance<INavigation>().GetTypeViewModelToBack() ==
                    typeof (ListCommandesPageViewModel))
                {
                    //Permet de relancer la fonction LoadDatas aprés la navigation arriére vers la liste des commandes
                    SimpleIoc.Default.GetInstance<INavigation>().GoBack("LoadDatas", new object[] {});
                }
                else
                {
                    SimpleIoc.Default.GetInstance<INavigation>().GoBack();
                }
                //A ajouter absolument à  partir du moment où l'on sait que l'on peut revenir en arriére
                //Sinon l'application se ferme
                return true;
            }
            return false;
        }
#endif

        /// <summary>
        /// Se déclenche lorsque la pré-navigation a échoué car la fonction identifiée n'est pas satisfait aux condiditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNavigationCanceledOnPreviewNavigate(object sender, EventArgs args)
        {
            //On revient à l'écran de login
            SimpleIoc.Default.GetInstance<INavigation>().Clear();
            SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<LandingPageViewModel>(true);
        }

        /// <summary>
        /// Permet d'effectuer une action avant chaque navigation
        /// </summary>
        /// <param name="currentViewModelInstance">Instance du ViewModel courant</param>
        /// <param name="currentViewModel">Type du ViewModel courant</param>
        /// <param name="viewModelToNavigate">Type du ViewModel vers lequel la navigation est dirigée</param>
        /// <returns>Un booléen indiquant si la navigation doit être poursuivi ou non</returns>
        private bool PreviewNavigate(ViewModelBase currentViewModelInstance, Type currentViewModel, Type viewModelToNavigate)
        {
            if (viewModelToNavigate != typeof(LandingPageViewModel))
            {
                return UsersController.IsConnected;    
            }
            return true;
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Permet de gérer la sortie de l'état suspendu de l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResuming(object sender, object e)
        {
            if (SimpleIoc.Default.GetInstance<INavigation>().GetViewModelCurrent().GetType() != typeof(ListClientsPageViewModel))
            {
                //On supprime l'historique de navigation et on revient vers la page de la liste des clients (pour l'exemple)
                SimpleIoc.Default.GetInstance<INavigation>().Clear();
                SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<ListClientsPageViewModel>(true);
            }
        }
    }
}