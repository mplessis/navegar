using System.Collections.Generic;
using System.Linq;
using CommonMobiles.Controllers;
using CommonMobiles.Messages;
using CommonMobiles.POCO;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CommonMobiles.ViewModels
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
                    Messenger.Default.Send<MessageLogin>(new MessageLogin() { Message = "Mot de passe incorrect" });
                }
            }
            else
            {
                Messenger.Default.Send<MessageLogin>(new MessageLogin() { Message = "Utilisateur inconnu" });
            }
        }
        #endregion
    }
}
