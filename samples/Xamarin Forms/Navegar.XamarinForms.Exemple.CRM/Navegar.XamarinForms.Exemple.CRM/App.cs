using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Navegar.XamarinForms.Exemple.CRM.ViewModel;
using Navegar.XamarinForms.Exemple.CRM.Views;
using Xamarin.Forms;

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
            MainPage = ServiceLocator.Current.GetInstance<INavigation>().InitializeRootFrame<LandingPageViewModel, LandingPage>();
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
