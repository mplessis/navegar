using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Navegar.XamarinForms.Exemple.CRM.ViewModel
{
    public class ListCommandesPageViewModel : ViewModelBase
    {
        private string _maintText2;

        public string MainText2
        {
            get
            {
                return _maintText2;
            }
            set
            {
                _maintText2 = value;
                RaisePropertyChanged(() => MainText2);
            }
        }

        public void OnLoad()
        {
            MainText2 = "new text";
        }
    }
}
