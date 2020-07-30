using IGPS.Models;
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
    public partial class MainHomePage : ContentPage
    {
        MainPage RootPage => Application.Current.MainPage as MainPage;
        MenuPage MainMenuPage => RootPage.Master as MenuPage;

        public MainHomePage()
        {
            InitializeComponent();
        }

        private async void GoRecordButton_Clicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.RecordList);
            //MainMenuPage.MainMenu.SelectedItem = MainMenuPage.menuItems[1];
        }
    }
}