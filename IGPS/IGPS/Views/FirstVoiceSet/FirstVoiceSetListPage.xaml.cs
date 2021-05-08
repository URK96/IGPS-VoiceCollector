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
            //                    await (BindingContext as FirstVoiceSetListViewModel).UpdateStatus();

            //                    await Task.Delay(100);
            //                });
            //                MainThread.BeginInvokeOnMainThread(() =>
            //                {
            //                    ListCollectionView.ItemsSource = null;
            //                    ListCollectionView.ItemsSource = (BindingContext as FirstVoiceSetListViewModel).ListItems;
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
            //            }
        }
    }
}