
using IGPS.Models;
using IGPS.ViewModels;

using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.VoiceListPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainVoiceChapterPage : ContentPage
    {
        public MainVoiceChapterPage(int key)
        {
            InitializeComponent();

            BindingContext = new MainVoiceChapterViewModel(key);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainVoiceChapterViewModel).CalcProgress();

            ListCollectionView.ItemsSource = null;
            ListCollectionView.ItemsSource = (BindingContext as MainVoiceChapterViewModel).ChapterItems;
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as ChapterItem;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new MainVoiceListPage(item), true);
        }
    }
}