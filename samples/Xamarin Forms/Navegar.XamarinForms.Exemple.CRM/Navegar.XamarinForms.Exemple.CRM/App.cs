using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonMobiles.Controllers;
using CommonMobiles.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Navegar.Libs.Class;
using Navegar.XamarinForms.Exemple.CRM.ViewModel;
using Navegar.XamarinForms.Exemple.CRM.Views;
using Xamarin.Forms;
using INavigation = Navegar.Libs.Interfaces.INavigation;

namespace Navegar.XamarinForms.Exemple.CRM
{
    public class App : Application
    {
        public static ViewModelLocator Locator { get; set; }

        public App()
        {
            //On génére en premier le ViewModelLocator pour pouvoir utiliser le ServiceLocator tout de suite aprés
            Locator = new ViewModelLocator();

            //Définition de la page de demarrage de l'application
            MainPage = (NavigationPage)ServiceLocator.Current.GetInstance<INavigation>().InitializeRootFrame<LandingPageViewModel, LandingPage>();
            MainPage.BackgroundColor = Color.FromHex("407aae");

            //Evenements de navigation
            ServiceLocator.Current.GetInstance<INavigation>().PreviewNavigate += PreviewNavigate;
            ServiceLocator.Current.GetInstance<INavigation>().NavigationCanceledOnPreviewNavigate +=
                OnNavigationCanceledOnPreviewNavigate;
        }

        private bool PreviewNavigate(ViewModelBase currentViewModelInstance, Type currentViewModel, Type viewModelToNavigate, out PreNavigationArgs preNavigationArgs)
        {
            //Exemple
            /*if (viewModelToNavigate == typeof(MonViewModel))
            {
                preNavigationArgs = new PreNavigationArgs { FunctionToLoad = "NewLoadFunction", ParametersFunctionToLoad = new object[] { } };
                return true;
            }
            preNavigationArgs = null;
            return true;*/

            if (viewModelToNavigate != typeof(LandingPageViewModel))
            {
                preNavigationArgs = null;
                return UsersController.IsConnected;
            }
            preNavigationArgs = null;
            return true;
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

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
