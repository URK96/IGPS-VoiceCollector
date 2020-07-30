using IGPS.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        public List<MainMenuItem> menuItems;
        public ListView MainMenu => MainMenuPageListView;

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<MainMenuItem>()
            {
                new MainMenuItem {Id = MenuItemType.Home, Title = AppResources.Main_MenuPage_Home },
                new MainMenuItem {Id = MenuItemType.RecordList, Title = AppResources.Main_MenuPage_RecordList},
                new MainMenuItem {Id = MenuItemType.Setting, Title = AppResources.Main_MenuPage_Setting},
                new MainMenuItem {Id = MenuItemType.About, Title = AppResources.Main_MenuPage_About }
            };

            MainMenuPageListView.ItemsSource = menuItems;

            MainMenuPageListView.SelectedItem = menuItems[0];
            MainMenuPageListView.ItemTapped += MainMenuPageListView_ItemTapped;
        }

        private async void MainMenuPageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await RootPage.NavigateFromMenu(e.ItemIndex);
        }
    }
}