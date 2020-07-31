using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using IGPS.Models;

namespace IGPS.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> menuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();

            menuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            await Task.Delay(10);

            if (!menuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        menuPages.Add(id, new NavigationPage(new MainHomePage()));
                        break;
                    case (int)MenuItemType.RecordList:
                        menuPages.Add(id, new NavigationPage(new MainVoiceSectionPage()));
                        break;
                    case (int)MenuItemType.Setting:
                        // Add page code
                        return;
                        break;
                    case (int)MenuItemType.About:
                        menuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = menuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                //if (Device.RuntimePlatform == Device.Android)
                //{
                //    await Task.Delay(100);
                //}
            }

            IsPresented = false;
        }
    }
}