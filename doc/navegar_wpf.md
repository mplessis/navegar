Intégrer Navegar à une solution WPF

Bonjour, bienvenue dans ce tutoriel vous permettant d'intégrer Navegar dans une solution WPF. Nous allons voir l'installation du package, l'initialisation et la navigation vers un premier ViewModel
I. Création d'une solution Windows 8.1 et installation de Navegar

Commonçons par créer une solution de type WPF
Pour cela, il vous suffit de faire

Fichier ? Nouveau ? Projet

ou cliquez ici : Créer une nouvelle solution

Une fois votre solution créée, nous allons procéder à l'installation de Navegar, pour cela vous pouvez utiliser le gestionnaire de paquet de NuGet en mode graphique ou bien en ligne de commande
install-package Navegar Copier

Ceci installera également MvvmLightLibs

Vous devez également installer MvvmLight complet pour le bon fonctionnement de l'application, Navegar ne l'installe pas car il est possible d'utiliser Navegar dans un projet bibliothéque qui n'a pas besoin de tout MvvmLight
II. Initialisation de la navigation

La navigation en WPF fonctionne sur le principe d'une page principale qui va accueillir des UserControl qui seront vos différentes pages. Navegar ne gére pas de navigation multi-fenêtres.

Pour que cela fonctionne il faut modifier le fichier ViewModelLocator de MvvmLight pour enregistrer la classe de navigation dans l'IOC de MvvmLight et enregistrer l'instance de la fenêtre principale (hôte)

Modifiez le constructeur de la classe ViewModelLocator et ajoutez :
//1. Enregistrer la classe de navigation dans l'IOC
SimpleIoc.Default.Register<INavigation, Navigation>();

//2. Générer le viewmodel principal, le type du viewmodel
//peut être n'importe lequel
//Cette génération va permettre de créer au sein de la
//navigation, une instance unique pour le viewmodel principal,
//qui sera utilisée par la classe de navigation
SimpleIoc.Default.GetInstance<INavigation>()
.GenerateMainViewModelInstance<MainViewModel>(); Copier

Supprimez la ligne :
SimpleIoc.Default.Register<MainViewModel>();

Afin d'affecter un DataContext à la page principale vous devez enregistrer une propriété dans la classe ViewModelLocator. Ajoutez le code dans la classe (en remplacement de la propriété déja définie par MvvmLight) :
public YourFirstViewModel Main
{
 //3. Retrouve le viewmodel principal
 get
 {
  return SimpleIoc.Default.GetInstance<INavigation>().GetMainViewModelInstance<MainViewModel>();
 }
} Copier

Normalement MvvmLight a déja affecté à la page principale la propriété, si ce n'est pas le cas affectez cette propriété comme DataContext de la page
<Window.DataContext>
 <Binding Path="Main" Source="{StaticResource Locator}"/>
</Window.DataContext> Copier

La navigation est maintenant initialisée.
III. Préparation de la page Main et du MainViewModel pour gérer la navigation en Usercontrol

Ajouter un USerControl et un ViewModel associé, nommez les FirstView et FirstViewModel

Afin d'avoir une navigation par UserControl vous devez modifier votre Main.xaml et votre MainViewModel.cs en utilisant le code suivant.

MainViewModel.cs
public class MainViewModel : ViewModelBase
{
private readonly DispatcherTimer _timerLoadAccueil;

/// <summary>
/// Permet de contrôler la propriété CurrentView
/// </summary>
private ViewModelBase _currentView;

/// <summary>
/// L'attribut CurrentViewNavigation permet de définir automatiquement, quelle propriété du viewmodel
/// devra être utilisé pour charger le viewmodel vers lequel la navigation va s'effectuer
/// </summary>
[CurrentViewNavigation]
public ViewModelBase CurrentView
{
get { return _currentView; }
set
{
_currentView = value;
RaisePropertyChanged("CurrentView");
}
}

public MainViewModel()
{ //Permet d'éviter l'appel à la navigation pendant le chargement du viewmodel principal. Sans ceci le chargement du MainViewModel
//ne se passe pas correctement puisque le chargement du viewmodel n'est pas possible
_timerLoadAccueil = new DispatcherTimer
{
Interval = new TimeSpan(0, 0, 0, 0, 500)
};
_timerLoadAccueil.Tick += LoadAccueil;
_timerLoadAccueil.Start();
}

/// <summary>
/// Navigation vers le premier viewmodel
/// </summary>
///
///
private void LoadAccueil(object sender, EventArgs e)
{
_timerLoadAccueil.Stop();
SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<FirstViewModel>();
}
}
Copier

Main.xaml
<Grid> <ContentControl Content="{Binding CurrentView}" Focusable="False" Grid.Row="1"/> </Grid> Copier

La navigation utilisera la propriété CurrentView afin d'intégrer la page associée au ViewModel vers lequel la navigation sera dirigé. Un ContentControl doit donc être placé dans la page afin d'afficher le UserControl.

 

Pour associer le ViewModel d'un UserControl au UserControl vous devez ajouter un DataTemplate, ainsi lorsque Navegar fera l'instanciation du ViewModel le UserControl lui sera associé.
<DataTemplate DataType="{x:Type ViewModel:YourFirstViewModel}">
 <View:YourFirstView />
</DataTemplate> Copier
IV. Premiére navigation vers une seconde page

Ajoutez un second UserControl avec son ViewModel associé, nommez les SecondViewModelet SecondView. Ajoutez un DataTemplate pour associer les deux

Ajoutez un bouton à votre premier ViewModel afin de lancer la navigation vers le second. Pour cela créez une ICommand et affectez le code suivant à votre ICommand :
SimpleIoc.Default.GetInstance<INavigation>().NavigateTo<SecondViewModel>(this, new object[] {}, true); Copier

Afin de revenir vers le premier ViewModel il vous faut ajouter un bouton qui vous permettra d'affecter une ICommand. Dans cette ICommand affectez le code suivant
if (SimpleIoc.Default.GetInstance<INavigation>().CanGoBack())
{
 SimpleIoc.Default.GetInstance<INavigation>().GoBack();
} Copier

Voila votre navigation est maintenant en place de façon simple. Pour plus de détails sur les fonctionnalités de la fonction NavigateTo et GoBack vous pouvez consulter la documentation de code
