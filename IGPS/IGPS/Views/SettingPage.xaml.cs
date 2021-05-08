using IGPS.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            Title = AppResources.SettingPage_Title;

            BindingContext = new SettingViewModel();
        }
    }
}