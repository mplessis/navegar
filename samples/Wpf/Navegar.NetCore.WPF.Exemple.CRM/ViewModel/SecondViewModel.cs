﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Navegar.Libs.Interfaces;
using Navegar.NetCore.WPF.Exemple.Class;

namespace Navegar.NetCore.WPF.Exemple.ViewModel
{
    public class SecondViewModel : ViewModelBase
    {
        public RelayCommand PreviousViewModelCommand { get; set; }

        /// <summary>
        /// Permet de contrôler la propriété Data
        /// </summary>
        private string _data;
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
            }
        }

        public SecondViewModel()
        {
            PreviousViewModelCommand = new RelayCommand(GoBack, CanGoBack);
        }

        public void LoadData()
        {
            Data = "Sans paramétres";
        }

        public void LoadData(string data)
        {
            Data = data;
        }

        public void LoadData(BaseData data, bool valueBool)
        {
            Data = data.Libelle;
        }

        private bool CanGoBack()
        {
            return SimpleIoc.Default.GetInstance<INavigationStandard>().CanGoBack();
        }

        //Permet de revenir sur le premier ViewModel
        private void GoBack()
        {
            if (SimpleIoc.Default.GetInstance<INavigationStandard>().GetTypeViewModelToBack() == typeof(FirstViewModel))
            {
                SimpleIoc.Default.GetInstance<INavigationStandard>().GoBack("SetIsRetour", new object[] { true });
            }
            else
            {
                SimpleIoc.Default.GetInstance<INavigationStandard>().GoBack();
            }
        }
    }
}
