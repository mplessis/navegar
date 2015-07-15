﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Navegar.XamarinForms.Exemple.CRM.Messages;
using Xamarin.Forms;

namespace Navegar.XamarinForms.Exemple.CRM.Views
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            Messenger.Default.Register<MessageLogin>(this, OnReceiveMessage);
        }

        private async void OnReceiveMessage(MessageLogin obj)
        {
            await DisplayAlert("CRM", obj.Message, "Fermer");
        }
    }
}