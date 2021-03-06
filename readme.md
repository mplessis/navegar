![Logo Navegar](https://pro2-bar-s3-cdn-cf3.myportfolio.com/174d98845cfd76e745f7db1b61e2da99/ef7f3e34-1cea-4eac-b82d-75882fffdd12_rw_600.png)

Navegar vous permet de gérer une navigation, au sein de vos applications Windows Presentation Foundation et Universal Windows Platform en utilisant l'approche ViewModel First et non l'approche View First intégrée dans le Framework WinRT, le tout basé sur MVVMLight et SimpleIoc, vous pouvez donc passer d'une page à une autre simplement depuis vos ViewModels.

Navegar permet également, sur le même principe, une navigation inspirée des applications Windows 8 mais pour les applications WPF et UWP.

Il s'agit d'un ensemble de classes dont les binaires sont disponibles sur [Nuget](https://www.nuget.org/packages/Navegar/) et dont le code source est lui disponible sur cette plateforme.

## Exemple de navigation
Voici un petit exemple de la syntaxe pour naviguer vers une nouvelle page

    /// <summary>
    /// Permet de naviguer la page de gestion du client
    /// </summary>
    private void OpenClient(Client client)
    {
        //Navigation vers la page ClientPage
        //this sert à indiquer que le ViewModel actuel (et donc par extension la page) sera ajouté à l'historique de navigation, afin que Navegar puisse savoir qu'il doit revenir vers cette page au Back
        //new object[]{client} permet de passer l'objet client au constructeur du ViewModel ClientPageViewModel
        //true indique que l'on souhaite générer une nouvelle instance du ViewModel ClientPageViewModel
        
        ServiceLocator.Current.GetInstance<INavigation>.NavigateTo<ClientPageViewModel>(this, new object[] {client}, true);
    }

## Installation
Afin d'intégrer Navegar à votre application vous devez installer le package NuGet :

    PM> Install-Package Navegar 

## Nouveautés :

V6.0:

- Suppression de la plateforme WPF full framework spécifique, elle est remplacée par une version Standard 2.0
- La librairie supporte désormais le .Net Full  Framework v4.7.2, .Net Core et UWP en version minimale 1809 (build 17763)
  
V5.0 :

- Suppression du support pour Xamarin.Forms, dû à un non support de Android Support par MvvmLight
- La librairie ne supporte désormais que le .Net Full  Framework v4.7.2 et UWP en version minimale 1809 (build 17763)

V4.5.9 :

- Plateformes UAP/UWP/Xamarin.Forms : ajout d'un argument PreNavigationArgs à l'événement PreviewNavigate afin de pouvoir modifier la fonction à charger au cours d'une navigation, pendant le déclenchement de la pré-navigation

V4.5.8.8 :

- Correction sur la fonction ShowVirtualBackButton

V4.5.8.6 :

- Corrections sur la fonction GetViewModelCurrent

V4.5.7:

- Windows 10 : Gestion de l'affichage du bouton virtuel (dans la barre de titre de l'application) par un paramétre de la fonction RegisterView
- Windows 10 : Gestion du passage en mode Tablette/Desktop automatiquement pour le bouton de retour virtuel

V4.5.6 :

- Suppport de Xamarin.Forms
- Modification de la structure du projet, désormais une DLL NavegarLibs contient les interfaces INavigationWpf et INavigation (pour les UAP/UWP et Xamarin.Forms), ainsi que les exceptions. Chaque plateforme (WPF/UAP/UWP/Xamarin.Forms) est gérée dans une DLL à part installée à partir du package Navegar (ce package étant dépendant du package NavegarLibs il l'installe en même temps). 
Ceci afin de permettre de réaliser une DLL Portable pour partager le code métier de votre application entre les UAP/UWP et Xamarin.Forms et avoir une navigation unique dans vos ViewModels. Vous pouvez donc installer avec cette DLL portable le package NavegarLibs pour bénéficier des interfaces et des exceptions et installer la dll de la plateforme sur chaque projet.
- Version Windows 10 : Replacement de l'événement BackVirtualButtonPressed (pour personnaliser le retour arrière dans l'application) par l'événement BackButtonPressed qui gére les boutons physiques (ou virtuels sur Windows Phone) et virtuels dans la barre de titre des applications UWP Windows 10
- Ajout d'une propriété CurrentPlatform indiquant la plateforme courante (WPF/UAP/UWP ou Xamarin)

V4.0.3 Release Candidate :

- Version Windows 10 : gestion du bouton de retour virtuel dans la barre de titre des applications UWP en version desktop. Avec la fonction ShowVirtualBackButton (affichage du bouton) et l'événement BackVirtualButtonPressed (pour personnaliser le retour arrière dans l'application)
- Version Windows 10 : ajout d'une propriété HasVirtualBackButtonShow indiquant si le bouton de retour virtuel est actuellement affiché

V4.0.2 Release Candidate :

- Version Windows 10 : ajout d'une propriété HasBackButton indiquant si le device posséde un bouton de retour

V4 Release Candidate :

- Ajout du support pour Windows 10 en Universal Window Platform
- Support de la présence d'un bouton de retour suivant les nouvelles capacités de détection du SDK Windows 10 :

<code class="language-csharp">
Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")
</code>

- Solution pour Visual Studio 2015 RC avec le projet pour la librairie pour Windows 10 et l'application exemple pour Windows 10 (la solution VS 2013 est toujours présente avec le support pour Windows 8.1)
