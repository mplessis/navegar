using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.UWP.Exemple.CRM.Controllers;
using Navegar.UWP.Exemple.CRM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace Navegar.UWP.Exemple.CRM
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Allows tracking page views, exceptions and other telemetry through the Microsoft Application Insights service.
        /// </summary>
        public static Microsoft.ApplicationInsights.TelemetryClient TelemetryClient;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            TelemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
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

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                //Initialisation et navigation vers la premiére page de l'application
                ServiceLocator.Current.GetInstance<INavigation>().InitializeRootFrame(rootFrame);

                //Permet d'exécuter une fonction avant chaque navigation, si la fonction n'est pas vérifiée Navegar déclenche l'événement NavigationCanceledOnPreviewNavigate,
                //il faut donc s'y abonner pour traiter l'erreur
                //remarque : pour tester cette pré-navigation et l'annulation, allez dans la fonction OpenClient du ViewModel ListClientsPageViewModel et décommentez la ligne "//UsersController.IsConnected = false;"
                ServiceLocator.Current.GetInstance<INavigation>().PreviewNavigate += PreviewNavigate;
                ServiceLocator.Current.GetInstance<INavigation>().NavigationCanceledOnPreviewNavigate += OnNavigationCanceledOnPreviewNavigate;

                //Si et seulement si il y a un bouton retour (physique ou virtuel) sur le device
                ServiceLocator.Current.GetInstance<INavigation>().BackButtonPressed += BackButtonPressed;
                ServiceLocator.Current.GetInstance<INavigation>().BackVirtualButtonPressed += BackVirtualButtonPressed;

                //Navigation vers la premiére page
                if (string.IsNullOrEmpty(ServiceLocator.Current.GetInstance<INavigation>().NavigateTo<LandingPageViewModel>(true)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

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
                ServiceLocator.Current.GetInstance<INavigation>().Clear();
                ServiceLocator.Current.GetInstance<INavigation>().NavigateTo<ListClientsPageViewModel>(true);
            }
        }

        /// <summary>
        /// Permet de surcharger le retour arriére, du bouton physique du device. 
        /// Fonction implémentée dans Navegar, cette surcharge n'est pas indispensable, elle vous permet simplement plus de liberté par rapport au métier de votre application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="backPressedEventArgs"></param>
        private void BackButtonPressed(object sender, BackPressedEventArgs backPressedEventArgs)
        {
            if (ServiceLocator.Current.GetInstance<INavigation>().CanGoBack())
            {
                GoBackNavigate();

                //A ajouter absolument à  partir du moment où l'on sait que l'on peut revenir en arriére
                //Sinon l'application se ferme
                backPressedEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Permet de surcharger le retour arriére, du bouton virtuel du device. 
        /// Fonction implémentée dans Navegar, cette surcharge n'est pas indispensable, elle vous permet simplement plus de liberté par rapport au métier de votre application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="backPressedEventArgs"></param>
        private void BackVirtualButtonPressed(object sender, Windows.UI.Core.BackRequestedEventArgs backRequestedEventArgs)
        {
            if (ServiceLocator.Current.GetInstance<INavigation>().CanGoBack())
            {
                GoBackNavigate();

                //A ajouter absolument à  partir du moment où l'on sait que l'on peut revenir en arriére
                //Sinon l'application se ferme
                backRequestedEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Se déclenche lorsque la pré-navigation a échoué car la fonction identifiée n'est pas satisfait aux condiditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNavigationCanceledOnPreviewNavigate(object sender, EventArgs args)
        {
            //On revient à l'écran de login
            ServiceLocator.Current.GetInstance<INavigation>().Clear();
            ServiceLocator.Current.GetInstance<INavigation>().NavigateTo<LandingPageViewModel>(true);
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

        /// <summary>
        /// Gére les traitements métiers du retour arriére
        /// </summary>
        private void GoBackNavigate()
        {
            //Lorsque l'on revient d'une commande on rafraichi la liste des commandes sur la page de liste
            if (ServiceLocator.Current.GetInstance<INavigation>().GetTypeViewModelToBack() ==
                typeof(ListCommandesPageViewModel))
            {
                //Permet de relancer la fonction LoadDatas aprés la navigation arriére vers la liste des commandes
                ServiceLocator.Current.GetInstance<INavigation>().GoBack("LoadDatas", new object[] { });
            }
            else
            {
                ServiceLocator.Current.GetInstance<INavigation>().GoBack();
            }
        }
    }
}
