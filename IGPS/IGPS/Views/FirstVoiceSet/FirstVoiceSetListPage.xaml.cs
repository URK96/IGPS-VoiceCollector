using IGPS.Models;
using IGPS.ViewModels;

using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstVoiceSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstVoiceSetListPage : ContentPage
    {
        public FirstVoiceSetListPage(ChapterItem item)
        {
            InitializeComponent();

            BindingContext = new FirstVoiceSetListViewModel(item);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as VoiceListItem;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new FirstVoiceSetRecordPage(item));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as FirstVoiceSetListViewModel).SetStatus();

            ListCollectionView.ItemsSource = null;
            ListCollectionView.ItemsSource = (BindingContext as FirstVoiceSetListViewModel).ListItems;
        }
    }
}