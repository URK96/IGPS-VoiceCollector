﻿using IGPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainVoiceSectionPage : ContentPage
    {
        public MainVoiceSectionPage()
        {
            InitializeComponent();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as IGrouping<int, VoiceDataItem>;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new MainVoiceListPage(item.ToList()), true);
        }
    }
}