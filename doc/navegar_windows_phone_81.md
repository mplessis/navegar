##Principe de fonctionnement sur Windows Phone 8.1 (XAML)

Bonjour, bienvenue dans ce tutoriel vous permettant d'intégrer Navegar dans une solution Windows Phone 8.1. Nous allons voir l'installation du package, l'initialisation et la navigation vers un premier ViewModel
I. Création d'une solution Windows Phone 8.1 et installation de Navegar

Commonçons par créer une solution de type Windows Phone 8.1
Pour cela, il vous suffit de faire

Fichier ? Nouveau ? Projet

Une fois votre solution créée, nous allons procéder à l'installation de Navegar, pour cela vous pouvez utiliser le gestionnaire de paquet de NuGet en mode graphique ou bien en ligne de commande
install-package Navegar

Ceci installera également MvvmLightLibs

Vous devez également installer MvvmLight complet pour le bon fonctionnement de l'application, Navegar ne l'installe pas car il est possible d'utiliser Navegar dans un projet bibliothéque qui n'a pas besoin de tout MvvmLight
II. Initialisation de la navigation

Afin d'initialiser la navigation au sein de l'application vous devez modifier le fichier App.xaml.cs pour naviguer sur votre premier ViewModel (premiére page)

Modifiez la fonction OnLaunched

En remplacement des lignes
if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
{
 throw new Exception("Failed to create initial page");
}

Ajoutez :
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
 ...

 if (rootFrame.Content == null)
 {
  ...

  //Initialisation de la navigation et navigation vers le premier ViewModel
  SimpleIoc.Default.GetInstance<INavigation>().InitializeRootFrame(rootFrame);

  //Surcharge éventuelle de la prise en compte du bouton physique ou virtuel de retour
  //Par défaut Navegar par en charge ce bouton
  //SimpleIoc.Default.GetInstance<INavigation>().BackButtonPressed += VotreFonctionDeRetour;

  if (string.IsNullOrEmpty(SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<YourFirstViewModel>(true)))
  {
   throw new Exception("Impossible de créer la page d'accueil");
  }
 }
}

Pour que cela fonctionne il faut modifier le fichier ViewModelLocator de MvvmLight pour enregistrer la classe de navigation ainsi que vos ViewModels et leur page associée dans l'IOC de MvvmLight.
L'enregistrement des ViewModel et des pages devra être fait à chaque création d'une page.

Modifiez le constructeur de la classe ViewModelLocator et ajoutez :
//Enregistrer la classe de navigation dans l'IOC et les ViewModels
if (!SimpleIoc.Default.IsRegistered<INavigation>())
{
 SimpleIoc.Default.Register<INavigation, Navigation>();
}

//Enregistrer le ViewModel avec sa page associée
SimpleIoc.Default.GetInstance<INavigation>().RegisterView<YourFirstViewModel, YourFirstPage>();

Supprimez la ligne :
SimpleIoc.Default.Register<YourFirstViewModel>();

Pas d'inquiétude le viewmodel YourFirstViewModel sera enregistré au premier appel vers celui-çi dans la classe de navigation

Afin d'affecter un DataContext à la page associée vous devez enregistrer une propriété dans la classe ViewModelLocator. Ajoutez le code alors le code suivant dans la classe (en remplacement de la propriété déja définie par MvvmLight) :
public YourFirstViewModel YourFirstViewModelInstance
{
 get { return SimpleIoc.Default.GetInstance<INavigation>().GetViewModelInstance<YourFirstViewModel>(); }
}

Il faut maintenant affecter cette propriété comme DataContext de la page
<Page.DataContext>
 <Binding Path="YourFirstViewModelInstance" Source="{StaticResource Locator}"/>
</Page.DataContext>

La navigation est maintenant initialisée et votre premier ViewModel et premiére Page sont prêts à être utilisés
III. Premiére navigation vers une seconde page

Ajoutez une seconde page avec son ViewModel associée, nommez les SecondViewModelPage et SecondPage

Créer une propriété d'instance comme pour le premier ViewModel et affectez cette propriété en tant que DataContext de la page

Ajoutez un bouton à votre premier ViewModel afin de lancer la navigation vers la seconde page. Pour cela créez une ICommand et affectez le code suivant à votre ICommand :
SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<SecondViewModelPage>(this, new object[] {}, true);

Voila votre navigation est maintenant en place de façon simple. Pour plus de détails sur les fonctionnalités de la fonction NavigateTo vous pouvez consulter la documentation de code
