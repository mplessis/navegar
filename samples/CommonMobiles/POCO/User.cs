namespace CommonMobiles.POCO
{
    public class User : PocoBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }
    }
}
