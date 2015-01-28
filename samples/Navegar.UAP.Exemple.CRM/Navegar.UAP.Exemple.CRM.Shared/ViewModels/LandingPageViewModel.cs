using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Command;
using Navegar.UAP.Exemple.CRM.Controllers;
using Navegar.UAP.Exemple.CRM.POCO;

namespace Navegar.UAP.Exemple.CRM.ViewModels
{
    public class LandingPageViewModel : ViewModelServices
    {
        #region properties

        private User _userCurrent;
        public User UserCurrent
        {
            get { return _userCurrent; }
            set
            {
                _userCurrent = value;
                RaisePropertyChanged(() => UserCurrent);
            }
        }

        private List<User> _users;
        public List<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged(() => Users);
            }
        }
        public RelayCommand ConnectCommand { get; set; }

        #endregion

        #region cstor

        public LandingPageViewModel()
        {
            Users = UsersController.Initialize();
            UserCurrent = Users.First();
            ConnectCommand = new RelayCommand(Connect, CanConnect);
        }
        #endregion

        #region relaycommand

        private bool CanConnect()
        {
            return UserCurrent != null;
        }

        private async void Connect()
        {
            var users = Users.Where(u => u.UserName == UserCurrent.UserName);
            if (Users.Any())
            {
                if (UsersController.Initialize().First().Password == UserCurrent.Password)
                {
                    UsersController.IsConnected = true;

                    //Permet de naviguer vers la page ListClientsPage
                    //true indique que l'on souhaite lancer une nouvelle instance du ViewModel
                    NavigationService.NavigateTo<ListClientsPageViewModel>(true);
                }
                else
                {
                    var message = new MessageDialog("Mot de passe incorrect", "Erreur");
                    await message.ShowAsync();
                }
            }
            else
            {
                var message = new MessageDialog("Utilisateur inconnu", "Erreur");
                await message.ShowAsync();
            }
        }
        #endregion
    }
}
