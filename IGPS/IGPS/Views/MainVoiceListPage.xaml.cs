using IGPS.Models;
using IGPS.ViewModels;
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
    public partial class MainVoiceListPage : ContentPage
    {
        public MainVoiceListPage(List<VoiceDataItem> list)
        {
            InitializeComponent();

            BindingContext = new MainVoiceListViewModel(list);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as VoiceDataItem;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new VoiceRecordDetailPage(item));
        }
    }
}