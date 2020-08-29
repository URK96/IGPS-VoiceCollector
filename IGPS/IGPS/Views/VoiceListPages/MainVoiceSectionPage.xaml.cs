using IGPS.Models;
using IGPS.ViewModels;
using IGPS.Views.FirstVoiceSet;
using IGPS.Views.VoiceListPages;

using System.Linq;

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

            BindingContext = new MainVoiceSectionViewModel();
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
            {
                return;
            }

            var item = e.CurrentSelection.FirstOrDefault() as SectionItem;

            (sender as CollectionView).SelectedItem = null;

            if (item.Section == 0)
            {
                await Navigation.PushAsync(new FirstVoiceSetListPage(new ChapterItem(item.Section, 0, 0, 8)), true);
            }
            else
            {
                await Navigation.PushAsync(new MainVoiceChapterPage(item.Section), true);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainVoiceSectionViewModel).CalcProgress();

            ListCollectionView.ItemsSource = null;
            ListCollectionView.ItemsSource = (BindingContext as MainVoiceSectionViewModel).SectionItems;
        }
    }
}