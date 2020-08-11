using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IGPS.ViewModels;

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
        }
    }
}