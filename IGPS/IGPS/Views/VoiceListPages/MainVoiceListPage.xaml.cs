using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;
using IGPS.Models;
using IGPS.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainVoiceListPage : ContentPage
    {
        private VoiceListItem previousItem;

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

            var item = previousItem = e.CurrentSelection.FirstOrDefault() as VoiceListItem;

            (sender as CollectionView).SelectedItem = null;

            await Navigation.PushAsync(new VoiceRecordDetailPage(item));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainVoiceListViewModel).SetStatus();

            ListCollectionView.ItemsSource = null;
            ListCollectionView.ItemsSource = (BindingContext as MainVoiceListViewModel).ListItems;

            if (previousItem != null)
            {
                ListCollectionView.ScrollTo(previousItem, animate:false);
            }

            //            try
            //            {
            //                await Task.Delay(100);

            //                Configurations.LoadingConfig = new LoadingConfig
            //                {
            //                    IndicatorColor = Color.AliceBlue,
            //                    OverlayColor = Color.Gray,
            //                    Opacity = 0.6,
            //                    DefaultMessage = AppResources.ServerChecking
            //                };

            //                await Loading.Instance.StartAsync(async (progress) =>
            //                {
            //                    if (previousItem == null)
            //                    {
            //                        await (BindingContext as MainVoiceListViewModel).UpdateStatus();
            //                    }
            //                    else
            //                    {
            //                        await (BindingContext as MainVoiceListViewModel).UpdateStatus(previousItem);
            //                    }

            //                    await Task.Delay(100);
            //                });
            //                MainThread.BeginInvokeOnMainThread(() =>
            //                {
            //                    ListCollectionView.ItemsSource = null;
            //                    ListCollectionView.ItemsSource = (BindingContext as MainVoiceListViewModel).ListItems;
            //                });
            //            }
            //            catch (Exception ex)
            //            {
            //#if DEBUG
            //                DependencyService.Get<IToast>().Show(ex.ToString());
            //#endif
            //                DependencyService.Get<IToast>().Show(AppResources.WorkFail);
            //            }
            //            finally
            //            {
            //                Loading.Instance.Hide();
            //                previousItem = null;
            //            }
        }
    }
}