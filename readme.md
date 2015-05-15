![Logo Navegar](http://www.kopigi.fr/navegar/navegar.png)

Navegar vous permet de gérer une navigation, au sein de vos applications Universal Apps en utilisant l'approche ViewModel First et non l'approche View First intégrée dans le Framework WinRT, le tout basé sur MVVMLight et SimpleIoc, vous pouvez donc passé d'une page à une autre simplement depuis vos ViewModels.

Navegar permet également, sur le même principe, une navigation inspirée des applications Windows 8 mais pour les applications WPF.

Il s'agit d'un ensemble de classes dont les binaires sont disponibles sur [Nuget](https://www.nuget.org/packages/Navegar/) et dont le code source est lui disponible sur cette plateforme. Un ensemble de documentation et de tutoriaux sont disponibles sur [navegar.kopigi.fr](http://navegar.kopigi.fr)

##Installation
Afin d'intégrer Navegar à votre application vous devez installer le package NuGet :

    PM> Install-Package Navegar 

##Nouveautés :

V4 Release Candidate :

- Ajout du support pour Windows 10 en Universal Window Platform
- Support de la présence d'un bouton de retour suivant les nouvelles capacités de détection du SDK Windows 10 : 
<code class="language-csharp">
Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")
</code>
- Solution pour Visual Studio 2015 RC avec le projet pour la librairie pour Windows 10 et l'application exemple pour Windows 10 (la solution VS 2013 est toujours présente avec le support pour Windows 8.1)

##Tutoriaux

Voici différents tutoriaux vous permettant de vous familiariser avec la librairie :

- [01. Installer et paramétrer Navegar](http://blog.kopigi.fr/index.php?article10/01-installer-et-parametrer-navegar)
- [02. Maintenant Navigons...] (http://blog.kopigi.fr/index.php?article14/02-maintenant-navigons)
- [03. Fonctionnalités avancées] (http://blog.kopigi.fr/index.php?article15/03-fonctionnalites-avancees)
 
##Documentation en ligne

Vous trouverez également la documentation (en ligne) du code, pour les plateformes WPF et Windows 8.1 & Windows Phone 8.1 (WinRT)

- [Plateforme WPF](http://www.kopigi.fr/navegar/documentation/wpf)
- [Plateforme Universal Application Plateform - Windows 8.1](http://www.kopigi.fr/navegar/documentation/uap.win81)
- [Plateforme Universal Application Plateform - Windows Phone 8.1](http://www.kopigi.fr/navegar/documentation/uap.wp81)

 
##Assistants Visual Studio

Enfin des assistants interactifs pour Visual Studio vous permettent également de suivre la mise en place de Navegar dans un projet

- [Navegar dans WPF](http://www.kopigi.fr/navegar/documentation/assistants/Navegar%20dans%20WPF.mvax)
- [Navegar dans Windows 8.1](http://www.kopigi.fr/navegar/documentation/assistants/Navegar%20dans%20Windows%208.1.mvax)
- [Navegar dans Windows Phone 8.1](http://www.kopigi.fr/navegar/documentation/assistants/Navegar%20dans%20Windows%20Phone%208.1.mvax)

Vous pouvez également retrouver le contenu de ces assistants dans le repertoire *doc* du projet ou ici :

- [Navegar dans WPF (en ligne)](https://github.com/mplessis/navegar/blob/master/doc/navegar_wpf.md)
- [Navegar dans Windows 8.1 (en ligne)](https://github.com/mplessis/navegar/blob/master/doc/navegar_windows_81.md)
- [Navegar dans Windows Phone 8.1 (en ligne)](https://github.com/mplessis/navegar/blob/master/doc/navegar_windows_phone_81.md)

Si vous rencontrez des soucis pour intégrer Navegar dans votre application, n'hésitez pas à revenir vers moi.
