using IGPS.Models;
using IGPS.ViewModels;

using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainVoiceListPage : ContentPage
    {
        public MainVoiceListPage(ChapterItem item)
        {
            InitializeComponent();

            BindingContext = new MainVoiceListViewModel(item);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as VoiceListItem;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new VoiceRecordDetailPage(item));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainVoiceListViewModel).SetStatus();

            ListCollectionView.ItemsSource = null;
            ListCollectionView.ItemsSource = (BindingContext as MainVoiceListViewModel).ListItems;
        }
    }
}