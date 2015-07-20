﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CommonMobiles.Messages;
using GalaSoft.MvvmLight.Messaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Navegar.UAP.Exemple.CRM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        public LandingPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<MessageLogin>(this, OnReceiveMessage);
        }

        private async void OnReceiveMessage(MessageLogin obj)
        {
            var message = new MessageDialog(obj.Message, "CRM");
            await message.ShowAsync();
        }
    }
}
